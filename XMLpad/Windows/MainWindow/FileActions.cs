using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Media;

namespace XMLpad
{
    /// <summary>
    /// File manipulation logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Handles the NewFile event of the MainMenu_File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_NewFile(object sender, RoutedEventArgs e)
        {
            SaveCurrentFile();
            Welcome_Window welcome_Window = new Welcome_Window(currentTheme);
            welcome_Window.CreateNewButon_Click(sender, e);

            // Open the file
            StreamReader reader = new(mCurrentFile.FilePath);
            mFilename = mCurrentFile.FileName;
            textEditor.Text = reader.ReadToEnd();
            reader.Close();
            UpdateTitle();
        }

        /// <summary>
        /// Saves the current file.
        /// </summary>
        /// <param name="text">The text.</param>
        private void SaveCurrentFile()
        {
            StreamWriter writer = new(mCurrentFile.FilePath);
            writer.Write(textEditor.Text);
            writer.Flush();
            writer.Close();
            mLoadedFile = textEditor.Text;
            UpdateStatusBar(pAppend: true, $" Wrote {textEditor.Text.Length} chars in {mFilename}");
        }

        /// <summary>
        /// Handles the CompareFileWithAnother event of the MainMenu_File control.
        /// Saves the current file and open the compare files window.
        /// There the window will open the Open dialog and will compare the selected file with the current one.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_File_CompareFileWithAnother(object sender, RoutedEventArgs e)
        {
            SaveCurrentFile();
            CompareFiles compareFiles = new(currentTheme)
            {
                Owner = this
            };
            compareFiles.Show();
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

            XmlFoldingStrategy mFoldingStrategy = new();

            // Update the text folding as well
            try
            {
                // TODO: Breaks the program the first time when we start it and the xml is foldable
                mFoldingStrategy.UpdateFoldings(mFoldingManager, textEditor.Document);
            }
            catch { }
            // Add the * as we have modified the file
            if (mLoadedFile != textEditor.Text)
            {
                if (!Title.Text.StartsWith("*"))
                {
                    Title.Text = '*' + Title.Text;
                }
            }
            else
            {
                Title.Text = Title.Text.Replace("*", string.Empty);
            }
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
                StreamReader reader = new(mDlgOpen.FileName);
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
            mDlgSave.FileName = string.Empty;
            mDlgSave.FileName += mCurrentFile.FileName;

            mDlgSave.FileName += pCopy ? " - Copy" : "";
            // Set the filter for the file extension

            mDlgSave.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";

            // Set the default file extension
            mDlgSave.DefaultExt = ".xml";

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
            FileStream fileStream = new(mCurrentFile.FilePath, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new(fileStream);
            writer.Write(textEditor.Text);
            writer.Flush();
            writer.Close();
            mLoadedFile = textEditor.Text;
            UpdateStatusBar(false);

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
            Window_Closing(sender, null);
        }

        /// <summary>
        /// Saves the temporary file.
        /// </summary>
        private void SavePreferenceFile()
        {
            FileStream fileStream;
            try
            {
                fileStream = new FileStream("C:/temp/XMLPadPreferences.txt", FileMode.Truncate, FileAccess.Write);

            }
            catch (FileNotFoundException)
            {
                fileStream = new FileStream("C:/temp/XMLPadPreferences.txt", FileMode.Create, FileAccess.Write);
            }
            StreamWriter writer = new(fileStream);
            writer.Write(currentTheme);
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
            // TODO: add a small window to ask to save the file if there are differences

            if (Title.Text.Contains('*'))
            {
                MessageBoxResult savefileYesNo = System.Windows.MessageBox.Show("Would you like to save the changes before you exit?", "Save File?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (savefileYesNo == MessageBoxResult.Yes)
                {
                    SaveCurrentFile();
                }
            }
            SavePreferenceFile();

            Environment.Exit(0);
        }
    }
}
