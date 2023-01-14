using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Printing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.IO;
using System.Diagnostics;
using System.Windows.Automation;
using ICSharpCode.AvalonEdit.Document;
using System.Windows.Annotations;
using Microsoft.VisualBasic.ApplicationServices;
using ScintillaNET;
using static ScintillaNET.Style;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;
using System.Reflection.Metadata;

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private string mFilename;
        private static bool isFullScreen = false;
        Microsoft.Win32.OpenFileDialog mDlgOpen = new Microsoft.Win32.OpenFileDialog();
        Microsoft.Win32.SaveFileDialog mDlgSave = new Microsoft.Win32.SaveFileDialog();
        PrintDialog mDlgPrint;
        CurrentFile mCurrentFile;
        private static int mTabSpacesCount = 4; // TODO: Change to be dynamic
        private enum tabSelection
        {
            Tabs,
            Spaces
        };

        private static tabSelection currentTabSelection = tabSelection.Tabs;

        private string tabCharacters = currentTabSelection == tabSelection.Spaces ? new string(' ', mTabSpacesCount) : "\t";
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            Welcome_Window welcome_Window = new Welcome_Window();
            if (welcome_Window.ShowDialog() == false)
            {
                InitializeComponent();
                GenerateInitializationText();
                textEditor.Focus();
            }
        }

        /// <summary>
        /// Generates the initialization text.
        /// </summary>
        private void GenerateInitializationText()
        {
            try
            {
                mCurrentFile = CurrentFile.getInstance();
                if (mCurrentFile.FileName == null)
                {
                    FileStream fileStream = new FileStream("C:/temp/tempfile.txt", FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(fileStream);
                    textEditor.Text = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    FileStream fileStream = new FileStream(mCurrentFile.FilePath, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(fileStream);
                    textEditor.Text = reader.ReadToEnd();
                    reader.Close();
                    UpdateTitle();
                }
            }
            catch (IOException)
            {
                textEditor.Text = "Open a file or start writing a new one";
            }

            UpdateStatusBar(pAppend: false);
            // TODO: check if a file is saved
            // Then load the file
            // Else create NewFile.xml and opn it here
        }

        private void UpdateTitle()
        {
            Title = mCurrentFile.FileName + " - XMLpad";
        }

        /// <summary>
        /// Handles the TextChanged event of the textEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        public void textEditor_TextChanged(object sender, KeyEventArgs e) => UpdateStatusBar(pAppend: false);

        /// <summary>
        /// Handles the PreviewMouseDoubleClick event of the textEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void textEditor_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) => UpdateStatusBar(pAppend: false);

        /// <summary>
        /// Handles the NewFile event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_NewFile(object sender, RoutedEventArgs e) => CheckIfCurrentFileIsSaved(textEditor.Text);

        /// <summary>
        /// Checks if current file is saved.
        /// </summary>
        /// <param name="text">The text.</param>
        private void CheckIfCurrentFileIsSaved(string text)
        {
            if (true/* TODO: text != textFile.text*/)
            {
                SaveCurrentFile(text);
            }
        }

        /// <summary>
        /// Saves the current file.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SaveCurrentFile(/*mFilename*/ string text)
        {
            StreamWriter writer = new StreamWriter(mCurrentFile.FilePath);
            writer.Write(textEditor.Text);
            writer.Flush();
            writer.Close();
            UpdateStatusBar(pAppend: true, $" Wrote {textEditor.Text.Length} chars in {mFilename}");
        }

        /// <summary>
        /// Updates the status bar.
        /// </summary>
        /// <param name="pAppend">if set to <c>true</c> [p append].</param>
        /// <param name="pValue">The p value.</param>
        private void UpdateStatusBar(bool pAppend, string pValue = "")
        {
            if (pAppend)
                LineCharacterPosition.Content = $"{DateTime.Now.ToShortDateString()}  {DateTime.Now.ToShortTimeString()} : {pValue}";
            else
                LineCharacterPosition.Content = $"Line: {textEditor.TextArea.Caret.Line} Column: {textEditor.TextArea.Caret.Column} Chars: {textEditor.Text.ToCharArray().Length} ";
        }

        /// <summary>
        /// Handles the OpenFile event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_OpenFile(object sender, RoutedEventArgs e)
        {
            if (mDlgOpen.ShowDialog() == true)
            {
                StreamReader reader = new StreamReader(mDlgOpen.FileName);
                mFilename = mDlgOpen.SafeFileName;
                textEditor.Text = reader.ReadToEnd();
                reader.Close();
                UpdateStatusBar(pAppend: true, "Read " + mDlgOpen.FileName);
                mCurrentFile.FileName = mFilename;
                mCurrentFile.FilePath = mDlgOpen.FileName;
                UpdateTitle();
            }
        }

        /// <summary>
        /// Handles the OpenContainingFolder event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_OpenContainingFolder(object sender, RoutedEventArgs e) => Process.Start("explorer.exe", "/select," + mFilename);

        /// <summary>
        /// Handles the Save event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_Save(object sender, RoutedEventArgs e) => SaveFile();

        /// <summary>
        /// Handles the SaveAs event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_SaveAs(object sender, RoutedEventArgs e) => SaveDialogFile(pCopy: false);

        /// <summary>
        /// Saves the dialog file.
        /// </summary>
        /// <param name="pCopy">if set to <c>true</c> [p copy].</param>
        private void SaveDialogFile(bool pCopy)
        {
            mDlgSave.FileName += pCopy ? "- Copy" : "";
            if (mDlgSave.ShowDialog() == true)
            {
                SaveFile();
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        private void SaveFile()
        {
            FileStream fileStream = new FileStream(mCurrentFile.FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(textEditor.Text);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Handles the SaveACopyAs event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_SaveACopyAs(object sender, RoutedEventArgs e) => SaveDialogFile(pCopy: true);

        /// <summary>
        /// Handles the Print event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_Print(object sender, RoutedEventArgs e)
        {
            // Initialise a PrintDialog
            mDlgPrint = new PrintDialog();

            mDlgPrint.ShowDialog();
        }

        /// <summary>
        /// Handles the Clear event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_Clear(object sender, RoutedEventArgs e)
        {
            ResetDlgs();
            textEditor.Text = "";
        }

        /// <summary>
        /// Handles the Exit event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_Exit(object sender, RoutedEventArgs e)
        {
            SaveTempFile();
            Environment.Exit(0);
        }

        /// <summary>
        /// Saves the temporary file.
        /// </summary>
        private void SaveTempFile()
        {
            FileStream fileStream;
            try
            {
                fileStream = new FileStream("C:/temp/tempfile.txt", FileMode.Truncate, FileAccess.Write);

            }
            catch (FileNotFoundException)
            {
                fileStream = new FileStream("C:/temp/tempfile.txt", FileMode.Create, FileAccess.Write);
            }
            StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(textEditor.Text);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Resets the dialogs.
        /// </summary>
        private void ResetDlgs()
        {
            mDlgOpen.FileName = "";
            mDlgSave.FileName = "";
            UpdateStatusBar(pAppend: true, "Ready ");
        }

        /// <summary>
        /// Handles the Closing event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveTempFile();
        }

        private void MainMenu_Undo(object sender, RoutedEventArgs e) => textEditor.Undo();

        private void MainMenu_Redo(object sender, RoutedEventArgs e) => textEditor.Redo();

        private void MainMenu_Cut(object sender, RoutedEventArgs e) => textEditor.Cut();

        private void MainMenu_Copy(object sender, RoutedEventArgs e) => textEditor.Copy();

        private void MainMenu_DuplicateLine(object sender, RoutedEventArgs e)
        {
            MainMenu_LineOperations_DuplicateLine(sender, e);
        }

        private void MainMenu_Paste(object sender, RoutedEventArgs e) => textEditor.Paste();
        private void MainMenu_Delete(object sender, RoutedEventArgs e) => textEditor.Delete();
        private void MainMenu_SelectAll(object sender, RoutedEventArgs e) => textEditor.SelectAll();
        private void MainMenu_Insert_DateTimeShort(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText += DateTime.Now.ToShortDateString();
        }

        private void MainMenu_Insert_DateTimeLong(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText += DateTime.Now.ToLongDateString();
        }

        private void MainMenu_Insert_DateTimeCustomized(object sender, RoutedEventArgs e)
        {
            // todo insert the loaded custom date time format if created otherwise call MainMenu_Insert_CustomizeFormat after a dialog that it is not set
        }

        private void MainMenu_Insert_CustomizeFormat(object sender, RoutedEventArgs e)
        {
            // Todo: Create a window for customizing date time format
        }

        private IEnumerable<string> GetSelectedLines()
        {
            string selectedText = textEditor.SelectedText;
            return selectedText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// Handles the IncreaseIndent event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_IncreaseIndent(object sender, RoutedEventArgs e)
        {
            // TODO: Test
            var selectedText = textEditor.SelectedText;
            var indentedText = selectedText.Replace("\n", "\n" + tabCharacters);
            textEditor.Text.Replace(textEditor.SelectedText, indentedText);
        }

        private void MainMenu_Copy_CurrentFilePath(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Copy_CurrentFileName(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// Handles the DecreaseIndent event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_DecreaseIndent(object sender, RoutedEventArgs e)
        {
            var selectedText = textEditor.SelectedText;
            var indentedText = selectedText.Replace("\n" + tabCharacters, "\n");
            textEditor.Text.Replace(textEditor.SelectedText, indentedText);
        }

        #region ConvertCase

        /// <summary>
        /// Handles the AllUppercase event of the MainMenu_ConvertCase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_ConvertCase_AllUppercase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText = textEditor.SelectedText.ToUpper();
        }

        /// <summary>
        /// Handles the AllLowercase event of the MainMenu_ConvertCase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_ConvertCase_AllLowercase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText = textEditor.SelectedText.ToLower();
        }

        // https://stackoverflow.com/questions/3681552/reverse-case-of-all-alphabetic-characters-in-c-sharp-string
        /// <summary>
        /// Handles the InvertCase event of the MainMenu_ConvertCase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_ConvertCase_InvertCase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText = new string(
                textEditor.SelectedText.Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                    char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
        }
        // https://stackoverflow.com/questions/43969153/converting-random-letters-within-a-string-to-upper-lower-case
        /// <summary>
        /// Handles the RandomCase event of the MainMenu_ConvertCase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_ConvertCase_RandomCase(object sender, RoutedEventArgs e)
        {
            Random randomizer = new Random();
            IEnumerable<char> final =
                textEditor.SelectedText.Select(x => randomizer.Next() % 2 == 0 ?
                (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
            string randomUpperLower = new string(final.ToArray());
            textEditor.SelectedText = randomUpperLower;
        }
        #endregion
        #region LineOperations

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
            var currentLine = textEditor.Document.GetLineByOffset(textEditor.CaretOffset);
            var lineText = textEditor.Document.GetText(currentLine);
            var insertionOffset = currentLine.EndOffset;
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
            HashSet<string> uniqueLines = new HashSet<string>();
            StringBuilder result = new StringBuilder();

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
            StringBuilder output = new StringBuilder();
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
            var textArea = textEditor.TextArea;
            var currentLine = textArea.Caret.Line;
            if (currentLine == 0)
                return; // Already at the top, can't move up

            var currentLineStart = textArea.Document.GetOffset(currentLine, 0);
            var currentLineEnd = textArea.Document.GetOffset(currentLine + 1, 0);
            var currentLineText = textArea.Document.GetText(currentLineStart, currentLineEnd - currentLineStart);

            var previousLineStart = textArea.Document.GetOffset(currentLine - 1, 0);
            var previousLineEnd = textArea.Document.GetOffset(currentLine, 0);
            var previousLineText = textArea.Document.GetText(previousLineStart, previousLineEnd - previousLineStart);

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
            var line = textEditor.Document.GetLineByOffset(textEditor.CaretOffset);
            var lineNumber = line.LineNumber;
            var lineText = textEditor.Document.GetText(line);

            if (lineNumber == textEditor.Document.LineCount)
                return;

            var lineBelow = textEditor.Document.GetLineByNumber(lineNumber + 1);
            var lineBelowText = textEditor.Document.GetText(lineBelow);

            textEditor.Document.Replace(line, lineText + Environment.NewLine + lineBelowText);
            textEditor.Document.Remove(lineBelow.Offset, lineBelow.Length);
            textEditor.CaretOffset = line.Offset + line.Length;
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
        #endregion

        #region Comment/Uncomment

        /// <summary>
        /// Handles the Comment event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Comment(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            int start = textArea.Selection.StartPosition.Column;
            int end = textArea.Selection.EndPosition.Column;

            string selectedText = textEditor.Document.GetText(start, end - start);
            string commentedText = "<!--" + selectedText + "-->";
            textEditor.Document.Replace(start, end - start, commentedText);
        }

        /// <summary>
        /// Handles the Uncomment event of the MainMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Uncomment(object sender, RoutedEventArgs e)
        {
            ICSharpCode.AvalonEdit.Editing.TextArea textArea = textEditor.TextArea;
            int start = textArea.Selection.StartPosition.Column;
            int end = textArea.Selection.EndPosition.Column;

            string selectedText = textEditor.Document.GetText(start, end - start);
            string uncommentedText = selectedText.Replace("<!--", "").Replace("-->", "");
            textEditor.Document.Replace(start, end - start, uncommentedText);
        }
        #endregion

        #region BlankOperations

        /// <summary>
        /// Handles the TrimTrailingSpace event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_TrimTrailingSpace(object sender, RoutedEventArgs e)
        {
            string[] lines = textEditor.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
            textEditor.Text = string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Handles the TrimLeadingSpace event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_TrimLeadingSpace(object sender, RoutedEventArgs e)
        {
            textEditor.Text = string.Join("\n", textEditor.Text.Split('\n').Select(x => x.TrimStart()));
        }

        /// <summary>
        /// Handles the TrimTrailingAndLeadingSpace event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_TrimTrailingAndLeadingSpace(object sender, RoutedEventArgs e)
        {
            MainMenu_Edit_BlankOperations_TrimLeadingSpace(sender, e);
            MainMenu_Edit_BlankOperations_TrimTrailingSpace(sender, e);
        }

        /// <summary>
        /// Handles the TabToSpace event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_TabToSpace(object sender, RoutedEventArgs e)
        {
            textEditor.Text.Replace("\t", new string(' ', mTabSpacesCount));
        }

        /// <summary>
        /// Handles the SpaceToTabLeading event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabLeading(object sender, RoutedEventArgs e)
        {
            StringBuilder result = new StringBuilder();
            int spaces = 0;
            foreach (char c in textEditor.Text)
            {
                if (c == ' ')
                {
                    spaces++;
                    if (spaces == mTabSpacesCount)
                    {
                        result.Append('\t');
                        spaces = 0;
                    }
                }
                else
                {
                    if (spaces > 0)
                    {
                        result.Append(new string(' ', spaces));
                        spaces = 0;
                    }
                    result.Append(c);
                }
            }
            textEditor.Text = result.ToString();
        }

        /// <summary>
        /// Handles the SpaceToTabTrailing event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabTrailing(object sender, RoutedEventArgs e)
        {
            string[] lines = textEditor.Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int spacesCount = line.Length - line.TrimEnd().Length;
                int tabCount = spacesCount / mTabSpacesCount;
                lines[i] = line.TrimEnd() + new string('\t', tabCount);
            }
            textEditor.Text = string.Join("\n", lines);
        }

        /// <summary>
        /// Handles the SpaceToTabAll event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabAll(object sender, RoutedEventArgs e)
        {
            string[] lines = textEditor.Text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(new string(' ', mTabSpacesCount), "\t");
            }
            textEditor.Text = string.Join("\n", lines);
        }
        #endregion

        /// <summary>
        /// Handles the SetAsReadOnly event of the MainMenu_Edit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_SetAsReadOnly(object sender, RoutedEventArgs e)
        {
            textEditor.IsReadOnly = !textEditor.IsReadOnly;
        }

        #region View

        /// <summary>
        /// Handles the ToggleFullScreen event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ToggleFullScreen(object sender, RoutedEventArgs e)
        {
            if (!isFullScreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                MenuItem_FullScreen.Header = "Toggle Normal Screen Mode";
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                MenuItem_FullScreen.Header = "Toggle Full Screen Mode";
            }
            isFullScreen = !isFullScreen;
        }
        #region ShowSymbols

        /// <summary>
        /// Handles the ShowSpacesAndTabs event of the MainMenu_View_ShowSymbol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowSymbol_ShowSpacesAndTabs(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowSpaces = !textEditor.Options.ShowSpaces;
            textEditor.Options.ShowTabs = textEditor.Options.ShowSpaces;
        }

        /// <summary>
        /// Handles the ShowNewLines event of the MainMenu_View_ShowSymbol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowSymbol_ShowNewLines(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowEndOfLine = !textEditor.Options.ShowEndOfLine;
        }

        /// <summary>
        /// Handles the ShowAllCharacters event of the MainMenu_View_ShowSymbol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowSymbol_ShowAllCharacters(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowSpaces = !textEditor.Options.ShowSpaces;
            textEditor.Options.ShowTabs = textEditor.Options.ShowSpaces;
            textEditor.Options.ShowEndOfLine = textEditor.Options.ShowSpaces;
        }
        #endregion
        #endregion
        #region Zoom

        /// <summary>
        /// Handles the ZoomIn event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ZoomIn(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize += 2;

        }

        /// <summary>
        /// Handles the ZoomOut event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ZoomOut(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize -= 2;

        }

        /// <summary>
        /// Handles the ZoomDefault event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ZoomDefault(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize = 11;
        }
        #endregion
        #region Settings/Help

        /// <summary>
        /// Handles the EnvironmentPreferences event of the MainMenu_Settings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Settings_EnvironmentPreferences(object sender, RoutedEventArgs e)
        {
            // TODO: Open a new Window and have the settings there
        }

        /// <summary>
        /// Handles the ViewManual event of the MainMenu_Help control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Help_ViewManual(object sender, RoutedEventArgs e)
        {
            ManualWindow manualWindow = new ManualWindow();
            manualWindow.Show();
        }
        #endregion
    }
}
