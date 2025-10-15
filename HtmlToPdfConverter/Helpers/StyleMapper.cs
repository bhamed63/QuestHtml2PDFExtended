using HtmlToPdfConverter.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace HtmlToPdfConverter.Helpers
{
    internal static class StyleMapper
    {
        public static TextStyle ApplyTextStyle(this TextStyle style, ElementStyle elementStyle, ElementType tag)
        {
            if (!string.IsNullOrEmpty(elementStyle.Color))
            {
                style.FontColor(elementStyle.Color);
            }
            if (!string.IsNullOrEmpty(elementStyle.FontSize))
            {
                if (float.TryParse(elementStyle.FontSize.Replace("px", ""), out var fontSize))
                {
                    style.FontSize(fontSize);
                }
            }
            if (!string.IsNullOrEmpty(elementStyle.FontWeight))
            {
                switch (elementStyle.FontWeight)
                {
                    case "bold":
                        style.Bold();
                        break;
                    case "600":
                    case "700":
                    case "800":
                    case "900":
                        style.SemiBold();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(elementStyle.FontFamily))
            {
                style.FontFamily(elementStyle.FontFamily);
            }
            if (elementStyle.IsItalic)
            {
                style.Italic();
            }
            if (elementStyle.IsUnderline)
            {
                style.Underline();
            }
            if (elementStyle.IsLineThrough)
            {
                style.Strikethrough();
            }

            switch (tag)
            {
                case ElementType.h1:
                    style.FontSize(32).Bold();
                    break;
                case ElementType.h2:
                    style.FontSize(24).Bold();
                    break;
                case ElementType.h3:
                    style.FontSize(18).Bold();
                    break;
                case ElementType.h4:
                    style.FontSize(16).Bold();
                    break;
                case ElementType.h5:
                    style.FontSize(14).Bold();
                    break;
                case ElementType.h6:
                    style.FontSize(12).Bold();
                    break;
            }

            return style;
        }

        public static IContainer ApplyContainerStyle(this IContainer container, ElementStyle style)
        {
            if (!string.IsNullOrEmpty(style.BackgroundColor))
            {
                container = container.Background(style.BackgroundColor);
            }

            if (!string.IsNullOrEmpty(style.Padding))
            {
                if (float.TryParse(style.Padding.Replace("px", ""), out var padding))
                {
                    container = container.Padding(padding);
                }
            }

            if (!string.IsNullOrEmpty(style.Border))
            {
                // This is a simplified version. A real implementation would parse the border string.
                container = container.Border(1).BorderColor("#000000");
            }

            if (!string.IsNullOrEmpty(style.Width))
            {
                if (float.TryParse(style.Width.Replace("px", ""), out var width))
                {
                    container = container.Width(width);
                }
            }

            if (!string.IsNullOrEmpty(style.Height))
            {
                if (float.TryParse(style.Height.Replace("px", ""), out var height))
                {
                    container = container.Height(height);
                }
            }

            return container;
        }
    }
}
