namespace XMLpad
{
    using System;
    using System.Windows;
    using ICSharpCode.AvalonEdit;
    using System.Windows.Media;
    using ICSharpCode.AvalonEdit.Folding;
    using ICSharpCode.AvalonEdit.Highlighting;
    using System.IO;

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
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// Used for testing purposes
        /// </summary>
        /// <param name="test">if set to <c>true</c> [test].</param>
        public MainWindow(bool test)
        {
        }
    }
}
