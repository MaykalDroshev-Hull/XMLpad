namespace XMLpad.Models
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
