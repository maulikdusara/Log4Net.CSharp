using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetExtension
{
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