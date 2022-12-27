using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLpad
{
    class CurrentFile
    {
        private string fileName;
        private string filePath;
        private static CurrentFile instance;

        public string FileName { get => fileName; set => fileName = value; }
        public string FilePath { get => filePath; set => filePath = value; }

        private CurrentFile()
        {

        }
        public static CurrentFile getInstance()
        {
            if(instance == null)
            {
                instance = new CurrentFile();
            }
            return instance;
        }
    }
}
