namespace Tablazor.Icons
{
    internal static class TabIconsRenderer
    {
        /// <summary>
        /// Renders an icon path as a complete SVG element
        /// Automatically detects if the icon is filled or outline based on the path data
        /// </summary>
        /// <param name="pathData">The SVG path data</param>
        /// <param name="size">Icon size (default: 24)</param>
        /// <param name="strokeWidth">Stroke width for outline icons (default: 2)</param>
        /// <param name="color">Icon color (default: currentColor)</param>
        /// <param name="cssClass">Optional CSS class</param>
        /// <returns>Complete SVG markup</returns>
        public static string Render(string pathData, int size = 24, int strokeWidth = 2, string color = "currentColor", string cssClass = "")
        {
            var classAttr = string.IsNullOrEmpty(cssClass) ? "" : $" class=\"{cssClass}\"";

            // Detect if this is a filled icon by checking if it exists in the filled icons
            var isFilled = IsFilledIcon(pathData);

            if (isFilled)
            {
                // Filled icon: use fill, no stroke
                return $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 24 24\" fill=\"{color}\" stroke=\"none\"{classAttr}><path d=\"{pathData}\"/></svg>";
            }

            // Outline icon: use stroke, no fill
            return $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"{color}\" stroke-width=\"{strokeWidth}\" stroke-linecap=\"round\" stroke-linejoin=\"round\"{classAttr}><path d=\"{pathData}\"/></svg>";
        }

        private static bool IsFilledIcon(string pathData)
        {
            // Check if the path data matches any filled icon

            var filledIconsType = typeof(TabIcons.Filled);
            var properties = filledIconsType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            return properties.Any(prop => prop.GetValue(null) as string == pathData);
        }
    }
}
