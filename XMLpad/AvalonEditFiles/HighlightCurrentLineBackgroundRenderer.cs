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
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Rendering;

    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private readonly TextEditor editor;
        private readonly MainWindow.theme theme;
        public HighlightCurrentLineBackgroundRenderer(TextEditor editor, MainWindow.theme currentTheme)
        {
            this.editor = editor;
            this.theme = currentTheme;
        }

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (editor.Document == null)
                return;

            textView.EnsureVisualLines();
            var currentLine = editor.Document.GetLineByOffset(editor.CaretOffset);

            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
            {
                drawingContext.DrawRectangle(
                    theme == MainWindow.theme.Dark?
                    new SolidColorBrush(Color.FromRgb(63, 63, 63)):
                    new SolidColorBrush(Color.FromRgb(232,232,255)),
                    null,
                    new System.Windows.Rect(rect.Location, new System.Windows.Size(textView.ActualWidth, rect.Height)));
            }
        }
    }
}
