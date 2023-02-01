namespace XMLpad
{
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Rendering;
    using System.Windows.Media;

    /// <summary>
    /// This class is used to colorize lines in the document.
    /// </summary>
    class HighlightingColorizer : DocumentColorizingTransformer
    {
        // Represents the line number to be colorized.
        private readonly int lineNumber;
        // Represents the starting position of the line part to be colorized.
        private readonly int start;
        // Represents the ending position of the line part to be colorized.
        private readonly int end;
        // Represents the color to be applied.
        private Color color;

        /// <summary>
        /// Constructor to create an instance of the class with the line number,
        /// start and end positions, and color to be used for colorizing.
        /// </summary>
        /// <param name="lineNumber">The line number to be colorized.</param>
        /// <param name="start">The starting position of the line part to be colorized.</param>
        /// <param name="end">The ending position of the line part to be colorized.</param>
        /// <param name="color">The color to be applied.</param>
        public HighlightingColorizer(int lineNumber, int start, int end, Color color)
        {
            this.lineNumber = lineNumber;
            this.start = start;
            this.end = end;
            this.color = color;
        }

        /// <summary>
        /// Constructor to create an instance of the class with the line number and
        /// color to be used for colorizing.
        /// </summary>
        /// <param name="lineNumber">The line number to be colorized.</param>
        /// <param name="color">The color to be applied.</param>
        public HighlightingColorizer(int lineNumber, Color color)
        {
            this.lineNumber = lineNumber;
            this.color = color;
        }

        /// <summary>
        /// Overrides the base class method to colorize the line with the specified color.
        /// </summary>
        /// <param name="line">The document line to be colorized.</param>
        protected override void ColorizeLine(DocumentLine line)
        {
            // If the line number is the specified line number, colorize the line part
            // between the start and end positions with the specified color.
            if (line.LineNumber == lineNumber)
            {
                ChangeLinePart(line.Offset + start, line.Offset + end, ApplyChanges);
            }
        }

        /// <summary>
        /// Method to apply the color to the line part.
        /// </summary>
        /// <param name="element">The visual line element to which the color is to be applied.</param>
        private void ApplyChanges(VisualLineElement element)
        {
            // Set the background brush of the text run properties to the specified color.
            element.TextRunProperties.SetBackgroundBrush(new SolidColorBrush(color));
        }
    }
}
