using HtmlToPdfConverter.Converter;
using HtmlToPdfConverter.Extractor;

namespace HtmlToPdfConverter
{
    public class HtmlToPdfConverter
    {
        public static byte[] Convert(string html)
        {
            var cssContent = HtmlExtractor.ExtractCssClassesAndStyles(html);
            var cssStyles = CssParser.Parse(cssContent);
            var element = HtmlExtractor.ExtractHtml(html, cssStyles);
            return PdfConverter.Convert(element);
        }
    }
}
