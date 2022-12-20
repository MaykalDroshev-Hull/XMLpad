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

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string mFilename;
        Microsoft.Win32.OpenFileDialog mDlgOpen = new Microsoft.Win32.OpenFileDialog();
        Microsoft.Win32.SaveFileDialog mDlgSave = new Microsoft.Win32.SaveFileDialog();
        PrintDialog mDlgPrint;


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
                FileStream fileStream = new FileStream("C:/temp/tempfile.txt", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fileStream);
                textEditor.Text = reader.ReadToEnd();
                reader.Close();
            }catch(IOException)
            {
                textEditor.Text = "Open a file or start writing a new one";
            }

            UpdateStatusBar(pAppend: false);
            // TODO: check if a file is saved
            // Then load the file
            // Else create NewFile.xml and opn it here
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
            if(true/* TODO: text != textFile.text*/)
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
            StreamWriter writer = new StreamWriter(mFilename);
            writer.Write(textEditor.Text);
            writer.Flush();
            writer.Close();
            UpdateStatusBar( pAppend: true, $" Wrote {textEditor.Text.Length} chars in { mFilename}");
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
                mFilename = mDlgOpen.FileName;
                textEditor.Text = reader.ReadToEnd();
                reader.Close();
                UpdateStatusBar(pAppend: true, "Read " + mDlgOpen.FileName);
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
            FileStream fileStream = new FileStream(mDlgSave.FileName, FileMode.Create, FileAccess.Write);
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

            }catch(FileNotFoundException)
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
    }
}
