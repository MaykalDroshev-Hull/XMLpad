namespace XMLpad
{
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Rendering;

    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private readonly TextEditor editor;

        public HighlightCurrentLineBackgroundRenderer(TextEditor editor)
        {
            this.editor = editor;
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
                    new SolidColorBrush(Color.FromRgb(63, 63, 63)),
                    null,
                    new System.Windows.Rect(rect.Location, new System.Windows.Size(textView.ActualWidth, rect.Height)));
            }
        }
    }
}
