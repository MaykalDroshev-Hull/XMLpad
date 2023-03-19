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
    using ICSharpCode.AvalonEdit.Document;
    using System;
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
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_RemoveDuplicateLines(object sender, RoutedEventArgs e)
        {
            textEditor.Text = RemoveDuplicateLinesFromString(textEditor.Text);
        }

        /// <summary>
        /// Handles the RemoveConsecutiveDuplicateLines event of the MainMenu_LineOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_LineOperations_RemoveConsecutiveDuplicateLines(object sender, RoutedEventArgs e)
        {
            textEditor.Text = RemoveConsecutiveDuplicateLinesFromString(textEditor.Text);
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
            textEditor.Text = RemoveEmtyLinesFromString(textEditor.Text);
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
