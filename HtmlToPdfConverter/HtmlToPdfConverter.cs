using HtmlToPdfConverter.Extractor;
using HtmlToPdfConverter.Converter;
using HtmlToPdfConverter.Models;
using System.Collections.Generic;

namespace HtmlToPdfConverter
{
    public class HtmlToPdfConverter
    {
        public static byte[] Convert(string html, string cssContent = "")
        {
            var cssStyles = CssParser.Parse(cssContent);
            var element = HtmlExtractor.Extract(html, cssStyles);
            return PdfConverter.Convert(element);
        }
    }
}
