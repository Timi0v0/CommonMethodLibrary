using System.Runtime.ExceptionServices;

namespace FormFilter.IFilters
{
    public class FormExceptionDispatchInfo
    {
        private readonly Exception _exception;
        public Exception Exception => _exception;

        public FormExceptionDispatchInfo(Exception exception)
        {
            _exception = exception;
        }

        public static FormExceptionDispatchInfo Capture(Exception exception)
        {
            return new FormExceptionDispatchInfo(exception);
        }
    }
}