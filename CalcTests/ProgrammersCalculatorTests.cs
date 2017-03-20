using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace CalcTests
{
    [TestClass]
    public class ProgrammersCalculatorTests
    {
        private static WindowsDriver<WindowsElement> _calculatorSession;

        [ClassInitialize]
        public static void StartCalculator(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            _calculatorSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);
            _calculatorSession.FindElementByAccessibilityId("NavButton").Click();
            _calculatorSession.FindElementByName("Programmer Calculator").Click();
            _calculatorSession.FindElementByAccessibilityId("decimalButton").Click();
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
            _calculatorSession.FindElementByAccessibilityId("decimalButton").Click();
            _calculatorSession.FindElementByName("Clear").Click();
        }

        [DataTestMethod]
        [DataRow("hexButton","C")]
        [DataRow("octolButton","14")]
        [DataRow("binaryButton","1100")]
        public void Number12ShouldConvertOkToHexOctalAndBinary(string buttonId, string result)
        {
            _calculatorSession.FindElementByName("One").Click();
            _calculatorSession.FindElementByName("Two").Click();
            _calculatorSession.FindElementByAccessibilityId(buttonId).Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual);
        }

        private static string GetDisplayText()
        {
            return _calculatorSession.FindElementByAccessibilityId("CalculatorResults")
                .Text.Replace("Display is", "").Trim();
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
            var enabledButtons = new[] {"One","Two","Three" ,"Four","Five","Six",
                "Seven","Eight","Nine","Zero","A", "B", "C", "D", "E", "F" };
            _calculatorSession.FindElementByAccessibilityId("hexButton").Click();
            foreach (var buttonName in enabledButtons)
            {
                Assert.IsTrue(_calculatorSession.FindElementByName(buttonName).Enabled, $"Test for {buttonName}");
            }
        }

        [TestMethod]
        public void InOctalModeButtonsZeroToSevenShouldBeEnabled()
        {
            var enabledButtons = new[] { "One", "Two", "Three", "Four", "Five", "Six",
                "Seven",  "Zero" };
            _calculatorSession.FindElementByAccessibilityId("octolButton").Click();
            foreach (var buttonName in enabledButtons)
            {
                Assert.IsTrue(_calculatorSession.FindElementByName(buttonName).Enabled, $"Test for {buttonName}");
            }
        }

        [TestMethod]
        public void InBinaryModeAllButtonsExceptZeroAndOneToSevenShouldBeDisabled()
        {
            var disabledButtons = new[] {"Two", "Three", "Four", "Five", "Six",
                "Seven", "Eight", "Nine", "A", "B", "C", "D", "E", "F" };
            _calculatorSession.FindElementByAccessibilityId("binaryButton").Click();
            foreach (var buttonName in disabledButtons)
            {
                Assert.IsFalse(_calculatorSession.FindElementByName(buttonName).Enabled, $"Test for {buttonName}");
            }
        }

        [DataTestMethod]
        [DataRow("Zero","-1")]
        [DataRow("One","-2")]
        [DataRow("Nine","-10")]
        public void NotTestsForPositiveNumbersShouldBeOk(string key, string result)
        {
            _calculatorSession.FindElementByName(key).Click();
            _calculatorSession.FindElementByName("Not").Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual,$"Test for key {key}");
        }

        [DataTestMethod]
        [DataRow("One", "0")]
        [DataRow("Two", "1")]
        [DataRow("Nine", "8")]
        public void NotTestsForNegativeNumbersShouldBeOk(string key, string result)
        {
            _calculatorSession.FindElementByName(key).Click();
            _calculatorSession.FindElementByName("Positive Negative").Click();
            _calculatorSession.FindElementByName("Not").Click();
            var actual = GetDisplayText();
            Assert.AreEqual(result, actual, $"Test for negative {key}");
        }

        
        [DataTestMethod]
        [DataRow("One","And","Zero","0")]
        [DataRow("One","And","One","1")]
        [DataRow("One","And","Three","1")]
        [DataRow("One", "Or", "Zero", "1")]
        [DataRow("One", "Or", "One", "1")]
        [DataRow("One", "Or", "Three", "3")]
        [DataRow("One", "Exclusive or", "Zero", "1")]
        [DataRow("One", "Exclusive or", "One", "0")]
        [DataRow("One", "Exclusive or", "Three", "2")]
        [DataRow("One", "Left shift", "Zero", "1")]
        [DataRow("One", "Left shift", "One", "2")]
        [DataRow("One", "Left shift", "Three", "8")]
        [DataRow("One", "Right shift", "Zero", "1")]
        [DataRow("One", "Right shift", "One", "0")]
        [DataRow("One", "Right shift", "Three", "0")]
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
