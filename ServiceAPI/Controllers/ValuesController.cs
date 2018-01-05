using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Log4NetExtension;

namespace ServiceAPI.Controllers
{

    [LogCategory("Product:Values","Business:Sales")]
    public class ValuesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET api/values
        public IEnumerable<string> Get()
        {
            Log.LogAttr("GET api/values");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            Log.LogAttr("GET api/values/5");
            return "value";
        }
    }
}
