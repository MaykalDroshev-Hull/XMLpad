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
using XMLpad.Models;

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for CompareFiles.xaml
    /// </summary>
    public partial class CompareFiles : Window
    {
        public CompareFiles()
        {
            InitializeComponent();
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
                CurrentFile currentFile = CurrentFile.getInstance();
                filePaths.Add(currentFile.FilePath);
                filePaths.Add(mDlgOpen.FileName);
                label1.Content = filePaths[0];
                label2.Content = filePaths[1];

                if (filePaths.Count == 2)
                {
                    string file1 = File.ReadAllText(filePaths[0]);
                    string file2 = File.ReadAllText(filePaths[1]);
                    Compare(file1, file2);
                }
                else
                {
                    MessageBox.Show("Please a file to compare.");
                }
            }
        }

        private void Compare(string file1, string file2)
        {
            string[] file1Lines = file1.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string[] file2Lines = file2.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string file1Text = string.Join(Environment.NewLine, file1Lines);
            txtFile1.Text = file1Text;

            for (int lineNumber = 0; lineNumber < file1Lines.Length; lineNumber++)
            {
                if (file1Lines[lineNumber] != file2Lines[lineNumber])
                {
                    string[] file1Words = file1Lines[lineNumber].Split(' ');
                    string[] file2Words = file2Lines[lineNumber].Split(' ');
                    int totalCharCount = file2Words.Select(word => word.Length).Sum() + file2Words.Count() - 1; // the sum of the chars + the num of spaces - 1(we have 2 spaces between 3 words)


                    for (int word = 0; word < file1Words.Length;)
                    {
                        if (word >= file2Words.Length || file1Words[word] != file2Words[word])
                        {
                            txtFile2.TextArea.TextView.LineTransformers.Add(new HighlightingColorizer(lineNumber + 1, 0, totalCharCount, Colors.LightGreen));
                            break;
                        }
                        else
                        {
                            txtFile2.TextArea.TextView.LineTransformers.Add(new HighlightingColorizer(lineNumber + 1, 0, totalCharCount, Colors.PaleVioletRed));
                            break;
                        }
                    }
                }
            }
            string file2Text = string.Join(Environment.NewLine, file2Lines);
            txtFile2.Text = file2Text;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

