namespace XMLpad
{
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Imaging;
    using System;
    using Window = System.Windows.Window;
    using System.Threading.Tasks;

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

        /// <summary>
        /// Switches the theme.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SwitchTheme(object sender, RoutedEventArgs e)
        {
            // Show loading popup
            var loadingPopup = new LoadingPopUp();
            loadingPopup.Show();

            // Execute the method asynchronously on a separate thread
            await Dispatcher.Invoke(async () =>
            {
                await Task.Run(() =>
                {
                    #region UIChanges
                    if (currentTheme == theme.Dark || (sender.Equals("firstBoot") && currentTheme == theme.Light))
                    {
                        // Switch to Light Theme -> General controls
                        textEditor.Foreground = Brushes.Black;
                        textEditor.Background = Brushes.White;
                        menu.Style = App.Current.Resources["LightModeStyleMenu"] as System.Windows.Style;
                        App.Current.Resources["Menu.Static.Background"] = new SolidColorBrush(Colors.White);
                        App.Current.Resources["Menu.Static.Border"] = new SolidColorBrush(Colors.White);
                        App.Current.Resources["Menu.Static.Foreground"] = new SolidColorBrush(Colors.Black);
                        App.Current.Resources["Menu.Static.Menu.Static.Separator"] = new SolidColorBrush(Colors.Black);
                        App.Current.Resources["MenuItem.Selected.Background"] = new SolidColorBrush(Colors.White);
                        App.Current.Resources["MenuItem.Selected.Foreground"] = new SolidColorBrush(Colors.Black);
                        App.Current.Resources["MenuItem.Selected.Border"] = new SolidColorBrush(Colors.White);
                        App.Current.Resources["MenuItem.Highlight.Background"] = new SolidColorBrush(Colors.White);
                        App.Current.Resources["MenuItem.Highlight.Border"] = new SolidColorBrush(Colors.White);
                        App.Current.Resources["MenuItem.Highlight.Disabled.Background"] = new SolidColorBrush(Colors.Black);
                        App.Current.Resources["MenuItem.Highlight.Disabled.Border"] = new SolidColorBrush(Colors.Black);
                        mainWindowStatusBar.Style = App.Current.Resources["LightModeStyleStatusBar"] as System.Windows.Style;
                        LineCharacterPosition.Foreground = Brushes.Black;

                        // XML Tree view

                        ElementTree.Background = Brushes.White;
                        refreshXmlTreeButton.Background = Brushes.White;
                        refreshXmlTreeButton.Foreground = Brushes.Black;
                        refreshXmlTreeButton_Click(sender, e);
                        TreeSearchTextBox.Background = Brushes.White;
                        TreeSearchTextBox.Foreground = Brushes.Black;
                        searchButton.Background = Brushes.White;
                        searchButton.Foreground = Brushes.Black;

                        // Title bar
                        TitleBar.Background = Brushes.White;
                        Title.Foreground = Brushes.Black;
                        MinimiseButton.Foreground = Brushes.Black;
                        MaximiseButton.Foreground = Brushes.Black;
                        CloseWindowButton.Foreground = Brushes.Black;
                        rightClickMenu.Background = Brushes.White;
                        rightClickMenu.Foreground = Brushes.Black;

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

                        currentTheme = theme.Light;
                        TryLoadXML();
                    }
                    else
                    {
                        // Switch to dark theme-> General controls
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

                        // XML tree view
                        ElementTree.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
                        refreshXmlTreeButton.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
                        refreshXmlTreeButton.Foreground = Brushes.White;
                        refreshXmlTreeButton_Click(sender, e);
                        TreeSearchTextBox.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
                        TreeSearchTextBox.Foreground = Brushes.White;
                        searchButton.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
                        searchButton.Foreground = Brushes.White;

                        // title bar
                        TitleBar.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;
                        Title.Foreground = Brushes.White;
                        MinimiseButton.Foreground = Brushes.White;
                        MaximiseButton.Foreground = Brushes.White;
                        CloseWindowButton.Foreground = Brushes.White;
                        rightClickMenu.Background = Application.Current.Resources["LightGrayBrush"] as SolidColorBrush;
                        rightClickMenu.Foreground = Brushes.White;

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

                        currentTheme = theme.Dark;
                        TryLoadXML();
                    }

                    var backgroundRenderer = new HighlightCurrentLineBackgroundRenderer(textEditor, currentTheme);
                    textEditor.TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);

                    UpdateStatusBar(pAppend: true, pValue: $"Theme changed!");
                    // Hide loading popup when the method completes successfully
                    #endregion
                });
            });

            await loadingPopup.HideAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Extracts the theme.
        /// </summary>
        /// <returns></returns>
        private theme ExtractTheme()
        {
            string preferencesFile = @"C:/temp/XMLPadPreferences.txt";

            if (File.Exists(preferencesFile))
            {
                string preferences = File.ReadAllText(preferencesFile);

                if (preferences.ToLower().Contains("light"))
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
