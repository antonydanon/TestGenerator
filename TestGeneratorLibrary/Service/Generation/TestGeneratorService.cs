using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGeneratorLibrary.Model;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TestGeneratorLibrary.Service.Generation
{
    public class TestGeneratorService
    {

        private readonly NamespaceIntegrationService _namespaceIntegrationService;

        public TestGeneratorService(NamespaceIntegrationService namespaceIntegrationService)
        {
            _namespaceIntegrationService = namespaceIntegrationService;
        }

        public List<FileInformation> Generate(string source)
        {
            CompilationUnitSyntax root = CSharpSyntaxTree.ParseText(source).GetCompilationUnitRoot();
            List<ClassDeclarationSyntax> classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(node => node.Modifiers.Any(n => n.Kind() == SyntaxKind.PublicKeyword)).ToList();
            
            List<ClassInformation> classesDeclarations = classes.Select(_namespaceIntegrationService.GenerateTestClassWithNamespaces).ToList();
            return GenerateFileInformation(classesDeclarations);
        }

        private List<FileInformation> GenerateFileInformation(List<ClassInformation> classesDeclarations)
        {
            return classesDeclarations.Select(classData => new FileInformation(classData.ClassName,
                CompilationUnit()
                    .WithUsings(new SyntaxList<UsingDirectiveSyntax>()
                        .Add(UsingDirective(QualifiedName(IdentifierName("NUnit"), IdentifierName("Framework"))))
                    )
                    .AddMembers(classData.TestClassDeclarationSyntax)
                    .NormalizeWhitespace().ToFullString())).ToList();
        }
    }
}