using System.Collections.Generic;

namespace TestGeneratorLibrary.Model
{
    public class ClassInfo
    {
        public string NamespaceName { get; }
        public string Name { get; }
        public IEnumerable<MethodInfo> Methods { get; }

        public ClassInfo(string namespaceName, string name, IEnumerable<MethodInfo> methods)
        {
            NamespaceName = namespaceName;
            Name = name;            
            Methods = methods;
        }    
    }
}