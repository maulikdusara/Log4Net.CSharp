using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Net.CSharp
{
    [System.AttributeUsage(
        System.AttributeTargets.Class |
            System.AttributeTargets.Method,
        AllowMultiple = true)
    ]
    public class LoggingAttribute : Attribute
    {
        public string CategoryName { get; set; }
        public string CategoryValue { get; set; }

        public LoggingAttribute()
        {
        }

        public override string ToString()
        {
            return string.Format("{0}={1}",CategoryName,CategoryValue);
        }
    }
}
