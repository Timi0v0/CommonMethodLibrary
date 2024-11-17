using System.Runtime.ExceptionServices;

namespace FormFilter.IFilters
{
    public class ExceptionContext
    {
        private Exception? exception;

        private FormExceptionDispatchInfo? dispatchInfo;

        public Exception? Exception
        {
            get
            {
                if (exception == null && dispatchInfo != null)
                {
                    return dispatchInfo.Exception;
                }
                else
                {
                    return exception;
                }
            }
            set
            {
                exception = value;
                dispatchInfo = null;
            }
        }

        public FormExceptionDispatchInfo? ExceptionDispatchInfo
        {
            get
            {
                return dispatchInfo;
            }
            set
            {
                dispatchInfo = value;
                exception = null;
            }
        }


    }
}