namespace XMLpad
{
    using ICSharpCode.AvalonEdit.CodeCompletion;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Folding;
    using ICSharpCode.AvalonEdit;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The member variables for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
        private string mFilename;
        private static bool isFullScreen = false;
        private readonly Microsoft.Win32.OpenFileDialog mDlgOpen = new();
        private readonly Microsoft.Win32.SaveFileDialog mDlgSave = new();
        private PrintDialog mDlgPrint;
        private CurrentFile mCurrentFile;
        private readonly List<string> mCompletionStrings = new();
        private readonly List<string> mOpenedStrings = new();
        private FoldingManager mFoldingManager = new(new TextDocument());
        private CompletionWindow? mCompletionWindow;
        private static readonly int mTabSpacesCount = 4; // TODO: Change to be dynamic
        private string mLoadedFile;
        public TextEditor testTextEditor;

        private double defaultFontSize;
        private enum tabSelection
        {
            Tabs,
            Spaces
        };

        public enum theme
        {
            Dark,
            Light
        };

        private enum HighlightLanguage
        {
            XmlDoc,
            CSharp,
            JavaScript,
            HTML,
            Boo,
            Coco,
            CSS,
            Cpp,
            Java,
            PowerShell,
            PHP,
            VBNET,
            XML,
            MarkDown
        }

        private const theme darkTheme = theme.Dark;
        public static theme currentTheme = darkTheme;
        private static HighlightLanguage currentLanguage;
        private static readonly tabSelection currentTabSelection = tabSelection.Tabs;
        private readonly string tabCharacters = currentTabSelection == tabSelection.Spaces ? new string(' ', mTabSpacesCount) : "\t";
    }
}
