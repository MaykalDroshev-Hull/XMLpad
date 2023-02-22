using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace XMLpad
{
    /// <summary>
    /// Utilities logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Handles the MouseDown event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Handles the Click event of the MinimiseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MinimiseButton_Click(object sender, RoutedEventArgs e)
        {
            MinimiseButton_Click(sender, e, WindowState);
        }

        /// <summary>
        /// Minimizes the window
        /// </summary>
        /// <param name="sender">The source of the events.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MinimiseButton_Click(object sender, RoutedEventArgs e, WindowState windowState)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Maximises or restores the window
        /// </summary>
        /// <param name="sender">The source of the events.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MaximiseButton_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="sender">The source of the events.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// Gathers the completion string.
        /// </summary>
        private void GatherCompletionString()
        {
            mCompletionStrings.Clear();

            int currentIndex = 0;
            int startIndex = -1;
            while (currentIndex < textEditor.Document.TextLength)
            {
                char currentChar = textEditor.Document.GetCharAt(currentIndex);

                if (currentChar == '<')
                {
                    startIndex = currentIndex;
                }
                else if (currentChar == '>')
                {
                    int endIndex = currentIndex;
                    if (startIndex != -1 && endIndex != -1)
                    {
                        string word = textEditor.Document.GetText(startIndex + 1, endIndex - startIndex - 1).Replace("/", "");
                        if (!mCompletionStrings.Contains(word))
                            mCompletionStrings.Add(word);
                        startIndex = -1;
                    }
                }

                currentIndex++;
            }
        }

        /// <summary>
        /// Handles the TextEntered event of the textEditor_TextArea control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        void TextEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            mCompletionWindow = new CompletionWindow(textEditor.TextArea);

            if (e.Text == "<")
            {
                // Open code completion after the user has pressed dot:
                IList<ICompletionData> mData = mCompletionWindow.CompletionList.CompletionData;
                foreach (string entry in mCompletionStrings)
                {
                    mData.Add(new MyCompletionData(entry));
                }
                if (mData.Count != 0)
                {
                    mCompletionWindow.Show();
                }
                mCompletionWindow.Closed += delegate
                {
                    mCompletionWindow = null;
                };
            }
            else if (e.Text == ">")
            {
                int currentPos = textEditor.CaretOffset;
                StringBuilder result = new();
                while (currentPos > 0)
                {
                    char currentChar = textEditor.Document.GetCharAt(currentPos - 1);
                    if (currentChar == '<')
                    {
                        break;
                    }
                    result.Append(currentChar);
                    currentPos--;
                }

                // [.. means substring
                string reversed = new string(result.ToString().Reverse().ToArray())[..(result.Length - 1)];
                if (!reversed.Contains('/'))
                {
                    mOpenedStrings.Add(reversed.Replace("/", "").Replace("\\", ""));
                }
                reversed = reversed.Replace("/", "").Replace("\\", "");
                if (!mCompletionStrings.Contains(reversed))
                    mCompletionStrings.Add(reversed);

            }
            else if (e.Text == "/")
            {
                // Open code completion after the user has pressed dot:
                IList<ICompletionData> mData = mCompletionWindow.CompletionList.CompletionData;
                foreach (string entry in mOpenedStrings)
                {
                    mData.Add(new MyCompletionData(entry));
                }
                if (mData.Count != 0)
                {
                    mCompletionWindow.Show();
                }
                mCompletionWindow.Closed += delegate
                {
                    mCompletionWindow = null;
                };
                mCompletionWindow.CompletionList.SelectionChanged += (sender, e) =>
                {
                    ICompletionData selectedData = mCompletionWindow.CompletionList.SelectedItem;
                    if (selectedData != null)
                    {
                        mOpenedStrings.Remove(((MyCompletionData)selectedData).Text);
                    }
                };
            }
        }


        /// <summary>
        /// Handles the TextEntering event of the textEditor_TextArea control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        void TextEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && mCompletionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    mCompletionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // Do not set e.Handled=true.
            // We still want to insert the character that was typed.
        }
        /// <summary>
        /// Generates the initialization text.
        /// </summary>
        private void GenerateInitializationText()
        {
            try
            {
                mCurrentFile = CurrentFile.GetInstance();
                if (mCurrentFile.FileName == null)
                {
                    FileStream fileStream = new("C:/temp/tempfile.txt", FileMode.Open, FileAccess.Read);
                    StreamReader reader = new(fileStream);
                    textEditor.Text = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    FileStream fileStream = new(mCurrentFile.FilePath, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new(fileStream);
                    textEditor.Text = reader.ReadToEnd();
                    reader.Close();
                    UpdateTitle();
                }
            }
            catch (IOException)
            {
                textEditor.Text = "Open a file or start writing a new one";
            }

            // Load the string that we are going to compare the edited file with
            mLoadedFile = textEditor.Text;
            UpdateStatusBar(pAppend: false);
        }

        /// <summary>
        /// Updates the title.
        /// </summary>
        private void UpdateTitle()
        {
            Title.Text = mCurrentFile.FileName + " - XMLpad";
        }

        /// <summary>
        /// Handles the TextChanged event of the textEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        public void TextEditor_TextChanged(object sender, KeyEventArgs e) => UpdateStatusBar(pAppend: false);

        /// <summary>
        /// Handles the PreviewMouseDoubleClick event of the textEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TextEditor_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) => UpdateStatusBar(pAppend: false);

    }
}
