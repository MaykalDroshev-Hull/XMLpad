using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XMLpad
{
    class HighlightingColorizer : DocumentColorizingTransformer
    {
        private int lineNumber;
        private Color color;

        public HighlightingColorizer(int lineNumber, Color color)
        {
            this.lineNumber = lineNumber;
            this.color = color;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.LineNumber == lineNumber)
            {
                ChangeLinePart(line.Offset, line.EndOffset, ApplyChanges);
            }
        }

        private void ApplyChanges(VisualLineElement element)
        {
            var pen = new Pen(new SolidColorBrush(color), 1);
            element.TextRunProperties.SetBackgroundBrush(new SolidColorBrush(color));
        }
    }
}
