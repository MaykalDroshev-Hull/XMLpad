namespace XMLpad
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Converting and changing text casing logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            Random randomizer = new();
            IEnumerable<char> final =
                textEditor.SelectedText.Select(x => randomizer.Next() % 2 == 0 ?
                (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
            string randomUpperLower = new(final.ToArray());
            textEditor.SelectedText = randomUpperLower;
        }
    }
}
