// Copyright (c) 2023 Maykal Droshev
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sub license, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

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
