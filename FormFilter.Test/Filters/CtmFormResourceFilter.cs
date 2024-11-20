using FormFilter.IFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFilter.Test.Filters
{
    public class CtmFormResourceFilter : Attribute, IFormResourceFilter
    {
        public void OnFormResourceExecuted(FormResouceContext context)
        {
        }

        public void OnFormResourceExecuting(FormResouceContext context)
        {
        }
    }
}
