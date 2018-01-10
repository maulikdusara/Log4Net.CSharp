using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceAPI;
using ServiceAPI.Controllers;

namespace ServiceAPI.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void ValueController_ParentWithCustomRoute_Log()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void ValueController_ParentInherit_withDefaultRoute_Log()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void ValueController_ParentInherit_withDefaultRoute_Debug()
        {
            // Arrange
            ValuesController controller = new ValuesController();

            // Act
            string result = controller.logDebugObject();

            // Assert
            Assert.AreEqual("value", result);
        }

    }
}
