namespace XMLpad
{
    using System.Windows.Media;
    using System.Windows;
    using System.IO;
    using System.Windows.Media.Imaging;
    using System;
    using System.Xml;
    using Window = System.Windows.Window;
    using System.Threading.Tasks;
    using ICSharpCode.AvalonEdit.Highlighting.Xshd;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System.Collections.Generic;

    /// <summary>
    /// Settings and help logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Handles the EnvironmentPreferences event of the MainMenu_Settings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Settings_EnvironmentPreferences(object sender, RoutedEventArgs e)
        {
            SwitchTheme(sender, e);
        }

        LoadingPopUp loadingPopup = new LoadingPopUp();

        /// <summary>
        /// Switches the theme.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SwitchTheme(object sender, RoutedEventArgs e)
        {
            // Show loading popup
            loadingPopup.Show();

            // Execute the method asynchronously on a separate thread
            await Task.Run(() =>
            {
                UIChanges(sender, e);
            });

            loadingPopup.Hide();
        }

        private void UIChanges(object sender, RoutedEventArgs e)
        {
            if (currentTheme == theme.Dark || (sender.Equals("firstBoot") && currentTheme == theme.Light))
            {
                Dispatcher.Invoke(() =>
                {
                    ChangeGeneralUIToLightMode();
                    ChangeXMLTreeViewToLightMode();
                    ChangeTitleBarToLightMode();
                    ChangeMenuIconsToLightMode();
                    ChangeContextIconsToLightMode();
                    ChangeLetterColours();
                });

                currentTheme = theme.Light;
                Dispatcher.Invoke(() =>
                {
                    TryLoadXML();
                });

            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    ChangeGeneralUIToDarkMode();
                    ChangeXmlTreeViewToDarkMode();
                    ChangeTitleBarToDarkMode();
                    ChangeMenuIconsToDarkMode();
                    ChangeContextIconsToDarkMode();
                    ChangeLetterColours();
                });

