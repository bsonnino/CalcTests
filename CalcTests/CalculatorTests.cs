using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace CalcTests
{
    [TestClass]
    public class CalculatorTests
    {
        private static WindowsDriver<WindowsElement> _calculatorSession;

        [TestMethod]
        public void CalculatorIsNotNull()
        {
            Assert.IsNotNull(_calculatorSession);
        }

        [DataTestMethod]
        [DataRow("One", "1")]
        [DataRow("Two", "2")]
        [DataRow("Three", "3")]
        [DataRow("Four", "4")]
        [DataRow("Five", "5")]
        [DataRow("Six", "6")]
        [DataRow("Seven", "7")]
        [DataRow("Eight", "8")]
        [DataRow("Nine", "9")]
        [DataRow("Zero", "0")]
        public void CalculatorKeysShouldDisplay(string key, string expected)
        {
            _calculatorSession.FindElementByName(key).Click();
            var actual = _calculatorSession.FindElementByAccessibilityId("CalculatorResults")
                .Text.Replace("Display is", "").Trim();
            Assert.AreEqual(expected, actual);
        }

        [ClassInitialize]
        public static void StartCalculator(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            _calculatorSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
        }

        [ClassCleanup]
        public static void CloseCalculator()
        {
            _calculatorSession.Dispose();
            _calculatorSession = null;
        }

        [TestInitialize]
        public void ResetCalculator()
        {
            _calculatorSession.FindElementByAccessibilityId("NavButton").Click();
            _calculatorSession.FindElementByName("Standard Calculator").Click();
            _calculatorSession.FindElementByName("Clear").Click();
        }
    }
}
