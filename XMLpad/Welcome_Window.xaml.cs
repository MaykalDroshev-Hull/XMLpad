﻿using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using XMLpad.Models;

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for Welcome_Window.xaml
    /// </summary>
    public partial class Welcome_Window : Window
    {
        /// <summary>
        /// The timer
        /// </summary>
        readonly DispatcherTimer timer = new DispatcherTimer();

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
                FileStream fileStream = new FileStream("C:/temp/XMLpadRecentFiles.txt", FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fileStream);
                while (reader.Peek() >= 0)
                {
                    string entry = reader.ReadLine();
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
        /// Handles the tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timer_tick(object sender, EventArgs e)
        {
            timer.Stop();
            media.Visibility = Visibility.Hidden;
            welcomeScreenControlsCanvas.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Environment.Exit(0);

        /// <summary>
        /// Loadings this instance.
        /// </summary>
        void Loading()
        {
            timer.Tick += timer_tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        /// <summary>
        /// Handles the Click event of the createNewButon control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void createNewButon_Click(object sender, RoutedEventArgs e)
        {
            // Create a save file dialog
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            // Set the filter for the file extension

           saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";

            // Set the default file extension
            saveFileDialog.DefaultExt = ".xml";

            saveFileDialog.FileName = fileNameTextBox.Text;

            // Set the initial directory
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Show the dialog
            if (saveFileDialog.ShowDialog() == true)
            {
                // Create the new file
                FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fileStream);
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
                createNewButon_Click(sender, e);
            }
        }

        private void AddFileToRecentList(string pFileName)
        {
            // Add the file to the list of files
            string recentFilesPath = "C:/temp/XMLpadRecentFiles.txt";
            using (FileStream fileStream = new FileStream(recentFilesPath, FileMode.Append, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.WriteLine(pFileName);
            }
        }

        private void SetFileName(string pFileName)
        {
            CurrentFile currentFile = CurrentFile.getInstance();
            currentFile.FileName = System.IO.Path.GetFileName(pFileName);
            currentFile.FilePath = pFileName;
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            string hex = "#FFFFFF";
            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
            System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(_color.A, _color.R, _color.G, _color.B);
            CloseButton.Background = new SolidColorBrush(newColor);
        }

        private void recentFilesListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = recentFilesListBox.SelectedIndex;
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                SetFileName(recentFilesListBox.SelectedItem.ToString());
                this.Close();
            }
        }
    }
}
