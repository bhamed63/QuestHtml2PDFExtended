using HtmlAgilityPack;
using HtmlToPdfConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlToPdfConverter.Extractor
{
    public class HtmlExtractor
    {
        public static HtmlElement Extract(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var body = doc.DocumentNode.SelectSingleNode("//body");
            return MapToHtmlElement(body);
        }

        private static HtmlElement MapToHtmlElement(HtmlNode node)
        {
            var tag = Enum.TryParse<ElementType>(node.Name, out var elementType) ? elementType : ElementType.unknown;

            var htmlElement = new HtmlElement(tag);

            htmlElement.Style = StyleExtractor.ExtractStyles(node.GetAttributeValue("style", ""));

            foreach (var attribute in node.Attributes)
            {
                htmlElement.Attributes.Add(attribute.Name, attribute.Value);
            }

            foreach (var child in node.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Element)
                {
                    htmlElement.Children.Add(MapToHtmlElement(child));
                }
                else if (child.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(child.InnerText))
                {
                    htmlElement.Children.Add(new HtmlElement(ElementType.span, child.InnerText));
                }
            }

            return htmlElement;
        }
    }
}
