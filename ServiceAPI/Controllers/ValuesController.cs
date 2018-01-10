using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Log4NetExtension;
using ServiceAPI.Models;

namespace ServiceAPI.Controllers
{

    [LogCategory("Product:Values","Business:Sales")]
    public class ValuesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET api/values
        [Route("api/getAllValues")]
        public IEnumerable<string> Get()
        {
            Log.LogA("GET api/values");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            Log.LogA("GET api/values/5");
            return "value";
        }

        [Route("api/log/debug/object")]
        public string logDebugObject()
        {
            ExternalLoginViewModel logObject = new ExternalLoginViewModel()
            {
                Name = "logObject",
                State = "NY",
                Url = "API Acccess URL: api/log/debug/object"
            };
            Log.DebugA(logObject);
            return "value";
        }
    }
}
