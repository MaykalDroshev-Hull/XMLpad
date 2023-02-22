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
