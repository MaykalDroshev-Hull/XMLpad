namespace XMLpad
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Blank Operations logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        private void MainMenu_Edit_BlankOperations_TabToSpace(object sender, RoutedEventArgs e) => textEditor.Text = textEditor.Text.Replace("\t", new string(' ', mTabSpacesCount));

        /// <summary>
        /// Handles the SpaceToTabLeading event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabLeading(object sender, RoutedEventArgs e)
        {
            StringBuilder result = new();
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
    }
}