                currentTheme = theme.Dark;
                Dispatcher.Invoke(() =>
                {
                    TryLoadXML();
                });
            }

            var backgroundRenderer = new HighlightCurrentLineBackgroundRenderer(textEditor, currentTheme);
            Dispatcher.Invoke(() =>
            {

                textEditor.TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);

                UpdateStatusBar(pAppend: true, pValue: $"Theme changed!");
                ChangeLetterColours();
            });

        }

        /// <summary>
        /// Changes the letter colours.
        /// </summary>
        private void ChangeLetterColours()
        {
            var languageToFileName = new Dictionary<HighlightLanguage, string>
{
    { HighlightLanguage.XML, "XML" },
    { HighlightLanguage.XmlDoc, "XMLDoc" },
    { HighlightLanguage.CSharp, "C#" },
    { HighlightLanguage.JavaScript, "JavaScript" },
    { HighlightLanguage.HTML, "HTML" },
    { HighlightLanguage.Boo, "Boo" },
    { HighlightLanguage.Coco, "Coco" },
    { HighlightLanguage.CSS, "CSS" },
    { HighlightLanguage.Cpp, "CPP" },
    { HighlightLanguage.Java, "Java" },
    { HighlightLanguage.Patch, "Patch" },
    { HighlightLanguage.PowerShell, "PowerShell" },
    { HighlightLanguage.PHP, "PHP" },
    { HighlightLanguage.TeX, "Tex" },
    { HighlightLanguage.VBNET, "VBNET" },
    { HighlightLanguage.MarkDown, "MarkDown" }
};

            var fileNameSuffix = currentTheme == theme.Light ? "-light" : "-dark";
            var fileNamePrefix = "Resources/" + languageToFileName.GetValueOrDefault(currentLanguage, "XML") + fileNameSuffix + ".xshd";
            LoadSyntax(fileNamePrefix);
        }

        private void LoadSyntax(string filePath)
        {
            // Load the syntax highlighting definition from the xml.xshd file
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    var definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    // Register the syntax highlighting definition with the HighlightingManager
                    HighlightingManager.Instance.RegisterHighlighting("XML", new string[] { ".xml" }, definition);
                }
            }

            // Apply the "XML" syntax highlighting definition to your TextEditor control
            textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML");
        }

        /// <summary>
        /// Changes the context icons to dark mode.
        /// </summary>
        private void ChangeContextIconsToDarkMode()
        {
            contextIcon_Cut.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/cut.png", UriKind.Relative));
            contextIcon_Copy.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/copy.png", UriKind.Relative));
            contextIcon_Paste.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/paste.png", UriKind.Relative));
            contextIcon_SelectAll.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/selectAll.png", UriKind.Relative));
            contextIcon_New.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/add_New.png", UriKind.Relative));
            contextIcon_Open.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/open.png", UriKind.Relative));
            contextIcon_Save.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/save.png", UriKind.Relative));
            contextIcon_SaveAs.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/saveAs.png", UriKind.Relative));
            contextIcon_Print.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/print.png", UriKind.Relative));
            contextIcon_Close.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/exit.png", UriKind.Relative));
            contextIcon_Undo.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/undo.png", UriKind.Relative));
            contextIcon_Redo.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/redo.png", UriKind.Relative));
        }

        /// <summary>
        /// Changes the context icons to light mode.
        /// </summary>
        private void ChangeContextIconsToLightMode()
        {
            contextIcon_Cut.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/cut.png", UriKind.Relative));
            contextIcon_Copy.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/copy.png", UriKind.Relative));
            contextIcon_Paste.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/paste.png", UriKind.Relative));
            contextIcon_SelectAll.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/selectAll.png", UriKind.Relative));
            contextIcon_New.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/add_New.png", UriKind.Relative));
            contextIcon_Open.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/open.png", UriKind.Relative));
            contextIcon_Save.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/save.png", UriKind.Relative));
            contextIcon_SaveAs.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/saveAs.png", UriKind.Relative));
            contextIcon_Print.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/print.png", UriKind.Relative));
            contextIcon_Close.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/exit.png", UriKind.Relative));
            contextIcon_Undo.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/undo.png", UriKind.Relative));
            contextIcon_Redo.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/redo.png", UriKind.Relative));
        }

        /// <summary>
        /// Changes the general UI to light mode.
        /// </summary>
        private void ChangeGeneralUIToLightMode()
        {
            textEditor.Foreground = Brushes.Black;
            textEditor.Background = Brushes.White;
            menu.Style = App.Current.Resources["LightModeStyleMenu"] as System.Windows.Style;
            Application.Current.Resources["Menu.Static.Background"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["Menu.Static.Border"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["Menu.Static.Foreground"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["Menu.Static.Menu.Static.Separator"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["MenuItem.Selected.Background"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["MenuItem.Selected.Foreground"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["MenuItem.Selected.Border"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["MenuItem.Highlight.Border"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["MenuItem.Highlight.Disabled.Background"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["MenuItem.Highlight.Disabled.Border"] = new SolidColorBrush(Colors.Black);
            mainWindowStatusBar.Style = App.Current.Resources["LightModeStyleStatusBar"] as System.Windows.Style;
            LineCharacterPosition.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Changes the XML TreeView to light mode.
        /// </summary>
        private void ChangeXMLTreeViewToLightMode()
        {
            ElementTree.Background = Brushes.White;
            refreshXmlTreeButton.Background = Brushes.White;
            refreshXmlTreeButton.Foreground = Brushes.Black;
            TreeSearchTextBox.Background = Brushes.White;
            TreeSearchTextBox.Foreground = Brushes.Black;
            searchButton.Background = Brushes.White;
            searchButton.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Changes the title bar to light mode.
        /// </summary>
        private void ChangeTitleBarToLightMode()
        {
            TitleBar.Background = Brushes.White;
            Title.Foreground = Brushes.Black;
            MinimiseButton.Foreground = Brushes.Black;
            MaximiseButton.Foreground = Brushes.Black;
            CloseWindowButton.Foreground = Brushes.Black;
            rightClickMenu.Background = Brushes.White;
            rightClickMenu.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Changes the menu icons to light mode.
        /// </summary>
        private void ChangeMenuIconsToLightMode()
        {
            icon_addNew.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/addNew.png", UriKind.Relative));
            icon_clear.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/clear.png", UriKind.Relative));
            icon_open.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/open.png", UriKind.Relative));
            icon_openFolder.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/openFolder.png", UriKind.Relative));
            icon_save.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/save.png", UriKind.Relative));
            icon_saveAs.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/saveAs.png", UriKind.Relative));
            icon_print.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/print.png", UriKind.Relative));
            icon_difference.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/difference.png", UriKind.Relative));
            icon_exit.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/exit.png", UriKind.Relative));
            icon_undo.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/undo.png", UriKind.Relative));
            icon_redo.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/redo.png", UriKind.Relative));
            icon_cut.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/cut.png", UriKind.Relative));
            icon_copyContent.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/copyContent.png", UriKind.Relative));
            icon_duplicateLine.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/duplicateLine.png", UriKind.Relative));
            icon_paste.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/paste.png", UriKind.Relative));
            icon_backspace.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/backspace.png", UriKind.Relative));
            icon_selectAll.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/selectAll.png", UriKind.Relative));
            icon_findReplace.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/findReplace.png", UriKind.Relative));
            icon_dateTimeShort.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/dateTimeShort.png", UriKind.Relative));
            icon_dateTimeLong.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/dateTimeLong.png", UriKind.Relative));
            icon_path.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/path.png", UriKind.Relative));
            icon_copyName.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/copyName.png", UriKind.Relative));
            icon_minus.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/minus.png", UriKind.Relative));
            icon_allUpper.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/allUpper.png", UriKind.Relative));
            icon_lowercase.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/lowercase.png", UriKind.Relative));
            icon_swap.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/swap.png", UriKind.Relative));
            icon_random.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/random.png", UriKind.Relative));
            icon_duplicateLine_1.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/duplicateLine.png", UriKind.Relative));
            icon_removeDuplicateLines.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/removeDuplicateLines.png", UriKind.Relative));
            icon_moveUp.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/moveUp.png", UriKind.Relative));
            icon_moveDown.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/moveDown.png", UriKind.Relative));
            icon_clear_1.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/clear.png", UriKind.Relative));
            icon_lock.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/lock.png", UriKind.Relative));
            icon_fullScreen.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/fullScreen.png", UriKind.Relative));
            icon_minus_1.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/minus.png", UriKind.Relative));
            icon_darkMode.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/darkMode.png", UriKind.Relative));
            icon_manual.Source = new BitmapImage(new Uri("/Windows/Assets/lightMode/manual.png", UriKind.Relative));
        }

        /// <summary>
        /// Changes the general UI to dark mode.
        /// </summary>
        private void ChangeGeneralUIToDarkMode()
        {
            textEditor.Foreground = Brushes.White;
            textEditor.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
            menu.Style = App.Current.Resources["DarkModeStyleMenu"] as System.Windows.Style;
            App.Current.Resources["Menu.Static.Background"] = App.Current.Resources["grayBrush"] as SolidColorBrush;
            App.Current.Resources["Menu.Static.Border"] = App.Current.Resources["grayBrush"] as SolidColorBrush;
            App.Current.Resources["Menu.Static.Foreground"] = new SolidColorBrush(Colors.White);
            App.Current.Resources["Menu.Static.Menu.Static.Separator"] = new SolidColorBrush(Colors.White);
            App.Current.Resources["MenuItem.Selected.Background"] = new SolidColorBrush(Colors.Gray);
            App.Current.Resources["MenuItem.Selected.Border"] = App.Current.Resources["lightBlue"] as SolidColorBrush;
            App.Current.Resources["MenuItem.Highlight.Background"] = App.Current.Resources["lightAqua"] as SolidColorBrush;
            App.Current.Resources["MenuItem.Highlight.Border"] = App.Current.Resources["lightBlue"] as SolidColorBrush;
            App.Current.Resources["MenuItem.Highlight.Disabled.Background"] = new SolidColorBrush(Colors.Transparent);
            App.Current.Resources["MenuItem.Highlight.Disabled.Border"] = new SolidColorBrush(Colors.Transparent);
            mainWindowStatusBar.Style = App.Current.Resources["DarkModeStyleStatusBar"] as System.Windows.Style;
            LineCharacterPosition.Foreground = Brushes.White;
        }

        /// <summary>
        /// Changes the XML TreeView to dark mode.
        /// </summary>
        private void ChangeXmlTreeViewToDarkMode()
        {
            ElementTree.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
            refreshXmlTreeButton.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
            refreshXmlTreeButton.Foreground = Brushes.White;
            TreeSearchTextBox.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
            TreeSearchTextBox.Foreground = Brushes.White;
            searchButton.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
            searchButton.Foreground = Brushes.White;
        }

        /// <summary>
        /// Changes the title bar to dark mode.
        /// </summary>
        private void ChangeTitleBarToDarkMode()
        {
            TitleBar.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
            Title.Foreground = Brushes.White;
            MinimiseButton.Foreground = Brushes.White;
            MaximiseButton.Foreground = Brushes.White;
            CloseWindowButton.Foreground = Brushes.White;
            rightClickMenu.Background = Application.Current.Resources["LightGrayBrush"] as SolidColorBrush;
            rightClickMenu.Foreground = Brushes.White;
        }

        /// <summary>
        /// Changes the menu icons to dark mode.
        /// </summary>
        private void ChangeMenuIconsToDarkMode()
        {
            icon_addNew.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/addNew.png", UriKind.Relative));
            icon_clear.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/clear.png", UriKind.Relative));
            icon_open.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/open.png", UriKind.Relative));
            icon_openFolder.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/openFolder.png", UriKind.Relative));
            icon_save.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/save.png", UriKind.Relative));
            icon_saveAs.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/saveAs.png", UriKind.Relative));
            icon_print.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/print.png", UriKind.Relative));
            icon_difference.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/difference.png", UriKind.Relative));
            icon_exit.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/exit.png", UriKind.Relative));
            icon_undo.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/undo.png", UriKind.Relative));
            icon_redo.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/redo.png", UriKind.Relative));
            icon_cut.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/cut.png", UriKind.Relative));
            icon_copyContent.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/copyContent.png", UriKind.Relative));
            icon_duplicateLine.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/duplicateLine.png", UriKind.Relative));
            icon_paste.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/paste.png", UriKind.Relative));
            icon_backspace.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/backspace.png", UriKind.Relative));
            icon_selectAll.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/selectAll.png", UriKind.Relative));
            icon_findReplace.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/findReplace.png", UriKind.Relative));
            icon_dateTimeShort.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/dateTimeShort.png", UriKind.Relative));
            icon_dateTimeLong.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/dateTimeLong.png", UriKind.Relative));
            icon_path.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/path.png", UriKind.Relative));
            icon_copyName.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/copyName.png", UriKind.Relative));
            icon_minus.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/minus.png", UriKind.Relative));
            icon_allUpper.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/allUpper.png", UriKind.Relative));
            icon_lowercase.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/lowercase.png", UriKind.Relative));
            icon_swap.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/swap.png", UriKind.Relative));
            icon_random.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/random.png", UriKind.Relative));
            icon_duplicateLine_1.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/duplicateLine.png", UriKind.Relative));
            icon_removeDuplicateLines.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/removeDuplicateLines.png", UriKind.Relative));
            icon_moveUp.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/moveUp.png", UriKind.Relative));
            icon_moveDown.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/moveDown.png", UriKind.Relative));
            icon_clear_1.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/clear.png", UriKind.Relative));
            icon_lock.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/lock.png", UriKind.Relative));
            icon_fullScreen.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/fullScreen.png", UriKind.Relative));
            icon_minus_1.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/minus.png", UriKind.Relative));
            icon_darkMode.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/darkMode.png", UriKind.Relative));
            icon_manual.Source = new BitmapImage(new Uri("/Windows/Assets/darkMode/manual.png", UriKind.Relative));
        }

        /// <summary>
        /// Extracts the theme.
        /// </summary>
        /// <returns>The current theme.</returns>
        private theme ExtractTheme()
        {
            string preferencesFile = @"C:/temp/XMLPadPreferences.txt";

            if (File.Exists(preferencesFile))
            {
                string preferences = File.ReadAllText(preferencesFile);

                string[] preferences_arr = preferences.Split(' ');

                Enum.TryParse(preferences_arr[1], out HighlightLanguage currentLanguage);

                if (preferences_arr[0].ToLower().Contains("light"))
                    return theme.Light;
            }

            return theme.Dark;
        }

        /// <summary>
        /// Sets the theme when the program is initialized.
        /// Object(string) firstBoot is passed as part of verification
        /// </summary>
        private void SetThemeFirstTime()
        {
            if (currentTheme == theme.Light)
                SwitchTheme("firstBoot", null);

        }

        /// <summary>
        /// Handles the ViewManual event of the MainMenu_Help control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MainMenu_Help_ViewManual(object sender, RoutedEventArgs e)
        {
            ManualWindow manualWindow = new();
            manualWindow.Show();
        }
    }
}
