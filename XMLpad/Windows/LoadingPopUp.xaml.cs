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
    using System;
    using System.Threading.Tasks;
    using System.Windows;



    /// <summary>
    /// Interaction logic for LoadingPopUp.xaml
    /// </summary>
    public partial class LoadingPopUp : Window
    {
        /// <summary>
        /// The hidden TCS
        /// </summary>
        private readonly TaskCompletionSource<bool> _hiddenTcs = new TaskCompletionSource<bool>();

        /// <summary>
        /// The shown TCS
        /// </summary>
        private readonly TaskCompletionSource<bool> _shownTcs = new TaskCompletionSource<bool>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingPopUp"/> class.
        /// </summary>
        public LoadingPopUp()
        {
            InitializeComponent();
            Closed += (_, _) => _hiddenTcs.TrySetResult(true);
        }

        /// <summary>
        /// Shows the asynchronous.
        /// </summary>
        public async Task ShowAsync()
        {
            Show();
            await _shownTcs.Task;
        }

        /// <summary>
        /// Hides the asynchronous.
        /// </summary>
        public async Task HideAsync()
        {
            Hide();
            await _hiddenTcs.Task;
        }

        /// <summary>
        /// Handles the ContentRendered event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _shownTcs.TrySetResult(true);
        }
    }
}
