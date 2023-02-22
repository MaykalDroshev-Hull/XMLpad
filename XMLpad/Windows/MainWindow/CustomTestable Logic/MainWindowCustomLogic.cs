using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        /// <summary>
        /// Trims the trailing spaces from string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string TrimTrailingSpacesFromString(string input)
        {
            string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Removes the leading spaces from string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveLeadingSpacesFromString(string input)
        {
            return string.Join("\n", input.Split('\n').Select(x => x.TrimStart()));
        }

        /// <summary>
        /// Converts the tabs to spaces.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ConvertTabsToSpaces(string input)
        {
            return input.Replace("\t", new string(' ', mTabSpacesCount));
        }

        /// <summary>
        /// Converts the spaces to tabs leading.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ConvertSpacesToTabsLeading(string input)
        {
            StringBuilder result = new StringBuilder();
            int spaces = 0;
            foreach (char c in input)
            {
                if (c == ' ')
                {
                    spaces++;
                    if (spaces == mTabSpacesCount)
                    {
                        // Replace mTabSpacesCount spaces with a single tab character
                        result.Append('\t');
                        spaces = 0;
                    }
                }
                else
                {
                    if (spaces > 0)
                    {
                        // Append remaining spaces as-is
                        result.Append(new string(' ', spaces));
                        spaces = 0;
                    }
                    result.Append(c);
                }
            }
            // Append any remaining spaces at the end of the string
            if (spaces > 0)
            {
                result.Append(new string(' ', spaces));
            }
            return result.ToString();
        }

        /// <summary>
        /// Converts the spaces to tabs trailing.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ConvertSpacesToTabsTrailing(string input)
        {
            string[] lines = input.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int spacesCount = line.Length - line.TrimEnd(' ').Length;
                int tabCount = spacesCount / mTabSpacesCount;
                if (tabCount > 0)
                {
                    lines[i] = line.TrimEnd() + new string('\t', tabCount);
                }
            }
            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Converts the spaces to tabs.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ConvertSpacesToTabs(string input)
        {
            string[] lines = input.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(new string(' ', mTabSpacesCount), "\t");
            }
            return string.Join("\n", lines);
        }

        /// <summary>
        /// Inverts the case.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string InvertCase(string input)
        {
            return new string(
                input.Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                    char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
        }

        /// <summary>
        /// Randomises the case.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RandomiseCase(string input)
        {
            Random randomizer = new();
            IEnumerable<char> final =
                input.Select(x => randomizer.Next() % 2 == 0 ?
                (char.IsUpper(x) ? x.ToString().ToLower().First() : x.ToString().ToUpper().First()) : x);
            return new(final.ToArray());
        }

        /// <summary>
        /// Removes the duplicate lines from string.
        /// This method first splits the input string into an array of lines using the
        /// Environment.NewLine separator. It then uses a HashSet to keep track of unique
        /// lines, and iterates through the array of lines. If a line is not already in the
        /// HashSet, it is added, and appended to the result StringBuilder. The final result
        /// is returned as a string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string RemoveDuplicateLinesFromString(string input)
        {
            string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            // Collection of unique elements
            HashSet<string> uniqueLines = new();
            StringBuilder result = new();

            foreach (string line in lines)
            {
                if (uniqueLines.Add(line))
                {
                    result.AppendLine(line);
                }
            }

            string returnValue = result.ToString();
            if (returnValue.EndsWith("\r\n"))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 2);
            }
            return returnValue;
        }

        /// <summary>
        /// Removes the consecutive duplicate lines from string.
        /// This method first splits the input string into an array of strings, where each element in
        /// the array represents a line of the input. Then, it iterates through the array of lines,
        /// comparing each line with the previous line. If a line is different from the previous line,
        /// it is added to the output string, otherwise it is skipped. Finally, it returns the output
        /// string containing only the non-consecutive duplicate lines.
        ///
        /// It's important to note that this method does not compare lines case-sensitive.
        /// If you want case-sensitive comparison, you can replace the equality operator
        /// with string.Compare(line, previousLine) != 0
        /// </summary>
        public static string RemoveConsecutiveDuplicateLinesFromString(string input)
        {
            string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            StringBuilder output = new();
            string previousLine = "";
            foreach (string line in lines)
            {
                if (line != previousLine)
                {
                    output.AppendLine(line);
                    previousLine = line;
                }
            }
            string returnValue = output.ToString();
            if (returnValue.EndsWith("\r\n"))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 2);
            }
            return returnValue;
        }

        /// <summary>
        /// Removes empty lines from a piece of string
        /// </summary>
        public static string RemoveEmtyLinesFromString(string input)
        {
            string[] nonEmptyLines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(Environment.NewLine, nonEmptyLines);
        }
    }
}
