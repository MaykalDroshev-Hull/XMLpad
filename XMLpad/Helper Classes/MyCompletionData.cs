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