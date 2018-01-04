﻿using System;
using log4net;
using System.Threading;
using System.Collections.Generic;

namespace Log4NetExtension
{

    [LogAttribute("Class Category", "Class SubCatName", "Class LastCatName", "Business:New", "Jurisdiction:NY")]
    public class Program
    {
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
        {
            ImplementLoggingFuntion();
        }


        //[LogAttribute("CatName", "SubCatName", "LastCatName", "Process:GeneratePDF")]
        private static void ImplementLoggingFuntion()
        {
            Log.LogAttr("Generate PDF Function");
        }
    }
}
