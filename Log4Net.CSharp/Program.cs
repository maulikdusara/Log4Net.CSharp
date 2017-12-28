using System;
using log4net;
using System.Threading;
using System.Collections.Generic;

namespace Log4Net.CSharp
{

    [Logging(CategoryName = "Jurisdiction", CategoryValue = "NJ")]
    public class Program
    {
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            ImplementLoggingFuntion();
        }

        
        [Logging(CategoryName = "Business", CategoryValue = "New")]
        private static void ImplementLoggingFuntion()
        {
            Log.JELog("JE Logs with Custom Attributes");
        }
    }
}
