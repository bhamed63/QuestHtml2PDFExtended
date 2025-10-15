namespace HtmlToPdfConverter.Models
{
    public class ElementStyle
    {
        // font
        public string? Color { get; set; }
        public string? BackgroundColor { get; set; }
        public string? FontSize { get; set; }
        public string? FontWeight { get; set; }
        public string? FontFamily { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsLineThrough { get; set; }

        // layout
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? Padding { get; set; }
        public string? Margin { get; set; }
        public string? BorderRadius { get; set; }
        public string? Border { get; set; }
        public string? Display { get; set; }
        public string? TextAlign { get; set; }
        public string? VerticalAlign { get; set; }

        public ElementStyle()
        {
        }
    }
}
