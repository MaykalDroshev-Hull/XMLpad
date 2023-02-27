﻿namespace XMLpad
{
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.IO;

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

        private void SwitchTheme(object sender, RoutedEventArgs e)
        {
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

                currentTheme = theme.Light;
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

                currentTheme = theme.Dark;
            }

            var backgroundRenderer = new HighlightCurrentLineBackgroundRenderer(textEditor, currentTheme);
            textEditor.TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);

            UpdateStatusBar(pAppend: true, pValue: $"Theme changed!");
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
        /// Sets the theme when the program is initialised.
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
