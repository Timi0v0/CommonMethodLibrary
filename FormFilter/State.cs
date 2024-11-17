using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFilter
{
    public enum State
    {
        ResourceFilterBefore,
        ResourceFilterNext,
        ResourceFilterBegin,
        ResourceFilterInside,
        ResourceFilterEnd,

        ExceptionFilterBefore,
        ExceptionFilterNext,
        ExceptionFilterInside,
        ExceptionFilterBegin,
        ExceptionFilterEnd,

        ActionFilterBefore,
        ActionFilterNext,
        ActionFilterBegin,
        ActionFilterInside,
        ActionFilterEnd,
        ActionEnd
    }
}
