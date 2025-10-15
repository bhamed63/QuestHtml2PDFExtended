// See https://aka.ms/new-console-template for more information
using HtmlToPdfConverter.Converter;
using HtmlToPdfConverter.Extractor;
using HtmlToPdfConverter.Models;


var html = System.IO.File.ReadAllText("Files/index_file_for_quest_pdf_test.html");
var styles = HtmlExtractor.ExtractCssClassesAndStyles(html);
var stylesObj = CssParser.Parse(styles);
var element = HtmlExtractor.ExtractHtml(html, stylesObj);

// Act
var result = PdfConverter.Convert(element);


System.IO.File.WriteAllBytes("Files/Test.pdf", result);