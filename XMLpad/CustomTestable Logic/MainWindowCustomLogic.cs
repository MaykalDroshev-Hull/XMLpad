using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLpad
{
    public partial class MainWindow
    {
        /// <summary>
        /// Indents the selected text.
        /// </summary>
        /// <param name="selectedText">The selected text.</param>
        /// <param name="tabCharacters">The tab characters.</param>
        /// <returns></returns>
        public static string IndentSelectedText(string selectedText, string tabCharacters)
        {
            return selectedText.Replace("\n", "\n" + tabCharacters);
        }
    }
}
