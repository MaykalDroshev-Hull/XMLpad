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
    using ICSharpCode.AvalonEdit.Editing;
    using System;

    /// <summary>
    /// Implements the ICompletionData interface of AvalonEdit to provide entries for the
    /// completion drop down in the text editor.
    /// </summary>
    public class MyCompletionData : ICompletionData
    {
        /// <summary>
        /// Initializes a new instance of the MyCompletionData class with the specified text.
        /// </summary>
        /// <param name="text">The text to be displayed in the completion drop down.</param>
        public MyCompletionData(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// Gets the image source for the completion data.
        /// </summary>
        public System.Windows.Media.ImageSource? Image
        {
            get { return null; }
        }

        /// <summary>
        /// Gets or sets the text to be displayed in the completion drop down.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the content of the completion data.
        /// </summary>
        public object Content
        {
            get { return this.Text; }
        }

        /// <summary>
        /// Gets the description for the completion data.
        /// </summary>
        public object Description
        {
            get { return "Description for " + this.Text; }
        }

        /// <summary>
        /// Gets the priority of the completion data.
        /// </summary>
        public double Priority => 0;

        /// <summary>
        /// Completes the text in the text area.
        /// </summary>
        /// <param name="textArea">The text area of the editor.</param>
        /// <param name="completionSegment">The segment of text to be completed.</param>
        /// <param name="insertionRequestEventArgs">The event arguments for the insertion request.</param>
        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}