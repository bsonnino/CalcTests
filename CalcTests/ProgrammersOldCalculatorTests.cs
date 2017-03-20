using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace CalcTests
{
    [TestClass]
    public class ProgrammersOldCalculatorTests
    {
        private static WindowsDriver<WindowsElement> _calculatorSession;

        [ClassInitialize]
        public static void StartCalculator(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "calc.exe");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            _calculatorSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
            _calculatorSession.FindElementByName("Calculator").SendKeys(Keys.Alt+"3");
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
            _calculatorSession.FindElementByName("Decimal").Click();
            _calculatorSession.FindElementByName("Clear").Click();
        }

        [DataTestMethod]
        [DataRow("Hexadecimal","C")]
        [DataRow("Octal","14")]
        [DataRow("Binary","1100")]
        public void Number12ShouldConvertOkToHexOctalAndBinary(string buttonId, string result)
        {
            _calculatorSession.FindElementByName("1").Click();
            _calculatorSession.FindElementByName("2").Click();
            _calculatorSession.FindElementByName(buttonId).Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual);
        }

        private static string GetDisplayText()
        {
            return _calculatorSession.FindElementByAccessibilityId("Result").Text.Trim();
        }

        [TestMethod]
        public void InDecimalModeLetterButtonsShouldBeDisabled()
        {
            var disabledButtons = new[] {"A", "B", "C", "D", "E", "F"};
            foreach (var buttonName in disabledButtons)
            {
               Assert.IsFalse(_calculatorSession.FindElementByName(buttonName).Enabled); 
            }
        }

        [TestMethod]
        public void InHexModeAllButtonsShouldBeEnabled()
        {
            var enabledButtons = new[] {"1","2","3" ,"4","5","6",
                "7","8","9","0","A", "B", "C", "D", "E", "F" };
            _calculatorSession.FindElementByAccessibilityId("hexButton").Click();
            foreach (var buttonName in enabledButtons)
            {
                Assert.IsTrue(_calculatorSession.FindElementByName(buttonName).Enabled, $"Test for {buttonName}");
            }
        }

        [TestMethod]
        public void InOctalModeButtonsZeroToSevenShouldBeEnabled()
        {
            var enabledButtons = new[] { "1", "2", "3", "4", "5", "6",
                "7",  "0" };
            _calculatorSession.FindElementByAccessibilityId("octolButton").Click();
            foreach (var buttonName in enabledButtons)
            {
                Assert.IsTrue(_calculatorSession.FindElementByName(buttonName).Enabled, $"Test for {buttonName}");
            }
        }

        [TestMethod]
        public void InBinaryModeAllButtonsExceptZeroAndOneToSevenShouldBeDisabled()
        {
            var disabledButtons = new[] {"2", "3", "4", "5", "6",
                "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            _calculatorSession.FindElementByAccessibilityId("binaryButton").Click();
            foreach (var buttonName in disabledButtons)
            {
                Assert.IsFalse(_calculatorSession.FindElementByName(buttonName).Enabled, $"Test for {buttonName}");
            }
        }

        [DataTestMethod]
        [DataRow("0","-1")]
        [DataRow("1","-2")]
        [DataRow("9","-10")]
        public void NotTestsForPositiveNumbersShouldBeOk(string key, string result)
        {
            _calculatorSession.FindElementByName(key).Click();
            _calculatorSession.FindElementByName("Not").Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual,$"Test for key {key}");
        }

        [DataTestMethod]
        [DataRow("1", "0")]
        [DataRow("2", "1")]
        [DataRow("9", "8")]
        public void NotTestsForNegativeNumbersShouldBeOk(string key, string result)
        {
            _calculatorSession.FindElementByName(key).Click();
            _calculatorSession.FindElementByName("Negate").Click();
            _calculatorSession.FindElementByName("Not").Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual, $"Test for negative {key}");
        }

        
        [DataTestMethod]
        [DataRow("1","And","0","0")]
        [DataRow("1","And","1","1")]
        [DataRow("1","And","3","1")]
        [DataRow("1", "Or", "0", "1")]
        [DataRow("1", "Or", "1", "1")]
        [DataRow("1", "Or", "3", "3")]
        [DataRow("1", "Exclusive or", "0", "1")]
        [DataRow("1", "Exclusive or", "1", "0")]
        [DataRow("1", "Exclusive or", "3", "2")]
        [DataRow("1", "Left shift", "0", "1")]
        [DataRow("1", "Left shift", "1", "2")]
        [DataRow("1", "Left shift", "3", "8")]
        [DataRow("1", "Right shift", "0", "1")]
        [DataRow("1", "Right shift", "1", "0")]
        [DataRow("1", "Right shift", "3", "0")]
        public void TestsForOperatorsShouldBeOk(string first, string oper, string second, string result)
        {
            _calculatorSession.FindElementByName(first).Click();
            _calculatorSession.FindElementByName(oper).Click();
            _calculatorSession.FindElementByName(second).Click();
            _calculatorSession.FindElementByName("Equals").Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual,$"Test for {first} {oper} {second}");
        }
        
    }
}
