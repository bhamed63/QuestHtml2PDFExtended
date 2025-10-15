using HtmlToPdfConverter.Models;
using System.Collections.Generic;

namespace HtmlToPdfConverter.Extractor
{
    internal class StyleExtractor
    {
        public static ElementStyle ExtractStyles(string style)
        {
            var elementStyle = new ElementStyle();
            if (string.IsNullOrEmpty(style))
            {
                return elementStyle;
            }

            var stylePairs = style.Split(';');
            foreach (var pair in stylePairs)
            {
                var styleParts = pair.Split(':');
                if (styleParts.Length == 2)
                {
                    var key = styleParts[0].Trim();
                    var value = styleParts[1].Trim();
                    SetStyle(elementStyle, key, value);
                }
            }

            return elementStyle;
        }

        private static void SetStyle(ElementStyle elementStyle, string key, string value)
        {
            switch (key)
            {
                case "color":
                    elementStyle.Color = value;
                    break;
                case "background-color":
                    elementStyle.BackgroundColor = value;
                    break;
                case "font-size":
                    elementStyle.FontSize = value;
                    break;
                case "font-weight":
                    elementStyle.FontWeight = value;
                    break;
                case "font-family":
                    elementStyle.FontFamily = value;
                    break;
                case "font-style":
                    if (value == "italic")
                    {
                        elementStyle.IsItalic = true;
                    }
                    break;
                case "text-decoration":
                    if (value == "underline")
                    {
                        elementStyle.IsUnderline = true;
                    }
                    else if (value == "line-through")
                    {
                        elementStyle.IsLineThrough = true;
                    }
                    break;
                case "width":
                    elementStyle.Width = value;
                    break;
                case "height":
                    elementStyle.Height = value;
                    break;
                case "padding":
                    elementStyle.Padding = value;
                    break;
                case "margin":
                    elementStyle.Margin = value;
                    break;
                case "border-radius":
                    elementStyle.BorderRadius = value;
                    break;
                case "border":
                    elementStyle.Border = value;
                    break;
                case "display":
                    elementStyle.Display = value;
                    break;
                case "text-align":
                    elementStyle.TextAlign = value;
                    break;
                case "vertical-align":
                    elementStyle.VerticalAlign = value;
                    break;
            }
        }
    }
}
