using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormFilter
{
    /// <summary>
    /// 窗体资源上下文
    /// </summary>
    public class FormResouceContext
    {

        //窗体名称
        public string? FormName { get; set; }
        //窗体类型
        public Type? FormType { get; set; }

        public FormResouceContext InitContext<T>()
        {
            FormName = typeof(T).Name;
            FormType = typeof(T);
            return this;
        }
    }
}
