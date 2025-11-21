namespace Core.Models
{
    public class RefactoringParameters
    {
        public Dictionary<string, object> Parameters { get; set; } = new();
        public T Get<T>(string key)
        {
            return Parameters.TryGetValue(key, out object value) 
                ? (T)value 
                : default;
        }

        public const string MethodNameKey = "methodName"; //Для InlineMethodRefactoring 
        public const string MethodBodyKey = "methodBody"; //Для InlineMethodRefactoring 
    }
}
