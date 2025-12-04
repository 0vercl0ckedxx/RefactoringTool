using System.Text.RegularExpressions;
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

            // --- Логіка для тесту 12 (Return) ---
            if (methodBody.Contains("return"))
            {
                string tempBody = methodBody.Trim().TrimStart('{').TrimEnd('}').Trim();
                if (tempBody.StartsWith("return"))
                {
                    methodBody = Regex.Replace(tempBody, @"^return\s+(.+?);$", "$1");
                }
            }

            // --- ГОЛОВНА ЗАМІНА ---
            string escapedName = Regex.Escape(methodName);

            // ВИПРАВЛЕНИЙ ПАТЕРН: (?:\s*;)? гарантує, що пробіли зникнуть тільки разом з ;
            string pattern = $@"\b{escapedName}\s*\(\s*\)(?:\s*;)?";

            return Regex.Replace(code, pattern, match =>
            {
                return methodBody;
            });
        }
    }
}
