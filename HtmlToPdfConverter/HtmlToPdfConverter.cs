using HtmlToPdfConverter.Extractor;
using HtmlToPdfConverter.Converter;

namespace HtmlToPdfConverter
{
    public class HtmlToPdfConverter
    {
        public static byte[] Convert(string html)
        {
            var element = HtmlExtractor.Extract(html);
            return PdfConverter.Convert(element);
        }
    }
}
