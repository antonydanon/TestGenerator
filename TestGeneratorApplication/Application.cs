using System.Collections.Generic;
using System.Threading.Tasks;
using TestGeneratorLibrary.Service.Pipeline;

namespace TestGeneratorApplication
{
    public static class Application
    {
        public static async Task Main(string[] args)
        {
            List<string> correctInputFiles = new ();
            
            correctInputFiles.Add("C:\\Users\\anton\\Desktop\\СПП\\lab_3\\AssemblyBrowser\\AssemblyBrowserCore\\Service\\FieldService.cs");
            string outputDirectory = "C:\\Users\\anton\\Desktop\\СПП\\lab_4\\output";
            
            int degreeOfParallelismRead = 20;
            int degreeOfParallelismGenerate = 20;
            int degreeOfParallelismWrite = 20;
            
            PipelineService testsGeneratorService = new (degreeOfParallelismRead, degreeOfParallelismGenerate,
                                                                    degreeOfParallelismWrite, outputDirectory);
            
            await testsGeneratorService.Generate(correctInputFiles);
        }
    }
}