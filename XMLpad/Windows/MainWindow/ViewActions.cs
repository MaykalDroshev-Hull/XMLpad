namespace XMLpad
{
    using ICSharpCode.AvalonEdit.Folding;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System;
    using System.Windows.Controls;
    using System.Windows;
    using System.IO;

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
        /// Handles the ShowXMLTree event of the MainMenu_View control.
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
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                menuItem_FullScreen.Header = "Toggle Normal Screen Mode";
            }
            else
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Normal;
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

            switch (menuItem.Header)
            {
                case "_XML":
                case "_HTML":
                    foldingStrategy = new XmlFoldingStrategy();
                    textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
                    highlightingDefinition = HighlightingManager.Instance.GetDefinition("XML");

                    XMLNodeTree.Visibility = Visibility.Visible;
                    mainMenu_showXMLTree.IsChecked = true;
                    Grid.SetColumnSpan(textEditor, 1);
                    break;
                case "_C#":
                case "_C++":
                case "_PHP":
                case "_Java":
                    textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(textEditor.Options);
                    foldingStrategy = new BraceFoldingStrategy();
                    highlightingDefinition = HighlightingManager.Instance.GetDefinition("C#");
                    XMLNodeTree.Visibility = Visibility.Collapsed;
                    mainMenu_showXMLTree.IsChecked = false;
                    Grid.SetColumnSpan(textEditor, 2);
                    break;
                default:
                    textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
                    foldingStrategy = null;
                    highlightingDefinition = null;
                    XMLNodeTree.Visibility = Visibility.Collapsed;
                    mainMenu_showXMLTree.IsChecked = false;
                    Grid.SetColumnSpan(textEditor, 2);
                    break;
            }

            SetCurrentLangauge(menuItem.Header);

            // collapse the XML tree if the highlight language is not XML


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
            ChangeLetterColours(textEditor, currentTheme);
        }


        private void Example_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem)
                return;

            AskUserToSaveFile();

            switch (menuItem.Header)
            {
                case "_XML":
                    textEditor.Text = ReadTextFile("ExampleFiles/XMLExample.xml");
                    currentLanguage = HighlightLanguage.XML;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_XmlDoc":
                    textEditor.Text = ReadTextFile("ExampleFiles/XmlDocExample.xml");
                    currentLanguage = HighlightLanguage.XML;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_C#":
                    textEditor.Text = ReadTextFile("ExampleFiles/CSharpExample.cs");
                    currentLanguage = HighlightLanguage.CSharp;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_JavaScript":
                    textEditor.Text = ReadTextFile("ExampleFiles/JavaScriptExample.js");
                    currentLanguage = HighlightLanguage.JavaScript;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_HTML":
                    textEditor.Text = ReadTextFile("ExampleFiles/HTMLExample.html");
                    currentLanguage = HighlightLanguage.HTML;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_Boo":
                    textEditor.Text = ReadTextFile("ExampleFiles/BooExample.boo");
                    currentLanguage = HighlightLanguage.Boo;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_Coco":
                    textEditor.Text = ReadTextFile("ExampleFiles/CocoExample.atg");
                    currentLanguage = HighlightLanguage.Coco;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_CSS":
                    textEditor.Text = ReadTextFile("ExampleFiles/CSSExample.css");
                    currentLanguage = HighlightLanguage.CSS;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_C++":
                    textEditor.Text = ReadTextFile("ExampleFiles/CPPExample.cpp");
                    currentLanguage = HighlightLanguage.Cpp;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_Java":
                    textEditor.Text = ReadTextFile("ExampleFiles/JavaExample.java");
                    currentLanguage = HighlightLanguage.Java;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_PowerShell":
                    textEditor.Text = ReadTextFile("ExampleFiles/PowerShellExample.ps1");
                    currentLanguage = HighlightLanguage.PowerShell;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_PHP":
                    textEditor.Text = ReadTextFile("ExampleFiles/PHPExample.php");
                    currentLanguage = HighlightLanguage.PHP;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_VBNET":
                    textEditor.Text = ReadTextFile("ExampleFiles/VBNETExample.vb");
                    currentLanguage = HighlightLanguage.VBNET;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                case "_MarkDown":
                    textEditor.Text = ReadTextFile("ExampleFiles/MarkDownExample.md");
                    currentLanguage = HighlightLanguage.MarkDown;
                    ChangeLetterColours(textEditor, currentTheme);
                    break;
                default:
                    break;
            }
            switch (menuItem.Header)
            {
                case "_XML":
                case "_XmlDoc":
                    XMLNodeTree.Visibility = Visibility.Visible;
                    mainMenu_showXMLTree.IsChecked = true;
                    Grid.SetColumnSpan(textEditor, 1);
                    break;
                default:
                    XMLNodeTree.Visibility = Visibility.Collapsed;
                    mainMenu_showXMLTree.IsChecked = false;
                    Grid.SetColumnSpan(textEditor, 2);
                    break;
            }
        }

        /// <summary>
        /// Reads the text file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string ReadTextFile(string filePath)
        {
            string fileContent = "";

            try
            {
                // Open the text file using a stream reader.
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Read the entire file and store its contents in a string variable.
                    fileContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur while reading the file.
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return fileContent;
        }
        /// <summary>
        /// Sets the current langauge.
        /// </summary>
        /// <param name="menuItemHeader">The menu item header.</param>
        private void SetCurrentLangauge(object menuItemHeader)
        {
            switch (menuItemHeader)
            {
                case "_XML":
                    currentLanguage = HighlightLanguage.XML;
                    break;
                case "_XmlDoc":
                    currentLanguage = HighlightLanguage.XmlDoc;
                    break;
                case "_C#":
                    currentLanguage = HighlightLanguage.CSharp;
                    break;
                case "_JavaScript":
                    currentLanguage = HighlightLanguage.JavaScript;
                    break;
                case "_HTML":
                    currentLanguage = HighlightLanguage.HTML;
                    break;
                case "_Boo":
                    currentLanguage = HighlightLanguage.Boo;
                    break;
                case "_Coco":
                    currentLanguage = HighlightLanguage.Coco;
                    break;
                case "_CSS":
                    currentLanguage = HighlightLanguage.CSS;
                    break;
                case "_C++":
                    currentLanguage = HighlightLanguage.Cpp;
                    break;
                case "_Java":
                    currentLanguage = HighlightLanguage.Java;
                    break;
                case "_PowerShell":
                    currentLanguage = HighlightLanguage.PowerShell;
                    break;
                case "_PHP":
                    currentLanguage = HighlightLanguage.PHP;
                    break;
                case "_VBNET":
                    currentLanguage = HighlightLanguage.VBNET;
                    break;
                case "_MarkDown":
                    currentLanguage = HighlightLanguage.MarkDown;
                    break;
                default:
                    currentLanguage = HighlightLanguage.XML;
                    break;
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
