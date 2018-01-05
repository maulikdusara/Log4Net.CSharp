using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Log4NetExtension;

namespace ServiceAPI.Controllers
{

    [LogCategory("Controller:Home", "Business:Sales")]
    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            Log.LogAttr("Custom Log",true);

            return View();
        }

        [HttpGet]
        [Route("hello/world")]
        [LogCategory("Method:HelloWorld","Client:MobileDevice")]
        public JsonResult helloWorld()
        {
            Log.LogAttr("Log Hello world with Custom URL re-writing with Method level Attributes");
            return Json("Usage of Log4net extension", JsonRequestBehavior.AllowGet);
        }
    }
}
