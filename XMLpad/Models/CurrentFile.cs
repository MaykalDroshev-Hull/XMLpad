namespace XMLpad.Models
{
    class CurrentFile
    {
        private static CurrentFile? instance;

        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        private CurrentFile()
        {

        }
        public static CurrentFile getInstance()
        {
            instance ??= new CurrentFile();
            return instance;
        }
    }
}
