using HtmlToPdfConverter.Helpers;
using HtmlToPdfConverter.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace HtmlToPdfConverter.Converter
{
    public class PdfConverter
    {
        static PdfConverter()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] Convert(HtmlElement element)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Column(x =>
                        {
                            BuildQuestPdfTree(x, element);
                        });


                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf();
        }

        private static void BuildQuestPdfTree(ColumnDescriptor column, HtmlElement element)
        {
            switch (element.Tag)
            {
                case ElementType.h1:
                case ElementType.h2:
                case ElementType.h3:
                case ElementType.h4:
                case ElementType.h5:
                case ElementType.h6:
                    column.Item().ApplyContainerStyle(element.Style).Text(text =>
                    {
                        text.DefaultTextStyle(new TextStyle().ApplyTextStyle(element.Style, element.Tag));
                        BuildTextContent(text, element);
                    });
                    break;
                case ElementType.p:
                case ElementType.span:
                case ElementType.strong:
                case ElementType.b:
                case ElementType.i:
                    column.Item().ApplyContainerStyle(element.Style).Text(text =>
                    {
                        text.DefaultTextStyle(new TextStyle().ApplyTextStyle(element.Style, element.Tag));
                        BuildTextContent(text, element);
                    });
                    break;
                case ElementType.div:
                    HandleDiv(column, element);
                    break;
                case ElementType.img:
                    if (element.Attributes.TryGetValue("src", out var src))
                    {
                        var imageData = GetImageData(src);
                        if (imageData != null)
                        {
                            column.Item().ApplyContainerStyle(element.Style).Image(imageData);
                        }
                    }
                    break;
                default:
                    foreach (var child in element.Children)
                    {
                        BuildQuestPdfTree(column, child);
                    }
                    break;
            }
        }

        private static void HandleDiv(ColumnDescriptor column, HtmlElement element)
        {
            column.Item().ApplyContainerStyle(element.Style).Column(col =>
            {
                var inlineGroup = new List<HtmlElement>();

                foreach (var child in element.Children)
                {
                    if (IsInline(child.Tag))
                    {
                        inlineGroup.Add(child);
                    }
                    else
                    {
                        // Render the collected inline group as a paragraph
                        if (inlineGroup.Any())
                        {
                            RenderInlineGroup(col, inlineGroup, element.Style);
                            inlineGroup.Clear();
                        }

                        // Render the block-level element
                        BuildQuestPdfTree(col, child);
                    }
                }

                // Render any remaining inline elements at the end
                if (inlineGroup.Any())
                {
                    RenderInlineGroup(col, inlineGroup, element.Style);
                }
            });
        }

        private static void RenderInlineGroup(ColumnDescriptor col, List<HtmlElement> elements, ElementStyle parentStyle)
        {
            col.Item().Text(text =>
            {
                text.DefaultTextStyle(new TextStyle().ApplyTextStyle(parentStyle, ElementType.p)); // Treat as paragraph
                var tempParent = new HtmlElement(ElementType.p);
                tempParent.Children.AddRange(elements);
                BuildTextContent(text, tempParent);
            });
        }

        private static bool IsInline(ElementType tag)
        {
            return tag == ElementType.span || tag == ElementType.strong || tag == ElementType.b || tag == ElementType.i ||
                   tag == ElementType.em || tag == ElementType.u || tag == ElementType.a || tag == ElementType.del ||
                   tag == ElementType.code || tag == ElementType.sub || tag == ElementType.sup || tag == ElementType.small ||
                   tag == ElementType.mark || tag == ElementType.ins;
        }

        private static void BuildTextContent(TextDescriptor text, HtmlElement element)
        {
            if (!string.IsNullOrEmpty(element.Text))
            {
                text.Span(element.Text).Style(new TextStyle().ApplyTextStyle(element.Style, element.Tag));
            }

            foreach (var child in element.Children)
            {
                BuildTextContent(text, child);
            }
        }

        private static byte[]? GetImageData(string src)
        {
            try
            {
                if (src.StartsWith("http"))
                {
                    using var client = new System.Net.Http.HttpClient();
                    return client.GetByteArrayAsync(src).Result;
                }
                else
                {
                    return System.IO.File.ReadAllBytes(src);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
