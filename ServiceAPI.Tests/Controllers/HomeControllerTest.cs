﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceAPI;
using ServiceAPI.Controllers;

namespace ServiceAPI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeController_ClassLevel_LogSuccess()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }

        [TestMethod]
        public void HomeController_ParentInherit_WithMethod_LogSuccess()
        {
            HomeController controller = new HomeController();

            JsonResult result = controller.helloWorld() as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Usage of Log4net extension", result.Data);
        }
    }
}
