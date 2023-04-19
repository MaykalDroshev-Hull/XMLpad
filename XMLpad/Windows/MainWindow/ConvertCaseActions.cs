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
            textEditor.SelectedText = InvertCase(textEditor.SelectedText);
        }

        // https://stackoverflow.com/questions/43969153/converting-random-letters-within-a-string-to-upper-lower-case
        /// <summary>
        /// Handles the RandomCase event of the MainMenu_ConvertCase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_ConvertCase_RandomCase(object sender, RoutedEventArgs e)
        {
            textEditor.SelectedText = RandomiseCase(textEditor.SelectedText);
        }
    }
}
