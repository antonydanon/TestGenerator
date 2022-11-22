namespace TestGeneratorLibrary.Model
{
    public class GeneratedTestClass
    {
        public string TestClassName { get; }
        public string TestClassData { get; }

        public GeneratedTestClass(string testClassName, string testClassData)
        {
            TestClassName = testClassName;
            TestClassData = testClassData;
        }    
    }
}