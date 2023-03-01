namespace XMLpad
{
    using System.Windows;
    using ICSharpCode.AvalonEdit.Folding;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Definition logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            currentTheme = ExtractTheme();
            Welcome_Window welcome_Window = new(currentTheme);
            if (welcome_Window.ShowDialog() == false)
            {
                InitializeComponent();
                GenerateInitializationText();
                SetThemeFirstTime();
                textEditor.Focus();
                mFoldingManager = FoldingManager.Install(textEditor.TextArea);
                textEditor.TextArea.TextEntering += TextEditor_TextArea_TextEntering;
                textEditor.TextArea.TextEntered += TextEditor_TextArea_TextEntered;
                GatherCompletionString();
                defaultFontSize = textEditor.FontSize;

                // add current line highlight
                var backgroundRenderer = new HighlightCurrentLineBackgroundRenderer(textEditor, currentTheme);
                textEditor.TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);
            }
        }

        /// <summary>
        /// Handles the Drop event of the textEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void textEditor_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileInfos = ((string[])e.Data.GetData(DataFormats.FileDrop)).Select(f => new FileInfo(f));
                foreach (var fileInfo in fileInfos)
                {
                    if (fileInfo.Extension == ".xml" || fileInfo.Extension == ".cs" || fileInfo.Extension == ".txt")
                    {
                        // Save the current file first before opening the other one
                        SaveCurrentFile();

                        // Get the file name
                        string fileName = fileInfo.Name;
                        mCurrentFile.FileName = fileName;
                        mCurrentFile.FilePath = fileInfo.FullName;
                        UpdateTitle();

                        // Load the file contents into the TextEditor
                        using (var reader = new StreamReader(fileInfo.FullName))
                        {
                            textEditor.Text = reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
