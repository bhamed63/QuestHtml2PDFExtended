using HtmlAgilityPack;
using HtmlToPdfConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HtmlToPdfConverter.Extractor
{
    public class HtmlExtractor
    {
        public static HtmlElement Extract(string html, Dictionary<string, ElementStyle> cssStyles)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var body = doc.DocumentNode.SelectSingleNode("//body");
            return MapToHtmlElement(body, cssStyles);
        }

        private static HtmlElement MapToHtmlElement(HtmlNode node, Dictionary<string, ElementStyle> cssStyles)
        {
            var tag = Enum.TryParse<ElementType>(node.Name, out var elementType) ? elementType : ElementType.unknown;

            var htmlElement = new HtmlElement(tag);

            // Apply styles from CSS classes first
            var classAttribute = node.GetAttributeValue("class", "");
            if (!string.IsNullOrEmpty(classAttribute))
            {
                var classes = classAttribute.Split(' ');
                foreach (var className in classes)
                {
                    if (cssStyles.TryGetValue(className, out var classStyle))
                    {
                        htmlElement.Style.Merge(classStyle);
                    }
                }
            }

            // Apply inline styles, which will override class styles
            var inlineStyle = StyleExtractor.ExtractStyles(node.GetAttributeValue("style", ""));
            htmlElement.Style.Merge(inlineStyle, true);

            foreach (var attribute in node.Attributes)
            {
                if (!htmlElement.Attributes.ContainsKey(attribute.Name))
                {
                    htmlElement.Attributes.Add(attribute.Name, attribute.Value);
                }
            }

            foreach (var child in node.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Element)
                {
                    htmlElement.Children.Add(MapToHtmlElement(child, cssStyles));
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
