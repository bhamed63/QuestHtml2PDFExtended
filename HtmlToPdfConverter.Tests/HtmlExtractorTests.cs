using HtmlToPdfConverter.Extractor;
using HtmlToPdfConverter.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace HtmlToPdfConverter.Tests
{
    [TestClass]
    public class HtmlExtractorTests
    {
        [TestMethod]
        public void ExtractCssClassesAndStyles_FindsSingleStyleTag()
        {
            var html = "<html><head><style>.my-class { color: red; }</style></head><body></body></html>";
            var result = HtmlExtractor.ExtractCssClassesAndStyles(html);
            Assert.IsTrue(result.Contains(".my-class { color: red; }"));
        }

        [TestMethod]
        public void ExtractCssClassesAndStyles_FindsMultipleStyleTags_ConcatenatesContent()
        {
            var html = "<html><head><style>.a { color: blue; }</style><style>.b { font-weight: bold; }</style></head><body></body></html>";
            var result = HtmlExtractor.ExtractCssClassesAndStyles(html);
            Assert.IsTrue(result.Contains(".a { color: blue; }"));
            Assert.IsTrue(result.Contains(".b { font-weight: bold; }"));
        }

        [TestMethod]
        public void ExtractCssClassesAndStyles_NoStyleTags_ReturnsEmptyString()
        {
            var html = "<html><body><p>Hello</p></body></html>";
            var result = HtmlExtractor.ExtractCssClassesAndStyles(html);
            Assert.AreEqual(string.Empty.Trim(), result.Trim());
        }

        [TestMethod]
        public void ExtractHtml_AppliesStylesFromCss()
        {
            // Arrange
            var html = "<body><p class=\"my-style\">Hello</p></body>";
            var cssStyles = new Dictionary<string, ElementStyle>
            {
                { "my-style", new ElementStyle { Color = "green" } }
            };

            // Act
            var result = HtmlExtractor.ExtractHtml(html, cssStyles);

            // Assert
            Assert.AreEqual("green", result.Children[0].Style.Color);
        }

        [TestMethod]
        public void ExtractHtml_InlineStyles_OverrideClassStyles()
        {
            // Arrange
            var html = "<body><p class=\"my-style\" style=\"color: red;\">Hello</p></body>";
            var cssStyles = new Dictionary<string, ElementStyle>
            {
                { "my-style", new ElementStyle { Color = "blue" } }
            };

            // Act
            var result = HtmlExtractor.ExtractHtml(html, cssStyles);

            // Assert
            Assert.AreEqual("red", result.Children[0].Style.Color);
        }

        [TestMethod]
        public void Extract_SimpleHtml_ReturnsCorrectStructure()
        {
            // Arrange
            var html = "<body><p>Hello, World!</p></body>";

            // Act
            var result = HtmlExtractor.ExtractHtml(html, new Dictionary<string, ElementStyle>());

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
            var result = HtmlExtractor.ExtractHtml(html, new Dictionary<string, ElementStyle>());

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
        public void Extract_Image_ReturnsCorrectStructure()
        {
            // Arrange
            var html = "<body><img src=\"https://example.com/logo.png\" /></body>";

            // Act
            var result = HtmlExtractor.ExtractHtml(html, new Dictionary<string, ElementStyle>());

            // Assert
            Assert.AreEqual(ElementType.body, result.Tag);
            Assert.AreEqual(1, result.Children.Count);
            Assert.AreEqual(ElementType.img, result.Children[0].Tag);
            Assert.AreEqual("https://example.com/logo.png", result.Children[0].Attributes["src"]);
        }

        [TestMethod]
        public void ExtractHtml_AppliesMultipleStylesFromCss()
        {
            // Arrange
            var html = "<body><p class=\"my-style\">Hello</p></body>";
            var cssStyles = new Dictionary<string, ElementStyle>
            {
                { "my-style", new ElementStyle { Color = "green", FontSize = "18px" } }
            };

            // Act
            var result = HtmlExtractor.ExtractHtml(html, cssStyles);

            // Assert
            Assert.AreEqual("green", result.Children[0].Style.Color);
            Assert.AreEqual("18px", result.Children[0].Style.FontSize);
        }

        [TestMethod]
        public void ExtractHtml_AppliesStylesFromMultipleClasses()
        {
            // Arrange
            var html = "<body><p class=\"style1 style2\">Hello</p></body>";
            var cssStyles = new Dictionary<string, ElementStyle>
            {
                { "style1", new ElementStyle { Color = "red" } },
                { "style2", new ElementStyle { FontWeight = "bold" } }
            };

            // Act
            var result = HtmlExtractor.ExtractHtml(html, cssStyles);

            // Assert
            Assert.AreEqual("red", result.Children[0].Style.Color);
            Assert.AreEqual("bold", result.Children[0].Style.FontWeight);
        }
    }
}
