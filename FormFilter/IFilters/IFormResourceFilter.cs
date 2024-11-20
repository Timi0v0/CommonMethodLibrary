using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFilter.IFilters
{
    public interface IFormResourceFilter:IFilterMateData
    {
        /// <summary>
        /// 窗体资源执行前进行的操作
        /// </summary>
        /// <param name="form"></param>
        void OnFormResourceExecuting(FormResouceContext context);
        /// <summary>
        /// 窗体资源执行后进行的操作
        /// </summary>
        /// <param name="form"></param>
        void OnFormResourceExecuted(FormResouceContext context);

    }
}
