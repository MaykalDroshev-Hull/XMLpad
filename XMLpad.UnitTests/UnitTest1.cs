using ICSharpCode.AvalonEdit;
using System.Windows;

namespace XMLpad.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndentSelectedText_SelectedTextIsIndented()
        {
            // Arrange
            string selectedText = "\nLine 2";
            string text = $"\nLine 1{ selectedText }\nLine 3";
            string expectedText = "\nLine 1\n    Line 2\nLine 3";


            // Act
            selectedText = MainWindow.IndentSelectedText(selectedText, "    ");

            text = $"\nLine 1\n{selectedText}Line 3";

            // Assert
            Assert.AreEqual(expectedText, text);
        }
    }
}