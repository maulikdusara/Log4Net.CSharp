using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetExtension
{
    [AttributeUsage(AttributeTargets.Class | 
                    AttributeTargets.Method |
                    AttributeTargets.Interface)]
    public class LogCategoryAttribute : Attribute
    {
        public string[] Data { get; set; }

        public LogCategoryAttribute()
        {

        }

        public LogCategoryAttribute(params string[] categories)
        {
            this.Data = categories;
        }

    }
}