using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFilter
{
    /// <summary>
    /// 窗体运行适配器
    /// </summary>
    public class FormRunAdapter
    {
        public void RunWithFilters<T>(Action<T> action) where T : Form
        {
            var filterInvoke = new FilterInvoke<T>(action);
        }
    }
}
