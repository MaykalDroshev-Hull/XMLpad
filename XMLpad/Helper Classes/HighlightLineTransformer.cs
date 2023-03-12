using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace XMLpad
{
    public class HighlightLineTransformer : DocumentColorizingTransformer
    {
        private string[] wordsToHighlight;

        public HighlightLineTransformer(string[] wordsToHighlight)
        {
            this.wordsToHighlight = wordsToHighlight;
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            string lineText = CurrentContext.Document.GetText(line.Offset, line.Length);
            foreach (string word in wordsToHighlight)
            {
                int index = lineText.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                while (index >= 0)
                {
                    base.ChangeLinePart(
                        line.Offset + index,
                        line.Offset + index + word.Length,
                        ApplyChanges);
                    index = lineText.IndexOf(word, index + word.Length, StringComparison.OrdinalIgnoreCase);
                }
            }

            ColorizeDocumentLine(line); // Call base implementation to perform actual colorization
        }

        private void ApplyChanges(VisualLineElement element)
        {
            element.TextRunProperties.SetBackgroundBrush(new SolidColorBrush(Colors.LightYellow));
        }
        protected void ColorizeDocumentLine(DocumentLine line)
        {
            string lineText = CurrentContext.Document.GetText(line);
            foreach (string word in wordsToHighlight)
            {
                int index = lineText.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                while (index >= 0)
                {
                    base.ChangeLinePart(
                        line.Offset + index,
                        line.Offset + index + word.Length,
                        ApplyChanges);
                    index = lineText.IndexOf(word, index + word.Length, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

    }
}
