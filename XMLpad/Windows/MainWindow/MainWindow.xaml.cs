namespace XMLpad
{
    using System.Windows;
    using ICSharpCode.AvalonEdit.Folding;

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
            Welcome_Window welcome_Window = new();
            if (welcome_Window.ShowDialog() == false)
            {
                InitializeComponent();
                GenerateInitializationText();
                textEditor.Focus();
                mFoldingManager = FoldingManager.Install(textEditor.TextArea);
                textEditor.TextArea.TextEntering += TextEditor_TextArea_TextEntering;
                textEditor.TextArea.TextEntered += TextEditor_TextArea_TextEntered;
                GatherCompletionString();
                defaultFontSize = textEditor.FontSize;
                var backgroundRenderer = new HighlightCurrentLineBackgroundRenderer(textEditor);
                textEditor.TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Used for testing purposes
        /// </summary>
        /// <param name="test">if set to <c>true</c> [test].</param>
        public MainWindow(bool test)
        {
        }
    }
}
