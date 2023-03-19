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
    using System;
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Rendering;

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
