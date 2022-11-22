using System.IO;
using System.Threading.Tasks;

namespace TestGeneratorLibrary.Service
{
    public class CodeReader
    {
        public async Task<string> ReadAsync(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return await sr.ReadToEndAsync();
            }      
        }    
    }
}