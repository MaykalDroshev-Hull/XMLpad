namespace XMLpad
{
    using ICSharpCode.AvalonEdit.Folding;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System;
    using System.Windows.Controls;
    using System.Windows;

    /// <summary>
    /// View actions logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Changes to show/ hide line numbers.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowLineNumbers(object sender, RoutedEventArgs e)
        {
            textEditor.ShowLineNumbers = !textEditor.ShowLineNumbers;
            mainMenu_showLineNumbers.IsChecked = textEditor.ShowLineNumbers;
        }

        /// <summary>
        /// Changes to show/ hide line numbers.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowXMLTree(object sender, RoutedEventArgs e)
        {
            if (XMLNodeTree.Visibility == Visibility.Visible)
            {
                XMLNodeTree.Visibility = Visibility.Collapsed;
                mainMenu_showXMLTree.IsChecked = false;
                Grid.SetColumnSpan(textEditor, 2);
            }
            else
            {
                XMLNodeTree.Visibility = Visibility.Visible;
                mainMenu_showXMLTree.IsChecked = true;
                Grid.SetColumnSpan(textEditor, 1);
            }
        }

        /// <summary>
        /// Handles the ToggleFullScreen event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ToggleFullScreen(object sender, RoutedEventArgs e)
        {
            if (!isFullScreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                menuItem_FullScreen.Header = "Toggle Normal Screen Mode";
            }
            else
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Normal;
                menuItem_FullScreen.Header = "Toggle Full Screen Mode";
            }
            isFullScreen = !isFullScreen;
        }

        /// <summary>
        /// The folding strategy
        /// </summary>
        dynamic? foldingStrategy;

        /// <summary>
        /// The highlighting definition
        /// </summary>
        IHighlightingDefinition? highlightingDefinition;

        /// <summary>
        /// Handles the Click event of the Language control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Language_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem)
                return;
            HighlightLanguage language;
            if (menuItem.Header.ToString() == "C#")
            {
                language = HighlightLanguage.CSharp;
            }
            else if (menuItem.Header.ToString() == "C++")
            {
                language = HighlightLanguage.Cpp;

            }
            else
            {
                language = (HighlightLanguage)Enum.Parse(typeof(HighlightLanguage), menuItem.Header.ToString());
            }
            switch (language)
            {
                case HighlightLanguage.XML:
                case HighlightLanguage.HTML:
                    foldingStrategy = new XmlFoldingStrategy();
                    textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
                    highlightingDefinition = HighlightingManager.Instance.GetDefinition("XML");
                    break;
                case HighlightLanguage.CSharp:
                case HighlightLanguage.Cpp:
                case HighlightLanguage.PHP:
                case HighlightLanguage.Java:
                    textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
                    foldingStrategy = new BraceFoldingStrategy();
                    highlightingDefinition = HighlightingManager.Instance.GetDefinition("C#");
                    break;
                default:
                    textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
                    foldingStrategy = null;
                    highlightingDefinition = null;
                    break;
            }

            textEditor.SyntaxHighlighting = highlightingDefinition;


            if (foldingStrategy != null)
            {
                mFoldingManager ??= FoldingManager.Install(textEditor.TextArea);
                foldingStrategy.UpdateFoldings(mFoldingManager, textEditor.Document);
            }
            else
            {
                if (mFoldingManager != null)
                {
                    FoldingManager.Uninstall(mFoldingManager);
                    mFoldingManager = null;
                }
            }
        }
        #region ShowSymbols

        /// <summary>
        /// Handles the ShowSpacesAndTabs event of the MainMenu_View_ShowSymbol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowSymbol_ShowSpacesAndTabs(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowSpaces = !textEditor.Options.ShowSpaces;
            textEditor.Options.ShowTabs = textEditor.Options.ShowSpaces;
            menuItem_showSpacesTabs.IsChecked = textEditor.Options.ShowTabs;

        }

        /// <summary>
        /// Handles the ShowNewLines event of the MainMenu_View_ShowSymbol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowSymbol_ShowNewLines(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowEndOfLine = !textEditor.Options.ShowEndOfLine;
            menuItem_showNewLines.IsChecked = textEditor.Options.ShowEndOfLine;

        }

        /// <summary>
        /// Handles the ShowAllCharacters event of the MainMenu_View_ShowSymbol control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ShowSymbol_ShowAllCharacters(object sender, RoutedEventArgs e)
        {
            textEditor.Options.ShowSpaces = !textEditor.Options.ShowSpaces;
            textEditor.Options.ShowTabs = textEditor.Options.ShowSpaces;
            textEditor.Options.ShowEndOfLine = textEditor.Options.ShowSpaces;
            menuItem_showSpacesTabs.IsChecked = textEditor.Options.ShowSpaces;
            menuItem_showNewLines.IsChecked = textEditor.Options.ShowSpaces;
        }
        #endregion

        #region Zoom

        /// <summary>
        /// Handles the ZoomIn event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ZoomIn(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize += 2;

        }

        /// <summary>
        /// Handles the ZoomOut event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ZoomOut(object sender, RoutedEventArgs e)
        {
            if (textEditor.FontSize < 2)
                return;

            textEditor.FontSize -= 2;
        }

        /// <summary>
        /// Handles the ZoomDefault event of the MainMenu_View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_View_ZoomDefault(object sender, RoutedEventArgs e)
        {
            textEditor.FontSize = defaultFontSize;
        }
        #endregion

        /// <summary>
        /// Handles the SetAsReadOnly event of the MainMenu_Edit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Edit_SetAsReadOnly(object sender, RoutedEventArgs e)
        {
            textEditor.IsReadOnly = !textEditor.IsReadOnly;
            readOnlyMenuItem.IsChecked = textEditor.IsReadOnly;
        }
    }
}
