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
    /// <summary>
    /// Class to store information about the current file.
    /// </summary>
    class CurrentFile
    {
        /// <summary>
        /// Instance of the class.
        /// </summary>
        private static CurrentFile? instance;

        /// <summary>
        /// Property to store the name of the current file.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Property to store the path of the current file.
        /// </summary>
        public string? FilePath { get; set; }

        /// <summary>
        /// Private constructor to ensure single instance of the class.
        /// </summary>
        private CurrentFile()
        {

        }

        /// <summary>
        /// Method to get the single instance of the class.
        /// </summary>
        /// <returns>Instance of the class</returns>
        public static CurrentFile GetInstance()
        {
            instance ??= new CurrentFile();
            return instance;
        }
    }
}
