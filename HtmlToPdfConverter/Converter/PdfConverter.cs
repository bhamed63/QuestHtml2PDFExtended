using HtmlToPdfConverter.Helpers;
using HtmlToPdfConverter.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Buffers.Text;
using System.Data.Common;
using System.Xml.Linq;

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

                    page.Content()
                        .Column(x =>
                        {
                            BuildQuestPdfTree(x, element);
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
                case ElementType.span:
                    BuildQuestPdfTreeForChildren(column, element);
                    AppendText(column, element);
                    break;

                case ElementType.p:
                    BuildQuestPdfTreeForChildren(column, element);
                    AppendText(column, element, true);
                    break;

                case ElementType.div:
                    BuildQuestPdfTreeForChildren(column, element);
                    AppendText(column, element);
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
                    BuildQuestPdfTreeForChildren(column, element);
                    AppendText(column, element);
                    break;
            }
        }

        private static void AppendText(ColumnDescriptor column, HtmlElement element, bool addParagraphSpacing = false)
        {
            if (!string.IsNullOrEmpty(element.Text) &&
                !string.IsNullOrWhiteSpace(element.Text))
            {
                column.Item().ApplyContainerStyle(element.Style).Text(text =>
                {
                    text.DefaultTextStyle(new TextStyle().ApplyTextStyle(element.Style, element.Tag));
                    text.Span(element.Text);
                    if (addParagraphSpacing)
                        text.ParagraphSpacing(1);
                });
            }
        }

        private static void BuildQuestPdfTreeForChildren(ColumnDescriptor column, HtmlElement element)
        {
            if (element.Children.Count > 0)
            {
                column.Item().ApplyContainerStyle(element.Style).Column(x =>
                {
                    foreach (var child in element.Children)
                    {
                        BuildQuestPdfTree(x, child);
                    }
                });
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
                else if (src.Contains(";base64,"))
                {
                    var imageContent = src.Substring(src.IndexOf(";base64,") + 9);
                    return System.Convert.FromBase64String(imageContent);
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
