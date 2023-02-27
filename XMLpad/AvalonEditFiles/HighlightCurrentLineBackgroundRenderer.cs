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
