namespace ExcelTool.Models
{
    public class ExcelFile
    {

        public ExcelFile(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }

        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}