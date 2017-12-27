using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JE.Logging.Test
{
    class Program
    {
        //Declare an instance for log4net
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            ImplementLoggingFuntion();
        }


        [JELoggingAttribute(Jurisdiction = "NJ", CampaignOfferType = "NEW", Business = "NewBusiness")]
        private static void ImplementLoggingFuntion()
        {
            object[] args = { "test", "abc", 1233 };
            Log.JemLogFormat("Start {0}Log {1}JEM {2}Format", args);
        }
    }
}
