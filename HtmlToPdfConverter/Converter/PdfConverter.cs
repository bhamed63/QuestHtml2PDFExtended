using HtmlToPdfConverter.Helpers;
using HtmlToPdfConverter.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
                case ElementType.span:
                    column.Item().ApplyContainerStyle(element.Style).Text(text =>
                    {
                        text.DefaultTextStyle(new TextStyle().ApplyTextStyle(element.Style, element.Tag));
                        text.Span(element.Text);
                    });
                    break;
                case ElementType.p:
                    column.Item().ApplyContainerStyle(element.Style).Text(text =>
                    {
                        text.DefaultTextStyle(new TextStyle().ApplyTextStyle(element.Style, element.Tag));
                        text.Span(element.Text);
                        text.ParagraphSpacing(1);
                    });
                    break;
                case ElementType.div:
                    column.Item().ApplyContainerStyle(element.Style).Column(x =>
                    {
                        foreach (var child in element.Children)
                        {
                            BuildQuestPdfTree(x, child);
                        }
                    });
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
