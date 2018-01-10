using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Log4NetExtension;
using ServiceAPI.Models;

namespace ServiceAPI.Controllers
{

    [LogCategory("Controller:Home", "Business:Sales")]
    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            Log.LogA("Custom Log",true);

            return View();
        }

        [HttpGet]
        [Route("hello/world")]
        [LogCategory("Method:HelloWorld","Client:MobileDevice")]
        public JsonResult helloWorld()
        {
            ExternalLoginViewModel logObject = new ExternalLoginViewModel()
            {
                Name = "Hello World",
                State = "NY",
                Url ="hello/World/{Identity ID}"
            };

            Log.LogA(logObject,true);
            return Json("Usage of Log4net extension", JsonRequestBehavior.AllowGet);
        }
    }
}
