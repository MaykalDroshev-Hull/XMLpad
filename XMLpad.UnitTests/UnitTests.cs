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

namespace XMLpad.UnitTests
{
    using ICSharpCode.AvalonEdit;
    using System.Windows;

    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// Tests that Inverting case works when the input is all uppercase.
        /// </summary>
        [TestMethod]
        public void Test_InvertCase_AllUpperCase()
        {
            string testInput = "TEST";
            string expectedText = "test";
            string testOutput = MainWindow.InvertCase(testInput);
            Assert.AreEqual(expectedText, testOutput, "The output string does not match the expected string when case is inverted.");
        }

        /// <summary>
        /// Tests that Inverting case works when the input is all lowercase.
        /// </summary>
        [TestMethod]
        public void Test_InvertCase_AllLowerCase()
        {
            string testInput = "test";
            string expectedText = "TEST";
            string testOutput = MainWindow.InvertCase(testInput);
            Assert.AreEqual(expectedText, testOutput, "The output string does not match the expected string when case is inverted.");
        }

        /// <summary>
        /// Tests that Inverting case works when the input is random cased.
        /// </summary>
        [TestMethod]
        public void Test_InvertCase_RandomCase()
        {
            string testInput = "tEsT";
            string expectedText = "TeSt";
            string testOutput = MainWindow.InvertCase(testInput);
            Assert.AreEqual(expectedText, testOutput, "The output string does not match the expected string when case is inverted.");
        }

        /// <summary>
        /// Mains the menu convert case random case.
        /// </summary>
        [TestMethod]
        public void Test_RandomCase()
        {
            string testInput = "tEsT";
            string testOutput = MainWindow.RandomiseCase(testInput);
            Assert.AreNotEqual(testInput, testOutput, StringComparer.Ordinal, "The output string should not match the casing of the input string");
        }

