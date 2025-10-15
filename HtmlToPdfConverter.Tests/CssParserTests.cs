using HtmlToPdfConverter.Extractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlToPdfConverter.Tests
{
    [TestClass]
    public class CssParserTests
    {
        [TestMethod]
        public void Parse_SimpleClass_ReturnsCorrectStyle()
        {
            var css = ".my-class { color: #FF0000; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey("my-class"));
            Assert.AreEqual("#FF0000", result["my-class"].Color);
        }

        [TestMethod]
        public void Parse_MultipleClasses_ReturnsAllStyles()
        {
            var css = ".class1 { font-size: 16px; } .class2 { font-weight: bold; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("16px", result["class1"].FontSize);
            Assert.AreEqual("bold", result["class2"].FontWeight);
        }

        [TestMethod]
        public void Parse_ClassWithMultipleProperties_ReturnsAllProperties()
        {
            var css = ".complex { color: blue; font-size: 1.2em; text-align: center; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(1, result.Count);
            var style = result["complex"];
            Assert.AreEqual("blue", style.Color);
            Assert.AreEqual("1.2em", style.FontSize);
            Assert.AreEqual("center", style.TextAlign);
        }

        [TestMethod]
        public void Parse_CssWithComments_IgnoresComments()
        {
            var css = "/* This is a comment */ .commented { color: green; } /* another comment */";
            var result = CssParser.Parse(css);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.ContainsKey("commented"));
            Assert.AreEqual("green", result["commented"].Color);
        }

        [TestMethod]
        public void Parse_EmptyCssString_ReturnsEmptyDictionary()
        {
            var css = "";
            var result = CssParser.Parse(css);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Parse_CssWithNoClassSelectors_ReturnsEmptyDictionary()
        {
            var css = "body { margin: 0; } p { line-height: 1.5; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Parse_MalformedCss_HandlesGracefully()
        {
            var css = ".malformed { color: red; font-size: ; } .good { color: blue; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("malformed"));
            Assert.IsTrue(result.ContainsKey("good"));
            Assert.AreEqual("red", result["malformed"].Color);
            Assert.AreEqual("blue", result["good"].Color);
        }

        [TestMethod]
        public void Parse_ClassWithHyphenAndUnderscore_ParsesCorrectly()
        {
            var css = ".class-with-hyphen { color: #333; } .class_with_underscore { background-color: #FFF; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey("class-with-hyphen"));
            Assert.IsTrue(result.ContainsKey("class_with_underscore"));
        }

        [TestMethod]
        public void Parse_DuplicateClassDefinition_LastOneWins()
        {
            var css = ".duplicate { color: red; } .duplicate { color: purple; }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("purple", result["duplicate"].Color);
        }

        [TestMethod]
        public void Parse_PropertiesWithVariousFormats_ExtractsCorrectly()
        {
            var css = @".various {
                font-family: 'Times New Roman', serif;
                padding: 10px 20px;
                border: 1px solid black;
            }";
            var result = CssParser.Parse(css);
            Assert.AreEqual(1, result.Count);
            var style = result["various"];
            Assert.AreEqual("'Times New Roman', serif", style.FontFamily);
            Assert.AreEqual("10px 20px", style.Padding);
            Assert.AreEqual("1px solid black", style.Border);
        }
    }
}
