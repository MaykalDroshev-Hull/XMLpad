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
            textEditor.Text = TrimTrailingSpacesFromString(textEditor.Text);
        }

        /// <summary>
        /// Handles the TrimLeadingSpace event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_TrimLeadingSpace(object sender, RoutedEventArgs e)
        {
            textEditor.Text = RemoveLeadingSpacesFromString(textEditor.Text);
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
            textEditor.Text = ConvertTabsToSpaces(textEditor.Text);
        }

        /// <summary>
        /// Handles the SpaceToTabLeading event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabLeading(object sender, RoutedEventArgs e)
        {
            textEditor.Text = ConvertSpacesToTabsLeading(textEditor.Text);
        }

        /// <summary>
        /// Handles the SpaceToTabTrailing event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabTrailing(object sender, RoutedEventArgs e)
        {
            textEditor.Text = ConvertSpacesToTabsTrailing(textEditor.Text);
        }

        /// <summary>
        /// Handles the SpaceToTabAll event of the MainMenu_Edit_BlankOperations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_BlankOperations_SpaceToTabAll(object sender, RoutedEventArgs e)
        {
            textEditor.Text = ConvertSpacesToTabs(textEditor.Text);
        }
    }
}
