using HtmlToPdfConverter.Converter;
using HtmlToPdfConverter.Extractor;

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
            var element = HtmlExtractor.Extract(html);

            // Act
            var result = PdfConverter.Convert(element);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public void Convert_Image_ReturnsPdf()
        {
            // Arrange
            var html = "<body><img src=\"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png\" /></body>";
            var element = HtmlExtractor.Extract(html);

            // Act
            var result = PdfConverter.Convert(element);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}
