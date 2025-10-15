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
    }
}
