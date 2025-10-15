using HtmlToPdfConverter.Extractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace HtmlToPdfConverter.Tests
{
    [TestClass]
    public class CssParserTests
    {
        [TestMethod]
        public void Parse_SimpleCss_ReturnsCorrectStyles()
        {
            // Arrange
            var css = ".test-class { color: red; font-size: 12px; }";

            // Act
            var result = CssParser.Parse(css);

            // Assert
            Assert.IsTrue(result.ContainsKey("test-class"));
            var style = result["test-class"];
            Assert.AreEqual("red", style.Color);
            Assert.AreEqual("12px", style.FontSize);
        }

        [TestMethod]
        public void Parse_MultipleClasses_ReturnsCorrectStyles()
        {
            // Arrange
            var css = @"
                .class1 { color: blue; }
                .class2 { font-weight: bold; }
            ";

            // Act
            var result = CssParser.Parse(css);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("blue", result["class1"].Color);
            Assert.AreEqual("bold", result["class2"].FontWeight);
        }

        [TestMethod]
        public void Parse_EmptyCss_ReturnsEmptyDictionary()
        {
            // Arrange
            var css = "";

            // Act
            var result = CssParser.Parse(css);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Parse_CssWithNoClasses_ReturnsEmptyDictionary()
        {
            // Arrange
            var css = "body { margin: 0; }";

            // Act
            var result = CssParser.Parse(css);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
