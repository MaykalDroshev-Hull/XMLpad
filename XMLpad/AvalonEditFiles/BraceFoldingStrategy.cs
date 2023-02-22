// Copyright (c) 2009 Daniel Grunwald
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
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
    using System.Collections.Generic;
    using ICSharpCode.AvalonEdit.Document;
    using ICSharpCode.AvalonEdit.Folding;

    /// <summary>
    /// Allows producing foldings from a document based on braces.
    /// </summary>
    public class BraceFoldingStrategy : AbstractFoldingStrategy
    {
        /// <summary>
        /// Gets/Sets the opening brace. The default value is '{'.
        /// </summary>
        public char OpeningBrace { get; set; }

        /// <summary>
        /// Gets/Sets the closing brace. The default value is '}'.
        /// </summary>
        public char ClosingBrace { get; set; }

        /// <summary>
        /// Creates a new BraceFoldingStrategy.
        /// </summary>
        public BraceFoldingStrategy()
        {
            this.OpeningBrace = '{';
            this.ClosingBrace = '}';
        }

        /// <summary>
        /// Create <see cref="NewFolding"/>s for the specified document.
        /// </summary>
        public IEnumerable<NewFolding> CreateNewFoldings(ITextSource document)
        {
            List<NewFolding> newFoldings = new();

            Stack<int> startOffsets = new();  // initialize a stack to keep track of start offsets
            int lastNewLineOffset = 0;  // initialize the last new line offset to 0
            char openingBrace = this.OpeningBrace;  // get the opening brace character from the current object
            char closingBrace = this.ClosingBrace;  // get the closing brace character from the current object

            for (int i = 0; i < document.TextLength; i++)  // loop through each character in the document
            {
                char c = document.GetCharAt(i);  // get the character at the current position

                if (c == openingBrace)  // if the character is the opening brace
                {
                    startOffsets.Push(i);  // push the current index onto the stack
                }
                else if (c == closingBrace && startOffsets.Count > 0)  // if the character is the closing brace and there are items in the stack
                {
                    int startOffset = startOffsets.Pop();  // pop the last index from the stack
                                                           // don't fold if opening and closing brace are on the same line
                    if (startOffset < lastNewLineOffset)  // if the start offset is before the last new line offset
                    {
                        newFoldings.Add(new NewFolding(startOffset, i + 1));  // add a new folding object to the list with the start and end offsets
                    }
                }
                else if (c == '\n' || c == '\r')  // if the character is a new line character
                {
                    lastNewLineOffset = i + 1;  // set the last new line offset to the current index plus one
                }
            }

            newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));  // sort the list of foldings by start offset
            return newFoldings;  // return the list of foldings

        }

        /// <summary>
        /// Create <see cref="NewFolding" />s for the specified document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="firstErrorOffset"></param>
        /// <returns></returns>
        public override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = -1;
            return CreateNewFoldings(document);
        }
    }
}
