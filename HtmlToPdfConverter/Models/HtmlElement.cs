using System.Collections.Generic;

namespace HtmlToPdfConverter.Models
{
    public class HtmlElement
    {
        public ElementType Tag { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public ElementStyle Style { get; set; }
        public List<HtmlElement> Children { get; set; }
        public string Text { get; set; }

        public HtmlElement(ElementType tag, string text = "")
        {
            Tag = tag;
            Attributes = new Dictionary<string, string>();
            Style = new ElementStyle();
            Children = new List<HtmlElement>();
            Text = text;
        }
    }
}
