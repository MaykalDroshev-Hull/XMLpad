// Copyright (c) 2023 Maykal Droshev
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sub license, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

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
