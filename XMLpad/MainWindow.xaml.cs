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

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string mFilename;
        private static bool isFullScreen = false;
        Microsoft.Win32.OpenFileDialog mDlgOpen = new Microsoft.Win32.OpenFileDialog();
        Microsoft.Win32.SaveFileDialog mDlgSave = new Microsoft.Win32.SaveFileDialog();
        PrintDialog mDlgPrint;
        CurrentFile mCurrentFile;


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
            catch (IOException e)
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
            this.Title = mCurrentFile.FileName + " - XMLpad";
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
            //textEditor.DuplicateLine();//TODO Copy the files and make them editable
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
        private void MainMenu_IncreaseIndent(object sender, RoutedEventArgs e)
        {
            //todo find the string between the cursor and the previous carriage return \r\n and insert  tab or 4 spacesdepending on prefernece
        }

        private void MainMenu_Copy_CurrentFilePath(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Copy_CurrentFileName(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_DecreaseIndent(object sender, RoutedEventArgs e)
        {
        }

        #region ConvertCase
        private void MainMenu_ConvertCase_AllUppercase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText =  textEditor.SelectedText.ToUpper();
        }
        private void MainMenu_ConvertCase_AllLowercase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText = textEditor.SelectedText.ToLower();
        }

        // https://stackoverflow.com/questions/3681552/reverse-case-of-all-alphabetic-characters-in-c-sharp-string
        private void MainMenu_ConvertCase_InvertCase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText = new string(
                textEditor.SelectedText.Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                    char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
        }
        // https://stackoverflow.com/questions/43969153/converting-random-letters-within-a-string-to-upper-lower-case
        private void MainMenu_ConvertCase_RandomCase(object sender, RoutedEventArgs e)
        {
            var randomizer = new Random();
            var final =
                textEditor.SelectedText.Select(x => randomizer.Next() % 2 == 0 ?
                (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
            var randomUpperLower = new string(final.ToArray());
            textEditor.SelectedText = randomUpperLower;
        }
        #endregion
        #region LineOperations
        private void MainMenu_LineOperations_DuplicateLine(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_RemoveDuplicateLines(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_RemoveConsecutiveDuplicateLines(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_MoveUpCurrentLine(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_MoveDownCurrentLine(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_RemoveEmptyLines(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_InsertBlankLineAboveCurrent(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_LineOperations_InsertBlankLineBelowCurrent(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region Comment/Uncomment
        private void MainMenu_Comment(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Uncomment(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region BlankOperations
        private void MainMenu_Edit_BlankOperations_TrimTrailingSpace(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Edit_BlankOperations_TrimLeadingSpace(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Edit_BlankOperations_TrimTrailingAndLeadingSpace(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Edit_BlankOperations_TabToSpace(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Edit_BlankOperations_SpaceToTabLeading(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Edit_BlankOperations_SpaceToTabTrailing(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Edit_BlankOperations_SpaceToTabAll(object sender, RoutedEventArgs e)
        {
        }
        #endregion
        private void MainMenu_Edit_SetAsReadOnly(object sender, RoutedEventArgs e)
        {
        }

        #region View
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
        private void MainMenu_View_ShowSymbol_ShowSpacesAndTabs(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_View_ShowSymbol_ShowNewLines(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_View_ShowSymbol_ShowAllCharacters(object sender, RoutedEventArgs e)
        {
        }
        #endregion
        #endregion
        #region Zoom
        private void MainMenu_View_ZoomIn(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_View_ZoomOut(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_View_ZoomDefault(object sender, RoutedEventArgs e)
        {
        }
        #endregion
        #region Settings/Help
        private void MainMenu_Settings_EnvironmentPreferences(object sender, RoutedEventArgs e)
        {
        }
        private void MainMenu_Help_ViewManual(object sender, RoutedEventArgs e)
        {
            ManualWindow manualWindow = new ManualWindow();
            manualWindow.Show();
        }
        #endregion
    }
}
