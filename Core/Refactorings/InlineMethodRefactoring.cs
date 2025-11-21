using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class InlineMethodRefactoring : IRefactoring
    {
        public string Name => "Inline Method (Вбудувати Метод)";

        public string Description => "Вбудовує тіло простого методу замість його виклику.";

        public bool CanApply(string code)
        {
            return true;
        }

        public string Apply(string code, RefactoringParameters parameters)
        {
            string methodName = parameters.Get<string>("methodName");
            string methodBody = parameters.Get<string>("methodBody");

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(methodBody) || string.IsNullOrEmpty(methodName))
            {
                return code;
            }

            string callToFind = $"{methodName}();";

            string result = code.Replace(callToFind, methodBody);

            return result;
        }
    }
}
