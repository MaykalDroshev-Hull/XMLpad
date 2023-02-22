namespace XMLpad
{
    using System;
    using System.Windows;

    /// <summary>
    /// Editing text logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Handles the Undo event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Undo(object sender, RoutedEventArgs e) => textEditor.Undo();

        /// <summary>
        /// Handles the Redo event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Redo(object sender, RoutedEventArgs e) => textEditor.Redo();

        /// <summary>
        /// Handles the Cut event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Cut(object sender, RoutedEventArgs e) => textEditor.Cut();

        /// <summary>
        /// Handles the Copy event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Copy(object sender, RoutedEventArgs e) => textEditor.Copy();

        /// <summary>
        /// Handles the DuplicateLine event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_DuplicateLine(object sender, RoutedEventArgs e)
        {
            MainMenu_LineOperations_DuplicateLine(sender, e);
        }

        /// <summary>
        /// Handles the Paste event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Paste(object sender, RoutedEventArgs e) => textEditor.Paste();

        /// <summary>
        /// Handles the Delete event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Delete(object sender, RoutedEventArgs e) => textEditor.Delete();

        /// <summary>
        /// Handles the SelectAll event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_SelectAll(object sender, RoutedEventArgs e) => textEditor.SelectAll();

        /// <summary>
        /// Handles the DateTimeShort event of the MainMenu_Insert control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Insert_DateTimeShort(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText += DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// Handles the DateTimeLong event of the MainMenu_Insert control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Insert_DateTimeLong(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText += DateTime.Now.ToLongDateString();
        }

        /// <summary>
        /// Handles the IncreaseIndent event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void MainMenu_IncreaseIndent(object sender, RoutedEventArgs e)
        {

            // Here we mark the whole line and indent it
            int length = textEditor.SelectionLength;
            int start = textEditor.SelectionStart;

            if (length == 0)
            {
                start = textEditor.Document.GetLineByOffset(textEditor.CaretOffset).Offset;
                int end = textEditor.Document.GetLineByOffset(textEditor.CaretOffset).EndOffset;
                length = end - start;
                textEditor.Select(start, length);
            }
            string selectedText = textEditor.SelectedText;
            if (selectedText.Contains("\n"))
            {
                selectedText.Replace("\n", $"\n{tabCharacters}");
            }
            else
            {
                selectedText = tabCharacters + selectedText;
            }

            textEditor.Document.Replace(start, length, selectedText);
        }



        /// <summary>
        /// Handles the CurrentFilePath event of the MainMenu_Copy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Copy_CurrentFilePath(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(mCurrentFile.FilePath);
            UpdateStatusBar(true, "File path copied to clipboard");
            System.Media.SystemSounds.Asterisk.Play();
        }

        /// <summary>
        /// Handles the CurrentFileName event of the MainMenu_Copy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Copy_CurrentFileName(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(mCurrentFile.FilePath);
            UpdateStatusBar(true, "File name copied to clipboard");
            System.Media.SystemSounds.Asterisk.Play();
        }

        /// <summary>
        /// Handles the DecreaseIndent event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_DecreaseIndent(object sender, RoutedEventArgs e)
        {
            string selectedText = textEditor.SelectedText;
            string indentedText = selectedText.Replace("\n" + tabCharacters, "\n");
            textEditor.Text.Replace(textEditor.SelectedText, indentedText);
        }

        /// <summary>
        /// Handles the FindAndReplace event of the MainMenu_Edit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_FindAndReplace(object sender, RoutedEventArgs e)
        {
            FindAndReplaceWindow findAndReplaceWindow = new(textEditor, currentTheme)
            {
                Topmost = true
            };
            findAndReplaceWindow.Show();
        }
    }
}
