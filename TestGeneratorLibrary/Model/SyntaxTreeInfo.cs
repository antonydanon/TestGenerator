using System.Collections.Generic;

namespace TestGeneratorLibrary.Model
{
    public class SyntaxTreeInfo
    {
        public IEnumerable<ClassInfo> Classes { get; }

        public SyntaxTreeInfo(IEnumerable<ClassInfo> classes)
        {
            Classes = classes;
        }   
    }
}