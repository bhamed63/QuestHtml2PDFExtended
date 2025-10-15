using HtmlToPdfConverter.Converter;
using HtmlToPdfConverter.Extractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlToPdfConverter.Tests
{
    [TestClass]
    public class PdfConverterTests
    {
        [TestMethod]
        public void Convert_SimpleHtml_ReturnsPdf()
        {
            // Arrange
            var html = "<body><p>Hello, World!</p></body>";

            // Act
            var result = HtmlToPdfConverter.Convert(html);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void Convert_Image_ReturnsPdf()
        {
            // Arrange
            var html = "<body><img src=\"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png\" /></body>";

            // Act
            var result = HtmlToPdfConverter.Convert(html);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}
