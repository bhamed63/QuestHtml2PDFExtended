using HtmlToPdfConverter.Extractor;
using HtmlToPdfConverter.Models;

namespace HtmlToPdfConverter.Tests
{
    [TestClass]
    public class HtmlExtractorTests
    {
        [TestMethod]
        public void Extract_SimpleHtml_ReturnsCorrectStructure()
        {
            // Arrange
            var html = "<body><p>Hello, World!</p></body>";

            // Act
            var result = HtmlExtractor.Extract(html);

            // Assert
            Assert.AreEqual(ElementType.body, result.Tag);
            Assert.AreEqual(1, result.Children.Count);
            Assert.AreEqual(ElementType.p, result.Children[0].Tag);
            Assert.AreEqual("Hello, World!", result.Children[0].Children[0].Text);
        }

        [TestMethod]
        public void Extract_NestedHtml_ReturnsCorrectStructure()
        {
            // Arrange
            var html = "<body><div><p>Hello, <span>World!</span></p></div></body>";

            // Act
            var result = HtmlExtractor.Extract(html);

            // Assert
            Assert.AreEqual(ElementType.body, result.Tag);
            Assert.AreEqual(1, result.Children.Count);
            Assert.AreEqual(ElementType.div, result.Children[0].Tag);
            Assert.AreEqual(1, result.Children[0].Children.Count);
            Assert.AreEqual(ElementType.p, result.Children[0].Children[0].Tag);
            Assert.AreEqual(2, result.Children[0].Children[0].Children.Count);
            Assert.AreEqual(ElementType.span, result.Children[0].Children[0].Children[0].Tag);
            Assert.AreEqual("Hello, ", result.Children[0].Children[0].Children[0].Text);
            Assert.AreEqual(ElementType.span, result.Children[0].Children[0].Children[1].Tag);
            Assert.AreEqual("World!", result.Children[0].Children[0].Children[1].Children[0].Text);
        }

        [TestMethod]
        public void Extract_HtmlWithStyles_ReturnsCorrectStyles()
        {
            // Arrange
            var html = "<body><p style=\"color:red;font-size:12px\">Hello, World!</p></body>";

            // Act
            var result = HtmlExtractor.Extract(html);

            // Assert
            Assert.AreEqual("red", result.Children[0].Style.Color);
            Assert.AreEqual("12px", result.Children[0].Style.FontSize);
        }
    }
}
