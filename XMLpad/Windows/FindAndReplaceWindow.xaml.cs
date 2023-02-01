namespace XMLpad
{
    using ICSharpCode.AvalonEdit;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Media;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Interaction logic for FindAndReplaceWindow.xaml
    /// </summary>
    public partial class FindAndReplaceWindow : Window
    {
        /// <summary>
        /// Gets or sets a value indicating whether [match case].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [match case]; otherwise, <c>false</c>.
        /// </value>
        public bool MatchCase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [match whole word].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [match whole word]; otherwise, <c>false</c>.
        /// </value>
        public bool MatchWholeWord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [match reg-ex].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [match reg-ex]; otherwise, <c>false</c>.
        /// </value>
        public bool MatchRegex { get; set; }

        /// <summary>
        /// The text editor
        /// </summary>
        private readonly TextEditor _textEditor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindAndReplaceWindow"/> class.
        /// </summary>

        public FindAndReplaceWindow(TextEditor textEditor, MainWindow.theme theme)
        {
            InitializeComponent();
            _textEditor = textEditor;


            MatchCaseCheckBox.Checked += (s, e) => MatchCase = true;
            MatchCaseCheckBox.Unchecked += (s, e) => MatchCase = false;

            MatchWholeWordCheckBox.Checked += (s, e) => MatchWholeWord = true;
            MatchWholeWordCheckBox.Unchecked += (s, e) => MatchWholeWord = false;

            MatchRegexCheckBox.Checked += (s, e) => MatchRegex = true;
            MatchRegexCheckBox.Unchecked += (s, e) => MatchRegex = false;

            ReplaceTextBox.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(ReplaceTextBox.Text))
                {
                    FindReplaceButton.Visibility = Visibility.Visible;
                    FindReplaceAllButton.Visibility = Visibility.Visible;
                }
                else
                {
                    FindReplaceButton.Visibility = Visibility.Collapsed;
                    FindReplaceAllButton.Visibility = Visibility.Collapsed;
                }
            };

            if (theme == MainWindow.theme.Dark)
            {
                findAndReplaceWindow.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                FindTextBox.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                FindTextBox.Foreground = Brushes.White;
                ReplaceTextBox.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                ReplaceTextBox.Foreground = Brushes.White;
                foreach (var label in FindVisualChildren<Label>(FindAndReplaceGrid))
                {
                    label.Foreground = Brushes.White;
                }
                FindButton.Foreground = Brushes.White;
                FindReplaceAllButton.Foreground = Brushes.White;
                FindReplaceButton.Foreground = Brushes.White;
                FindButton.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                FindReplaceAllButton.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                FindReplaceButton.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                MatchCaseCheckBox.Foreground = Brushes.White;
                MatchWholeWordCheckBox.Foreground = Brushes.White;
                MatchRegexCheckBox.Foreground = Brushes.White;
            }
        }

        /// <summary>
        /// Finds the visual children.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj">The dep object.</param>
        /// <returns></returns>
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the FindButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            bool found = false;
            int start = _textEditor.SelectionStart == _textEditor.Text.Length ? 0 : _textEditor.SelectionStart + 1;
            int index = start;
            while (index != -1)
            {
                if (MatchRegex)
                {
                    try
                    {
                        Match match = Regex.Match(_textEditor.Text, FindTextBox.Text, RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            index = match.Index;
                        }
                        else
                        {
                            index = -1;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid Regular Expression", "Find and Replace", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }
                else
                {
                    index = _textEditor.Text.IndexOf(FindTextBox.Text, index, MatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                }

                if (index == -1) break;

                if (MatchWholeWord)
                {
                    bool isWholeWord = MatchWholeWord;
                    if (isWholeWord)
                    {
                        int startWord = index;
                        int end = index + FindTextBox.Text.Length;
                        isWholeWord = (startWord == 0 || !char.IsLetterOrDigit(_textEditor.Text[startWord - 1])) &&
                                      (end == _textEditor.Text.Length || !char.IsLetterOrDigit(_textEditor.Text[end]));
                    }

                    if (!isWholeWord)
                    {
                        index++;
                        continue;
                    }
                }

                _textEditor.Select(index, FindTextBox.Text.Length);
                _textEditor.ScrollToLine(_textEditor.Document.GetLineByOffset(index).LineNumber);
                _textEditor.Focus();
                found = true;
                break;
            }
            if (!found)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("No matches until the end of the file found.", "Find and Replace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        /// <summary>
        /// Handles the Click event of the FindReplaceButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FindReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_textEditor.SelectedText == FindTextBox.Text || (MatchRegexCheckBox.IsChecked.Value && Regex.IsMatch(_textEditor.Text, FindTextBox.Text)))
            {
                int start = _textEditor.SelectionStart;
                int length = _textEditor.SelectionLength;
                _textEditor.Document.Replace(start, length, ReplaceTextBox.Text);
                _textEditor.Select(start, ReplaceTextBox.Text.Length);
                _textEditor.Focus();
            }
            else
            {
                FindButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the ReplaceAllButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ReplaceAllButton_Click(object sender, RoutedEventArgs e)
        {
            string findText = FindTextBox.Text;
            string replaceText = ReplaceTextBox.Text;
            int start = 0;
            int count = 0;
            bool matchCase = MatchCaseCheckBox.IsChecked ?? false;
            bool matchWholeWord = MatchWholeWordCheckBox.IsChecked ?? false;

            while (true)
            {
                start = _textEditor.Text.IndexOf(findText, start,
                    matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                if (start == -1)
                {
                    break;
                }

                if (matchWholeWord)
                {
                    // Check if the word is surrounded by whitespace or start/end of line
                    if ((start > 0 && !char.IsWhiteSpace(_textEditor.Text[start - 1])) ||
                        (start + findText.Length < _textEditor.Text.Length && !char.IsWhiteSpace(_textEditor.Text[start + findText.Length])))
                    {
                        start++;
                        continue;
                    }
                }

                _textEditor.SelectedText = replaceText;
                start += replaceText.Length;
                count++;
            }

            if (count == 0)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show("No matches until the end of the file found.", "Find and Replace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
