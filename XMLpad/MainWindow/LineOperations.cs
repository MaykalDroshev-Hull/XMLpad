namespace XMLpad
{
    using ICSharpCode.AvalonEdit.Document;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;


    /// <summary>
    /// Line operations logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Handles the DuplicateLine event of the MainMenu_LineOperations control.
        /// This method gets the current line by using the GetLineByOffset method of the Document class and passing the CaretOffset
        /// property of the TextEditor.
        /// Then it gets the text of the line using the GetText method of the Document class and passing the currentLine object.
        /// It also get the line number of the current line.
        /// Then it insert the text of the line at the end of the current line using the Insert method of the Document class,
        /// passing the insertionOffset which is the end offset of the current line and the lineText + newline.
        /// Finally, it sets the Caret offset to the insertion offset
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_DuplicateLine(object sender, RoutedEventArgs e)
        {
            DocumentLine currentLine = textEditor.Document.GetLineByOffset(textEditor.CaretOffset);
            string lineText = textEditor.Document.GetText(currentLine);
            int insertionOffset = currentLine.EndOffset;
            textEditor.Document.Insert(insertionOffset, lineText + Environment.NewLine);
            textEditor.CaretOffset = insertionOffset;
        }

        /// <summary>
        /// Handles the RemoveDuplicateLines event of the MainMenu_LineOperations control.
        /// This method first splits the input string into an array of lines using the
        /// Environment.NewLine separator. It then uses a HashSet to keep track of unique
        /// lines, and iterates through the array of lines. If a line is not already in the
        /// HashSet, it is added, and appended to the result StringBuilder. The final result
        /// is returned as a string.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_RemoveDuplicateLines(object sender, RoutedEventArgs e)
        {
            string[] lines = textEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            HashSet<string> uniqueLines = new();
            StringBuilder result = new();

            foreach (string line in lines)
            {
                if (uniqueLines.Add(line))
                {
                    result.AppendLine(line);
                }
            }

            textEditor.Text = result.ToString();
        }

        /// <summary>
        /// Handles the RemoveConsecutiveDuplicateLines event of the MainMenu_LineOperations control.
        /// This method first splits the input string into an array of strings, where each element in
        /// the array represents a line of the input. Then, it iterates through the array of lines,
        /// comparing each line with the previous line. If a line is different from the previous line,
        /// it is added to the output string, otherwise it is skipped. Finally, it returns the output
        /// string containing only the non-consecutive duplicate lines.
        ///
        /// It's important to note that this method does not compare lines case-sensitive.
        /// If you want case-sensitive comparison, you can replace the equality operator
        /// with string.Compare(line, previousLine) != 0
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_RemoveConsecutiveDuplicateLines(object sender, RoutedEventArgs e)
        {
            string[] lines = textEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            StringBuilder output = new();
            string previousLine = "";
            foreach (string line in lines)
            {
                if (line != previousLine)
                {
                    output.AppendLine(line);
                    previousLine = line;
                }
            }
            textEditor.Text = output.ToString();
        }

        /// <summary>
        /// Handles the MoveUpCurrentLine event of the MainMenu_LineOperations control.
        /// Please note that this is a basic implementation that moves the current line up one line only
        /// , if you want to move the current line up multiple lines you need to adjust the logic accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_MoveUpCurrentLine(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            int currentLine = textArea.Caret.Line;

            if (currentLine == 1)
                return; // Already at the top, can't move up

            if (currentLine == textArea.Document.LineCount)
            {
                textEditor.Text += "\r\n";
            }
            int currentLineStart = textArea.Document.GetOffset(currentLine, 0);
            int currentLineEnd = textArea.Document.GetOffset(currentLine + 1, 0);
            string currentLineText = textArea.Document.GetText(currentLineStart, currentLineEnd - currentLineStart);

            int previousLineStart = textArea.Document.GetOffset(currentLine - 1, 0);
            int previousLineEnd = textArea.Document.GetOffset(currentLine, 0);
            string previousLineText = textArea.Document.GetText(previousLineStart, previousLineEnd - previousLineStart);

            // Replace the current line with the previous line
            textArea.Document.Replace(currentLineStart, currentLineEnd - currentLineStart, previousLineText);

            // Replace the previous line with the current line
            textArea.Document.Replace(previousLineStart, previousLineEnd - previousLineStart, currentLineText);

            // Move the caret to the new current line
            textArea.Caret.Line--;
        }

        /// <summary>
        /// Handles the MoveDownCurrentLine event of the MainMenu_LineOperations control.
        /// This method uses the TextEditor's Document property to access the current
        /// document and manipulate its text. First, it gets the current line by using
        /// the CaretOffset property to find the offset of the caret, and then finds
        /// the line number of the line by using the GetLineByOffset method.
        /// Then it gets the text of the current line using the GetText method.
        /// Then it checks whether the current line is the last line of the document, if so then it return nothing.
        /// Then it gets the line below the current line by using the GetLineByNumber
        /// method and the line number + 1. Then it gets the text of the line below using the GetText method.
        /// Then it replaces the current line with the text of the current line and the text
        /// of the line below, by using the Replace method.Then it removes the current line
        /// below from the document using the Remove method.Finally, it sets the caret offset
        /// to the end of the current line using the CaretOffset property.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_MoveDownCurrentLine(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            int currentLine = textArea.Caret.Line;

            if (currentLine == textArea.Document.LineCount)
                return; // Already at the bottom, can't move down

            int currentLineStart = textArea.Document.GetOffset(currentLine, 0);
            int currentLineEnd = textArea.Document.GetOffset(currentLine + 1, 0);
            string currentLineText = textArea.Document.GetText(currentLineStart, currentLineEnd - currentLineStart);

            int nextLineStart = textArea.Document.GetOffset(currentLine + 1, 0);
            int nextLineEnd = textArea.Document.GetOffset(currentLine + 2, 0);
            string nextLineText = textArea.Document.GetText(nextLineStart, nextLineEnd - nextLineStart);

            // Swap the current line with the next line
            textArea.Document.Replace(currentLineStart, currentLineEnd - currentLineStart, nextLineText);
            textArea.Document.Replace(nextLineStart, nextLineEnd - nextLineStart, currentLineText);

            // Move the caret to the new current line
            textArea.Caret.Line++;
        }

        /// <summary>
        /// Handles the RemoveEmptyLines event of the MainMenu_LineOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_RemoveEmptyLines(object sender, RoutedEventArgs e)
        {
            string[] nonEmptyLines = textEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            textEditor.Text = string.Join(Environment.NewLine, nonEmptyLines);
        }

        /// <summary>
        /// Handles the InsertBlankLineAboveCurrent event of the MainMenu_LineOperations control.
        /// In the above code, _textEditor is an instance of AvalonEdit.TextEditor.
        ///You can call the InsertBlankLineAbove() when the user wants to insert a blank line above the current line.
        ///You can bind the method to appropriate keybindings or button clicks.
        ///This method is using the Environment.NewLine property to insert a newline character which is platform
        ///dependent(\r\n on Windows and \n on Linux and macOS). If you want to use a specific newline character
        ///you can use "\r\n" or "\n" instead.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_InsertBlankLineAboveCurrent(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            int currentLine = textArea.Caret.Line;
            int currentLineStart = textEditor.Document.GetLineByNumber(currentLine).Offset;
            string textToInsert = Environment.NewLine;
            textEditor.Document.Insert(currentLineStart, textToInsert);
        }

        /// <summary>
        /// Handles the InsertBlankLineBelowCurrent event of the MainMenu_LineOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_InsertBlankLineBelowCurrent(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            int currentLine = textArea.Caret.Line;
            int currentLineStart = textEditor.Document.GetLineByNumber(currentLine).EndOffset;
            string textToInsert = Environment.NewLine;
            textEditor.Document.Insert(currentLineStart, textToInsert);
        }
    }
}
