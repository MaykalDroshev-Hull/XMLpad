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
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Commenting text logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Handles the Comment event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Comment(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            string selectedText = textArea.Selection.GetText();
            string commentedText = "<!--" + selectedText + "-->";
            foreach (var segment in textArea.Selection.Segments)
            {
                textArea.Document.Replace(segment, commentedText);
            }
        }

        /// <summary>
        /// Handles the Uncomment event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Uncomment(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            string selectedText = textArea.Selection.GetText();
            string uncommentedText = selectedText.Replace("<!--", "").Replace("-->", "");
            textEditor.Document.Replace(textArea.Selection.Segments.First(), uncommentedText);
        }
    }
}
