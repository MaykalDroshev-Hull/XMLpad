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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // TextEditor is an AvalonEdit.TextEditor
            TextEditor edit = new TextEditor();
            XmlReader reader = XmlReader.Create("..\\net6.0-windows\\Resources\\XML.xshd");
            edit.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);

            InitializeComponent();

        }
    }
}
