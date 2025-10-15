// See https://aka.ms/new-console-template for more information
using HtmlToPdfConverter.Converter;
using HtmlToPdfConverter.Extractor;


var html = System.IO.File.ReadAllText("Files/index_file_for_quest_pdf_test.html");
var element = HtmlExtractor.Extract(html);

// Act
var result = PdfConverter.Convert(element);


System.IO.File.WriteAllBytes("Files/Test.pdf", result);