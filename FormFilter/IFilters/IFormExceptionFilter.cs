using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormFilter.IFilters
{
    public interface IFormExceptionFilter: IFilterMateData
    {
        public void OnFormException(Form form, ExceptionContext exceptionContext);
    }
}
