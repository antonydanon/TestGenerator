using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using TestGeneratorLibrary.Model;
using TestGeneratorLibrary.Service.Generation;


namespace TestGeneratorTests
{
    [TestFixture]
    public class Tests
    {
 
        private readonly TestGeneratorService _testGenerator =
            new TestGeneratorService(new NamespaceIntegrationService(new ClassGenerationService())); 
        
        [Test]
        public void MethodCountTest()
        {
            List<FileInformation> generatedTests = _testGenerator.Generate(TestInput);
            Assert.That(generatedTests, Has.Count.EqualTo(1));
            var generatedClass = CSharpSyntaxTree.ParseText(generatedTests[0].FileContent).GetCompilationUnitRoot();
            var generatedMethods = generatedClass.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            Assert.That(generatedMethods, Has.Count.EqualTo(6));
        }

        [Test]
        public void OverloadedMethodsTest()
        {
            List<FileInformation> generatedTests = _testGenerator.Generate(TestInput);
            Assert.That(generatedTests, Has.Count.EqualTo(1));
            var generatedClass = CSharpSyntaxTree.ParseText(generatedTests[0].FileContent).GetCompilationUnitRoot();
            var generatedMethods = generatedClass.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
            
            Assert.That(generatedMethods[0].Identifier.Text, Is.EqualTo("GetFieldInfos1Test"));
            Assert.That(generatedMethods[1].Identifier.Text, Is.EqualTo("GetFieldInfos2Test"));
            Assert.That(generatedMethods[2].Identifier.Text, Is.EqualTo("GetFieldInfos3Test"));
            Assert.That(generatedMethods[3].Identifier.Text, Is.EqualTo("A1Test"));
            Assert.That(generatedMethods[4].Identifier.Text, Is.EqualTo("A2Test"));
        }

        [Test]
        public void InterfaceAndNotPublicClassTest()
        {
            List<FileInformation> generatedTests = _testGenerator.Generate(PrivateClassAndInterfaceInput);
            Assert.That(generatedTests, Has.Count.EqualTo(0));
        }
        
        private const string TestInput = @"
            using System;
            using System.Collections.Generic;
            using System.Reflection;

            namespace AssemblyBrowserCore.Service
            {
                public class FieldService
                {
                    public List<FieldInfo> GetFieldInfos(Type type)
                    {
                        List<FieldInfo> fieldInfos = new();
                        System.Reflection.FieldInfo[] fields = type.GetFields();
                        foreach (var field in fields)
                        {
                            FieldInfo fieldInfo = new FieldInfo();
                            fieldInfo.FieldName = field.Name;
                            fieldInfo.FieldType = field.FieldType.Name;
                            fieldInfos.Add(fieldInfo);
                        }
                        
                        return fieldInfos;
                    }
                    
                    public List<FieldInfo> GetFieldInfos(ParameterInfo[] parameterInfos)
                    {
                        List<FieldInfo> fieldInfos = new();
                        foreach (var parameterInfo in parameterInfos)
                        {
                            FieldInfo fieldInfo = new FieldInfo();
                            fieldInfo.FieldName = parameterInfo.Name;
                            fieldInfo.FieldType = parameterInfo.ParameterType.Name;
                            fieldInfos.Add(fieldInfo);
                        }
                        
                        return fieldInfos;
                    }
                    
                    public List<FieldInfo> GetFieldInfos(int a)
                    {
                        return new List<FieldInfo>();
                    }
                    
                    public int A(string b)
                    {
                        return 1;
                    }
                    
                    public int B(string b)
                    {
                        return 1;
                    }
                    
                    public int A(int a)
                    {
                        return 0;
                    }
                }
            }";
        
        private const string PrivateClassAndInterfaceInput = @"
            using System;
            using System.Collections.Generic;
            using System.Reflection;

            namespace AssemblyBrowserCore.Service
            {
                private class FieldService
                {
                    public List<FieldInfo> GetFieldInfos(Type type)
                    {
                        List<FieldInfo> fieldInfos = new();
                        System.Reflection.FieldInfo[] fields = type.GetFields();
                        foreach (var field in fields)
                        {
                            FieldInfo fieldInfo = new FieldInfo();
                            fieldInfo.FieldName = field.Name;
                            fieldInfo.FieldType = field.FieldType.Name;
                            fieldInfos.Add(fieldInfo);
                        }
                        
                        return fieldInfos;
                    }
                }

                public interface IPrintable
                {
                    void Print();
                }  
            }";
    }
}