        /// <summary>
        /// Tests that duplicate lines are removed from a string.
        /// </summary>
        [TestMethod]
        public void Test_RemoveDuplicateLines()
        {
            string testInput = "The quick brown fox\r\njumps over the lazy dog\r\nThe quick brown fox\r\njumps over the lazy dog";
            string expectedOutput = "The quick brown fox\r\njumps over the lazy dog";
            string actualOutput = MainWindow.RemoveDuplicateLinesFromString(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should remove duplicate lines from the input string.");
        }

        /// <summary>
        /// Tests that non-consecutive duplicate lines are NOT removed from a string.
        /// </summary>
        [TestMethod]
        public void Test_RemoveConsecutiveDuplicateLines_NonConsecutive()
        {
            string testInput = "The quick brown fox jumps over the lazy dog\r\nThe quick the lazy dog\r\nThe quick brown fox jumps over the lazy dog";
            string expectedOutput = "The quick brown fox jumps over the lazy dog\r\nThe quick the lazy dog\r\nThe quick brown fox jumps over the lazy dog";
            string actualOutput = MainWindow.RemoveConsecutiveDuplicateLinesFromString(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not remove duplicate lines from the input string, because they are not consecutive.");
        }

        /// <summary>
        /// Tests that consecutive duplicate lines are removed from a string.
        /// </summary>
        [TestMethod]
        public void Test_RemoveConsecutiveDuplicateLines_Consecutive()
        {
            string testInput = "The quick brown fox jumps over the lazy dog\r\nThe quick brown fox jumps over the lazy dog\r\nThe quick the lazy dog\r\nThe quick brown fox jumps over the lazy dog\r\nThe quick brown fox jumps over the lazy dog";
            string expectedOutput = "The quick brown fox jumps over the lazy dog\r\nThe quick the lazy dog\r\nThe quick brown fox jumps over the lazy dog";
            string actualOutput = MainWindow.RemoveConsecutiveDuplicateLinesFromString(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should remove duplicate lines from the input string, because they are consecutive.");
        }

        /// <summary>
        /// Tests that empty lines are removed from a string
        /// </summary>
        [TestMethod]
        public void Test_RemoveEmptyLines()
        {
            string testInput = "Test \r\n\r\nTest";
            string expectedOutput = "Test \r\nTest";
            string actualOutput = MainWindow.RemoveEmtyLinesFromString(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should remove empty lines from the input string");
        }

        /// <summary>
        /// Tests that non-empty lines are not removed from a string
        /// </summary>
        [TestMethod]
        public void Test_RemoveEmptyLines_NonEmptyLines()
        {
            string testInput = "Test \r\n  \r\nTest";
            string expectedOutput = "Test \r\n  \r\nTest";
            string actualOutput = MainWindow.RemoveEmtyLinesFromString(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should remove the lines from the input string as they have characters on them");
        }

        /// <summary>
        /// Tests that trailing spaces are removed from a string
        /// </summary>
        [TestMethod]
        public void Test_TrimTrailingSpacesFromString()
        {
            // Input string with trailing spaces
            string testInput = "The quick brown fox  \r\njumps over the lazy dog   \r\n";

            // Expected output with trailing spaces removed
            string expectedOutput = "The quick brown fox\r\njumps over the lazy dog\r\n";

            // Call the method to get the actual output
            string actualOutput = MainWindow.TrimTrailingSpacesFromString(testInput);

            // Assert that the actual output matches the expected output
            Assert.AreEqual(expectedOutput, actualOutput, "The method should trim trailing spaces from each line in the input string.");
        }

        [TestMethod]
        public void Test_RemoveLeadingSpacesFromString()
        {
            // Input string with leading spaces
            string testInput = "    The quick brown fox\r\n    jumps over the lazy dog\r\n";

            // Expected output with leading spaces removed
            string expectedOutput = "The quick brown fox\r\njumps over the lazy dog\r\n";

            // Call the method to get the actual output
            string actualOutput = MainWindow.RemoveLeadingSpacesFromString(testInput);

            // Assert that the actual output matches the expected output
            Assert.AreEqual(expectedOutput, actualOutput, "The method should remove leading spaces from each line in the input string.");
        }

        /// <summary>
        /// Tests that leading spaces are converted to tabs in a string.
        /// </summary>
        [TestMethod]
        public void Test_ConvertSpacesToTabsLeading()
        {
            string testInput = "    line1\r\n        line2\r\n            line3\r\n    line4   \r\n\tline5\r\nline6";
            string expectedOutput = "\tline1\r\n\t\tline2\r\n\t\t\tline3\r\n\tline4   \r\n\tline5\r\nline6";
            string actualOutput = MainWindow.ConvertSpacesToTabsLeading(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs in the beginning of each line according to the set tab size.");

            testInput = "    line1";
            expectedOutput = "\tline1";
            actualOutput = MainWindow.ConvertSpacesToTabsLeading(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs in the beginning of the line according to the set tab size.");

            testInput = "line1";
            expectedOutput = "line1";
            actualOutput = MainWindow.ConvertSpacesToTabsLeading(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert spaces to tabs if there are no spaces at the beginning of the line.");

            testInput = "    line1  ";
            expectedOutput = "\tline1  ";
            actualOutput = MainWindow.ConvertSpacesToTabsLeading(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs in the beginning of the line according to the set tab size and ignore trailing spaces.");

            testInput = "line1  ";
            expectedOutput = "line1  ";
            actualOutput = MainWindow.ConvertSpacesToTabsLeading(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert spaces to tabs if there are no spaces at the beginning of the line.");

            testInput = "line1   line2";
            expectedOutput = "line1   line2";
            actualOutput = MainWindow.ConvertSpacesToTabsLeading(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert spaces to tabs in the middle of the line.");
        }

        /// <summary>
        /// Tests that trailing spaces are converted to tabs in a string.
        /// </summary>
        [TestMethod]
        public void Test_ConvertSpacesToTabsTrailing()
        {
            string testInput = "line1    \r\nline2    \r\n    line3    \r\nline4";
            string expectedOutput = "line1\t\r\nline2\t\r\n    line3\t\r\nline4";
            string actualOutput = MainWindow.ConvertSpacesToTabsTrailing(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert trailing spaces to tabs according to the set tab size.");

            testInput = "line1    \r\nline2";
            expectedOutput = "line1\t\r\nline2";
            actualOutput = MainWindow.ConvertSpacesToTabsTrailing(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert trailing spaces to tabs according to the set tab size.");

            testInput = "line1  ";
            expectedOutput = "line1  ";
            actualOutput = MainWindow.ConvertSpacesToTabsTrailing(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should ignore spaces at the end of the last line.");

            testInput = "line1";
            expectedOutput = "line1";
            actualOutput = MainWindow.ConvertSpacesToTabsTrailing(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert spaces to tabs if there are no trailing spaces.");

            testInput = "line1    line2";
            expectedOutput = "line1    line2";
            actualOutput = MainWindow.ConvertSpacesToTabsTrailing(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert spaces to tabs in the middle of the line.");
        }

        /// <summary>
        /// Tests that tabs are converted to spaces in a string.
        /// </summary>
        [TestMethod]
        public void Test_ConvertTabsToSpaces()
        {
            string testInput = "line1\tline2\r\n\t\tline3";
            string expectedOutput = "line1    line2\r\n        line3";
            string actualOutput = MainWindow.ConvertTabsToSpaces(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert tabs to spaces according to the set tab size.");

            testInput = "\tline1";
            expectedOutput = "    line1";
            actualOutput = MainWindow.ConvertTabsToSpaces(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert tabs to spaces according to the set tab size.");

            testInput = "line1";
            expectedOutput = "line1";
            actualOutput = MainWindow.ConvertTabsToSpaces(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert tabs to spaces if there are no tabs in the input.");

            testInput = "line1\t\t";
            expectedOutput = "line1        ";
            actualOutput = MainWindow.ConvertTabsToSpaces(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert tabs to spaces according to the set tab size and ignore trailing tabs.");
        }

        /// <summary>
        /// Tests that spaces are converted to tabs in a string.
        /// </summary>
        [TestMethod]
        public void Test_ConvertSpacesToTabs()
        {
            string testInput = "    line1\r\n        line2\r\n            line3\r\n    line4   \r\n\tline5\r\nline6";
            string expectedOutput = "\tline1\r\n\t\tline2\r\n\t\t\tline3\r\n\tline4   \r\n\tline5\r\nline6";
            string actualOutput = MainWindow.ConvertSpacesToTabs(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs according to the set tab size.");

            testInput = "    line1";
            expectedOutput = "\tline1";
            actualOutput = MainWindow.ConvertSpacesToTabs(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs according to the set tab size.");

            testInput = "line1";
            expectedOutput = "line1";
            actualOutput = MainWindow.ConvertSpacesToTabs(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should not convert spaces to tabs if there are no spaces in the line.");

            testInput = "    line1    ";
            expectedOutput = "\tline1\t";
            actualOutput = MainWindow.ConvertSpacesToTabs(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs according to the set tab size.");

            testInput = "line1    ";
            expectedOutput = "line1\t";
            actualOutput = MainWindow.ConvertSpacesToTabs(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs.");

            testInput = "line1    line2";
            expectedOutput = "line1\tline2";
            actualOutput = MainWindow.ConvertSpacesToTabs(testInput);
            Assert.AreEqual(expectedOutput, actualOutput, "The method should convert spaces to tabs in the middle of the line.");
        }

    }
}