namespace TestGeneratorLibrary.Model
{
    public class FileInformation
    {
        public string FileName { get; }
        public string FileContent { get; }

        public FileInformation(string fileName, string fileContent)
        {
            FileName = fileName;
            FileContent = fileContent;
        }
    }
}