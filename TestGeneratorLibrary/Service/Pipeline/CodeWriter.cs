using System.IO;
using System.Threading.Tasks;

namespace TestGeneratorLibrary.Service.Pipeline
{
    public class CodeWriter
    {
        public async Task Write(string path, string fileContent)
        {
            await using var writer = new StreamWriter(path);
            await writer.WriteAsync(fileContent);    
        }
    }
}