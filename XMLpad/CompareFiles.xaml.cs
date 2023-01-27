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
            string[] file1Lines = File.ReadAllLines(file1);
            string[] file2Lines = File.ReadAllLines(file2);

            string file1Text = string.Join(Environment.NewLine, file1Lines);
            txtFile1.Text = file1Text;

            for (int i = 0; i < file1Lines.Length; i++)
            {
                if (file1Lines[i] != file2Lines[i])
                {
                    txtFile2.TextArea.TextView.LineTransformers.Add(new HighlightingColorizer(i, Colors.Red));
                }
                else
                {
                    txtFile2.TextArea.TextView.LineTransformers.Add(new HighlightingColorizer(i, Colors.Green));
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

