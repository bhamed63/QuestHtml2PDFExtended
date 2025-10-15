using HtmlToPdfConverter.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HtmlToPdfConverter.Extractor
{
    public class CssParser
    {
        public static Dictionary<string, ElementStyle> Parse(string cssContent)
        {
            var styles = new Dictionary<string, ElementStyle>();
            if (string.IsNullOrWhiteSpace(cssContent))
            {
                return styles;
            }

            // This is a simplified regex for parsing CSS. It may not handle all edge cases.
            var regex = new Regex(@"\.([a-zA-Z0-9_-]+)\s*\{([^}]+)\}");
            var matches = regex.Matches(cssContent);

            foreach (Match match in matches)
            {
                var className = match.Groups[1].Value;
                var styleProperties = match.Groups[2].Value;

                var elementStyle = StyleExtractor.ExtractStyles(styleProperties);

                if (styles.ContainsKey(className))
                {
                    // In a real scenario, you might want to merge styles.
                    // For simplicity, we'll just overwrite.
                    styles[className] = elementStyle;
                }
                else
                {
                    styles.Add(className, elementStyle);
                }
            }

            return styles;
        }
    }
}
