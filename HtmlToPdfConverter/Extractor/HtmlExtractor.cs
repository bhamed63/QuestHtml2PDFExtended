using HtmlAgilityPack;
using HtmlToPdfConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HtmlToPdfConverter.Extractor
{
    public class HtmlExtractor
    {
        public static string ExtractCssClassesAndStyles(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var styleNodes = doc.DocumentNode.SelectNodes("//style");
            if (styleNodes == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var styleNode in styleNodes)
            {
                sb.AppendLine(styleNode.InnerText);
            }

            return sb.ToString();
        }

        public static HtmlElement ExtractHtml(string html, Dictionary<string, ElementStyle> cssStyles)
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

            var finalStyle = new ElementStyle();

            // Apply styles from CSS classes first
            var classAttribute = node.GetAttributeValue("class", "");
            if (!string.IsNullOrEmpty(classAttribute))
            {
                var classes = classAttribute.Split(' ');
                foreach (var className in classes)
                {
                    if (cssStyles.TryGetValue(className, out var classStyle))
                    {
                        finalStyle.Merge(classStyle.Clone());
                    }
                }
            }

            // Apply inline styles, which will override class styles
            var inlineStyle = StyleExtractor.ExtractStyles(node.GetAttributeValue("style", ""));
            finalStyle.Merge(inlineStyle, true);

            htmlElement.Style = finalStyle;

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
