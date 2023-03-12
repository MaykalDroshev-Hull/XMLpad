using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using static ICSharpCode.AvalonEdit.Rendering.TextViewWeakEventManager;
using Azure;
using DiffMatchPatch;
using Microsoft.SqlServer.Management.Sdk.Differencing;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Application;
using Window = System.Windows.Window;
using File = System.IO.File;
using MessageBox = System.Windows.MessageBox;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using ScintillaNET;


namespace XMLpad
{
    /// <summary>
    /// Interaction logic for CompareFiles.xaml
    /// </summary>
    public partial class CompareFiles : Window
    {
        public CompareFiles(MainWindow.theme theme)
        {
            InitializeComponent();

            if (theme == MainWindow.theme.Dark)
            {
                compareWindow.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                txtFile1.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                txtFile2.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                btnClose.Background = Application.Current.Resources["grayBrush"] as SolidColorBrush;
                btnClose.Foreground = Brushes.White;
                fileName1.Foreground = Brushes.White;
                fileName2.Foreground = Brushes.White;
                txtFile1.Foreground = Brushes.White;
                txtFile2.Foreground = Brushes.White;

                // Removing the scroll bars as their color cannot be changed
                txtFile1.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                txtFile2.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                txtFile1.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                txtFile2.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
            CompareProcess();
        }
        private void CompareProcess()
        {
            // Open the file dialog to select the two files to compare
            Microsoft.Win32.OpenFileDialog mDlgOpen = new Microsoft.Win32.OpenFileDialog();
            mDlgOpen.Multiselect = true;
            if (mDlgOpen.ShowDialog() == true)
            {
                List<string> filePaths = new List<string>(2);
                CurrentFile currentFile = CurrentFile.GetInstance();
                filePaths.Add(currentFile.FilePath);
                filePaths.Add(mDlgOpen.FileName);
                fileName1.Content = filePaths[0];
                fileName2.Content = filePaths[1];

                if (filePaths.Count == 2)
                {
                    string file1 = File.ReadAllText(filePaths[0]);
                    string file2 = File.ReadAllText(filePaths[1]);
                    Compare(file1, file2);
                }
                else
                {
                    MessageBox.Show("Please add a file to compare.");
                }
            }
        }

        //private void Compare(string file1, string file2)
        //{
        //    try
        //    {
        //        string[] file1Lines = file1.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        //        string[] file2Lines = file2.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        //        string file1Text = string.Join(Environment.NewLine, file1Lines);
        //        txtFile1.Text = file1Text;

        //        for (int lineNumber = 0; lineNumber < file1Lines.Length; lineNumber++)
        //        {
        //            if (file1Lines[lineNumber] != file2Lines[lineNumber])
        //            {
        //                string[] file1Words = file1Lines[lineNumber].Split(' ');
        //                string[] file2Words = file2Lines[lineNumber].Split(' ');
        //                int totalCharCount = file2Words.Select(word => word.Length).Sum() + file2Words.Count() - 1; // the sum of the chars + the num of spaces - 1(we have 2 spaces between 3 words)


        //                for (int word = 0; word < file1Words.Length;)
        //                {
        //                    if (word >= file2Words.Length || file1Words[word] != file2Words[word])
        //                    {
        //                        txtFile2.TextArea.TextView.LineTransformers.Add(new HighlightingColorizer(lineNumber + 1, 0, totalCharCount, Colors.LightGreen));
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        txtFile2.TextArea.TextView.LineTransformers.Add(new HighlightingColorizer(lineNumber + 1, 0, totalCharCount, Colors.PaleVioletRed));
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        string file2Text = string.Join(Environment.NewLine, file2Lines);
        //        txtFile2.Text = file2Text;
        //    }
        //    catch
        //    {
        //        this.Hide();
        //        MessageBox.Show("Cannot compare these files at the moment");
        //    }
        //}

        private void Compare(string file1, string file2)
        {
            diff_match_patch dmp = new diff_match_patch();
            List<Diff> diff = dmp.diff_main(file1, file2);
            dmp.diff_cleanupSemantic(diff);

            // Clear the contents of the TextEditors
            txtFile1.Document.Text = "";
            txtFile2.Document.Text = "";

            int lineNumber1 = 1;
            int lineNumber2 = 1;
            foreach (Diff difference in diff)
            {
                // Set the HighlightingColor of the words based on their corresponding Operation value
                switch (difference.operation)
                {
                    case DiffMatchPatch.Operation.EQUAL:
                        txtFile1.AppendText(difference.text);
                        txtFile2.AppendText(difference.text);
                        lineNumber1 += difference.text.Count(c => c == '\n');
                        lineNumber2 += difference.text.Count(c => c == '\n');
                        break;
                    case DiffMatchPatch.Operation.INSERT:
                        editor.Document.Insert(editor.Document.GetLineByNumber(line).Offset, run);
                        lineNumber2 += difference.text.Count(c => c == '\n');
                        break;
                    case DiffMatchPatch.Operation.DELETE:
                        txtFile1.InsertAtLine(difference.text, lineNumber1, Colors.LightSalmon);
                        lineNumber1 += difference.text.Count(c => c == '\n');
                        break;
                }
            }

            // Highlight words in txtFile1 and txtFile2
            string[] wordsToHighlight = new string[] { "hello", "world" };
            HighlightLineTransformer highlighter1 = new HighlightLineTransformer(wordsToHighlight);
            HighlightLineTransformer highlighter2 = new HighlightLineTransformer(wordsToHighlight);
            txtFile1.TextArea.TextView.LineTransformers.Add(highlighter1);
            txtFile2.TextArea.TextView.LineTransformers.Add(highlighter2);
        }



        private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
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

        private void compareWindow_Closed(object sender, EventArgs e)
        {
            this.Owner.Activate();
        }
        private void chkSyncScrolling_Checked(object sender, RoutedEventArgs e)
        {
            // Attach the ScrollOffsetChanged event handlers to both TextEditors
            txtFile1.TextArea.TextView.ScrollOffsetChanged += txtFile1_ScrollOffsetChanged;
            txtFile2.TextArea.TextView.ScrollOffsetChanged += txtFile2_ScrollOffsetChanged;
        }

        private void chkSyncScrolling_Unchecked(object sender, RoutedEventArgs e)
        {
            // Detach the ScrollOffsetChanged event handlers from both TextEditors
            txtFile1.TextArea.TextView.ScrollOffsetChanged -= txtFile1_ScrollOffsetChanged;
            txtFile2.TextArea.TextView.ScrollOffsetChanged -= txtFile2_ScrollOffsetChanged;
        }

        private void txtFile1_ScrollOffsetChanged(object sender, EventArgs e)
        {
            // Synchronize the vertical scroll offset of txtFile2
            txtFile2.ScrollToVerticalOffset(txtFile1.TextArea.TextView.VerticalOffset);
        }

        private void txtFile2_ScrollOffsetChanged(object sender, EventArgs e)
        {
            // Synchronize the vertical scroll offset of txtFile1
            txtFile1.ScrollToVerticalOffset(txtFile2.TextArea.TextView.VerticalOffset);
        }
    }

}

