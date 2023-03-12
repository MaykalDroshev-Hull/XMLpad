using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;

namespace XMLpad.Helper_Classes
{
    public static class TextEditorExtensions
    {
        public static void AppendTextWithColor(this TextEditor editor, string text, Brush brush)
        {
            // Create a new Run with the specified text and color
            var run = new Run(text);
            run.Foreground = brush;

            // Create a new Span with the Run
            var span = new Span(run);

            // Get the current TextPointer position
            var anchor = editor.CaretOffset > 0 ? editor.Document.GetPositionAtOffset(editor.CaretOffset - 1) : editor.Document.ContentStart;

            // Insert the Span at the current TextPointer position
            var paragraph = anchor.Paragraph;
            if (paragraph == null)
            {
                paragraph = new Paragraph();
                editor.Document.Add(paragraph);
            }

            paragraph.Inlines.Add(span);
        }

    }
}
