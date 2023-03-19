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
