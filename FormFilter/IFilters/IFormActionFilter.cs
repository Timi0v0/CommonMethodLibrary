using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFilter.IFilters
{
    /// <summary>
    ///定义一个行为过滤器接口
    /// </summary>
    internal interface IFormActionFilter: IFilterMateData
    {
        /// <summary>
        /// 窗体执行前进行的操作
        /// </summary>
        /// <param name="context">过滤器上下文</param>
        void OnFormActionExecuting(Form form);

        /// <summary>
        /// 窗体执行后进行的操作
        /// </summary>
        /// <param name="form"></param>
        void OnFormActionExecuted(Form form);
    }
}
