namespace HtmlToPdfConverter.Models
{
    public class ElementStyle
    {
        public string? Color { get; set; }
        public string? BackgroundColor { get; set; }
        public string? FontSize { get; set; }
        public string? FontWeight { get; set; }
        public string? FontFamily { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsLineThrough { get; set; }
        public string? Padding { get; set; }
        public string? Border { get; set; }
        public string? BorderRadius { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? Margin { get; set; }
        public string? Display { get; set; }
        public string? TextAlign { get; set; }
        public string? VerticalAlign { get; set; }

        public void Merge(ElementStyle other, bool overwrite = false)
        {
            if (overwrite || string.IsNullOrEmpty(Color)) Color = other.Color;
            if (overwrite || string.IsNullOrEmpty(BackgroundColor)) BackgroundColor = other.BackgroundColor;
            if (overwrite || string.IsNullOrEmpty(FontSize)) FontSize = other.FontSize;
            if (overwrite || string.IsNullOrEmpty(FontWeight)) FontWeight = other.FontWeight;
            if (overwrite || string.IsNullOrEmpty(FontFamily)) FontFamily = other.FontFamily;
            if (overwrite || !IsItalic) IsItalic = other.IsItalic;
            if (overwrite || !IsUnderline) IsUnderline = other.IsUnderline;
            if (overwrite || !IsLineThrough) IsLineThrough = other.IsLineThrough;
            if (overwrite || string.IsNullOrEmpty(Padding)) Padding = other.Padding;
            if (overwrite || string.IsNullOrEmpty(Border)) Border = other.Border;
            if (overwrite || string.IsNullOrEmpty(BorderRadius)) BorderRadius = other.BorderRadius;
            if (overwrite || string.IsNullOrEmpty(Width)) Width = other.Width;
            if (overwrite || string.IsNullOrEmpty(Height)) Height = other.Height;
            if (overwrite || string.IsNullOrEmpty(Margin)) Margin = other.Margin;
            if (overwrite || string.IsNullOrEmpty(Display)) Display = other.Display;
            if (overwrite || string.IsNullOrEmpty(TextAlign)) TextAlign = other.TextAlign;
            if (overwrite || string.IsNullOrEmpty(VerticalAlign)) VerticalAlign = other.VerticalAlign;
        }
    }
}
