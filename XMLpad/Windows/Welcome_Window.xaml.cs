namespace XMLpad
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using XMLpad.Models;

    /// <summary>
    /// Interaction logic for Welcome_Window.xaml
    /// </summary>
    public partial class Welcome_Window : Window
    {
        /// <summary>
        /// The timer
        /// </summary>
        readonly DispatcherTimer timer = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Welcome_Window"/> class.
        /// </summary>
        public Welcome_Window()
        {
            InitializeComponent();
            //media.Source = new Uri(Environment.CurrentDirectory + @"\Loading_Screen.gif");
            //Loading();
            FillRecentOpenDocument();
        }

        /// <summary>
        /// Fills the recent open document List Box.
        /// </summary>
        private void FillRecentOpenDocument()
        {
            try
            {
                FileStream fileStream = new("C:/temp/XMLpadRecentFiles.txt", FileMode.Open, FileAccess.Read);
                StreamReader reader = new(fileStream);
                while (reader.Peek() >= 0)
                {
                    string? entry = reader.ReadLine();
                    if (entry != string.Empty)
                        recentFilesListBox.Items.Add(entry);
                }
                reader.Close();
            }
            catch (IOException) { }
        }

        /// <summary>
        /// Handles the MediaEnded event of the MediaElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = new TimeSpan(0, 0, 0);
            media.Play();
        }



        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);



        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        /// <summary>
        /// Handles the Click event of the createNewButon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void CreateNewButon_Click(object sender, RoutedEventArgs e)
        {
            // Create a save file dialog
            Microsoft.Win32.SaveFileDialog saveFileDialog = new()
            {
                // Set the filter for the file extension

                Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*",

                // Set the default file extension
                DefaultExt = ".xml",

                FileName = fileNameTextBox.Text,

                // Set the initial directory
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            // Show the dialog
            if (saveFileDialog.ShowDialog() == true)
            {
                // Create the new file
                FileStream fileStream = new(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new(fileStream);
                writer.Flush();
                writer.Close();

                // Add the file to the list of files
                AddFileToRecentList(saveFileDialog.FileName);
                SetFileName(saveFileDialog.FileName);
                this.Close();
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the MyTextBox control.
        /// Fired when the user has focused the text box to enter a name and has pressed ENTER
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void MyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CreateNewButon_Click(sender, e);
            }
        }


        /// <summary>
        /// AddFileToRecentList method adds the file to the list of recent files
        /// </summary>
        /// <param name="pFileName">Name of the p file.</param>
        private static void AddFileToRecentList(string pFileName)
        {
            // Declare recent files path
            string recentFilesPath = "C:/temp/XMLpadRecentFiles.txt";

            // Append the file name to the recent files text file
            using FileStream fileStream = new(recentFilesPath, FileMode.Append, FileAccess.Write);
            using StreamWriter writer = new(fileStream);
            writer.WriteLine(pFileName);
        }

        /// <summary>
        /// Sets the current file name and file path.
        /// </summary>
        /// <param name="pFileName">Name of the p file.</param>
        private static void SetFileName(string pFileName)
        {
            // Get the instance of CurrentFile class
            CurrentFile currentFile = CurrentFile.GetInstance();
            // Set the file name
            currentFile.FileName = System.IO.Path.GetFileName(pFileName);
            // Set the file path
            currentFile.FilePath = pFileName;
        }

        /// <summary>
        /// Цhanges the background color of CloseButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Define the hex color code for white
            string hex = "#FFFFFF";

            // Convert the hex color code to System.Drawing.Color
            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

            // Convert the System.Drawing.Color to System.Windows.Media.Color
            System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(_color.A, _color.R, _color.G, _color.B);

            // Set the background color of CloseButton to newColor
            CloseButton.Background = new SolidColorBrush(newColor);
        }

        /// <summary>
        /// Event handler for recentFilesListBox MouseUp event
        /// Selects a file from the recentFilesListBox and sets the file name
        /// Closes the window if a file is selected
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void RecentFilesListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = recentFilesListBox.SelectedIndex;
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                SetFileName(recentFilesListBox.SelectedItem.ToString());
                SetFileNameOnTopOfTheList(recentFilesListBox.SelectedItem.ToString());
                Close();
            }
        }

        /// <summary>
        /// Sets the file name on top of the list.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void SetFileNameOnTopOfTheList(string? fileName)
        {
            // Read the text file into a list
            List<string> fileList = ReadFileList("C:/temp/XMLpadRecentFiles.txt");

            string selectedFile = fileName;

            // Remove the selected file from the list
            fileList.Remove(selectedFile);

            // Add the selected file to the top of the list
            fileList.Insert(0, selectedFile+"\r\n");

            // Write the updated list to the text file
            WriteFileList("C:/temp/XMLpadRecentFiles.txt", fileList);
        }

        static List<string> ReadFileList(string fileName)
        {
            List<string> fileList = new List<string>();

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    fileList.Add(line);
                }
            }

            return fileList;
        }

        static void WriteFileList(string fileName, List<string> fileList)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (string file in fileList)
                {
                    writer.WriteLine(file);
                }
            }
        }

        #region Obsolete
        /// <summary>
        /// Handles the tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            media.Visibility = Visibility.Hidden;
            welcomeScreenControlsCanvas.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Loadings this instance.
        /// </summary>
        private void Loading()
        {
            timer.Tick += Timer_tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        #endregion
    }
}
