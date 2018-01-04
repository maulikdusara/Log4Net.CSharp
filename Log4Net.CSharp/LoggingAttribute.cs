using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetExtension
{
    public class LogAttribute : Attribute
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string LastCategoryName { get; set; }
        public string[] Data { get; set; }

        public LogAttribute()
        {

        }

        public LogAttribute(string categoryName, string subCategoryName, string lastCategoryName, params string[] additionalProperties)
        {
            this.CategoryName = categoryName;
            this.SubCategoryName = subCategoryName;
            this.LastCategoryName = lastCategoryName;
            this.Data = additionalProperties;
        }

    }
}