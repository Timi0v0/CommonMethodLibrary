using FormFilter.IFilters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormFilter
{
    public class FilterInvoke<T> where T : Form
    {
        private T? _form;
        private readonly Action<T> _action;
        private IFilterMateData[] _filters;
        private int _index = 0;
        private ExceptionContext? _exceptionContext;
        private FormResouceContext _formResouceContext;
        public FilterInvoke(Action<T> action)
        {
            _action = action;
            _formResouceContext= new FormResouceContext().InitContext<T>();
            var ctmFilters =typeof(T).GetCustomAttributes(false).Where(a => a is IFilterMateData && !a.GetType().IsAbstract).ToArray();
            _filters = new IFilterMateData[ctmFilters.Length];
            for (int i = 0; i < ctmFilters.Length; i++)
            {
                object[] ctmFilters1 = ctmFilters;
                _filters[i] = ctmFilters1[i] as IFilterMateData ?? throw new InvalidOperationException("Filter must implement IFilterMateData interface.");
            }
            //设置初始节点
            State next = State.ResourceFilterBefore;
            //设置初始绘画状态
            object? currFilter = null;
            //标记过滤器是否完结
            bool isCompleted = false;
            if (!isCompleted)
            {
                Next(ref next, ref currFilter, ref isCompleted);
            }
        }

        private Task Next(ref State next, ref object? currFilter, ref bool isCompleted)
        {
            switch (next)
            {
                case State.ResourceFilterBefore:
                    {
                        Reset();
                        goto case State.ResourceFilterNext;
                    }
                case State.ResourceFilterNext:
                    {
                        //获取下一个过滤器
                        currFilter = GetNextFilter<IFormResourceFilter>();
                        if (currFilter != null)
                        {
                            goto case State.ResourceFilterBegin;
                        }
                        //当前的资源过滤器已经执行完毕
                        else
                        {
                            goto case State.ResourceFilterInside;
                        }
                    }
                case State.ResourceFilterBegin:
                    {
                        ((IFormResourceFilter)currFilter!).OnFormResourceExecuting(_formResouceContext!);
                        var task = InvokeResourceNextFilter();
                        task.Wait();
                        //如果任务没有执行完成
                        if (task.Status != TaskStatus.RanToCompletion)
                        {
                            next = State.ResourceFilterEnd;
                            return task;
                        }
                        goto case State.ResourceFilterEnd;
                    }
                case State.ResourceFilterInside:
                    {
                        goto case State.ExceptionFilterBefore;
                    }
                case State.ResourceFilterEnd:
                    {
                        ((IFormResourceFilter)currFilter!).OnFormResourceExecuted(_formResouceContext!);
                        isCompleted = true;
                        return Task.CompletedTask;
                    }
                case State.ExceptionFilterBefore:
                    {
                        Reset();
                        goto case State.ExceptionFilterNext;
                    }
                case State.ExceptionFilterNext:
                    {
                        currFilter = GetNextFilter<IFormExceptionFilter>();
                        if (currFilter != null)
                        {
                            goto case State.ExceptionFilterBegin;
                        }
                        else
                        {
                            goto case State.ExceptionFilterInside;
                        }
                    }
                case State.ExceptionFilterBegin:
                    {
                        var task = InvokeExceptionNextFilter();
                        task.Wait();
                        //如果任务没有执行完成
                        if (task.Status != TaskStatus.RanToCompletion)
                        {
                            next = State.ExceptionFilterEnd;
                            return task;
                        }
                        goto case State.ExceptionFilterEnd;
                    }
                case State.ExceptionFilterInside:
                    {
                        goto case State.ActionFilterBefore;
                    }
                case State.ExceptionFilterEnd:
                    {
                        if (_exceptionContext != null)
                        {
                            ((IFormExceptionFilter)currFilter!).OnFormException(_form!, _exceptionContext!);
                        }
                        isCompleted = true;
                        return Task.CompletedTask;
                    }
                case State.ActionFilterBefore:
                    {
                        Reset();
                        //通过反射创建当前需要操作的对象
                        // _form = (T)Activator.CreateInstance(typeof(T))!;
                        // 通过容器创建对象
                        _form= ServiceProviderHelper.serviceProvider!.GetRequiredService<T>();
                        if (_form == null)
                        {
                            throw new Exception();
                        }
                        goto case State.ActionFilterNext;
                    };
                case State.ActionFilterNext:
                    {
                        //获取下一个过滤器
                        currFilter = GetNextFilter<IFormActionFilter>();
                        if (currFilter != null)
                        {
                            goto case State.ActionFilterBegin;
                        }
                        //当前的资源过滤器已经执行完毕
                        else
                        {
                            goto case State.ActionFilterInside;
                        }
                    }
                case State.ActionFilterBegin:
                    {
                        //执行当前的过滤器
                        ((IFormActionFilter)currFilter!).OnFormActionExecuting(_form!);
                        //执行下一个同类的过滤器
                        var task = InvokeActionNextFilter();
                        task.Wait();
                        //如果任务没有执行完成
                        if (task.Status != TaskStatus.RanToCompletion)
                        {
                            next = State.ActionFilterEnd;
                            return task;
                        }
                        goto case State.ActionFilterEnd;
                    }
                case State.ActionFilterInside:
                    {
                        _action(_form!);
                        goto case State.ActionEnd;
                    }
                case State.ActionFilterEnd:
                    {
                        ((IFormActionFilter)currFilter!).OnFormActionExecuted(_form!);
                        isCompleted = true;
                        return Task.CompletedTask;
                    }
                case State.ActionEnd:
                    {
                        isCompleted = true;
                        return Task.CompletedTask;
                    }
                default:
                    throw new InvalidOperationException("Invalid state.");
            }
        }

        public void Reset()
        {
            _index = 0;
        }

        public TFilter? GetNextFilter<TFilter>() where TFilter : class, IFilterMateData
        {
            while (_index < _filters.Length)
            {
                var filter = _filters[_index] as TFilter;
                _index++;
                if (filter != null)
                {
                    return filter;
                }
            }
            return null;
        }
        /// <summary>
        /// 递归调用下一个过滤器
        /// </summary>
        /// <returns></returns>
        public async Task InvokeResourceNextFilter()
        {
            var next = State.ResourceFilterNext;
            object? state = null;
            var isCompleted = false;
            while (!isCompleted)
            {
                await Task.Run(() => Next(ref next, ref state, ref isCompleted));
            }
        }

        public async Task InvokeActionNextFilter()
        {
            var next = State.ActionFilterNext;
            object? state = null;
            var isCompleted = false;
            while (!isCompleted)
            {
                await Task.Run(() => Next(ref next, ref state, ref isCompleted));
            }
        }

        public async Task InvokeExceptionNextFilter()
        {
            try
            {
                var next = State.ExceptionFilterNext;
                object? state = null;
                var isCompleted = false;
                while (!isCompleted)
                {
                    await Task.Run(() => Next(ref next, ref state, ref isCompleted));
                }
            }
            catch (Exception ex)
            {
                _exceptionContext = new ExceptionContext()
                {
                    ExceptionDispatchInfo = FormExceptionDispatchInfo.Capture(ex)
                };
            }

        }
    }
}
