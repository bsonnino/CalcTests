using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace CalcTests
{
    [TestClass]
    public class OldCalculatorTests
    {
        private static WindowsDriver<WindowsElement> _calculatorSession;

        [TestMethod]
        public void CalculatorIsNotNull()
        {
            Assert.IsNotNull(_calculatorSession);
        }

        [DataTestMethod]
        [DataRow("1", "1")]
        [DataRow("2", "2")]
        [DataRow("3", "3")]
        [DataRow("4", "4")]
        [DataRow("5", "5")]
        [DataRow("6", "6")]
        [DataRow("7", "7")]
        [DataRow("8", "8")]
        [DataRow("9", "9")]
        [DataRow("0", "0")]
        public void CalculatorKeysShouldDisplay(string key, string expected)
        {
            _calculatorSession.FindElementByName(key).Click();
            var actual = _calculatorSession.FindElementByName("Result").Text.Trim();
            Assert.AreEqual(expected, actual);
        }

        [ClassInitialize]
        public static void StartCalculator(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "calc.exe");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            _calculatorSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
            _calculatorSession.FindElementByName("Calculator").SendKeys(Keys.Alt+"1");
            _calculatorSession.FindElementByName("Clear").Click();
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
            _calculatorSession.FindElementByName("Clear").Click();
        }
    }
}
