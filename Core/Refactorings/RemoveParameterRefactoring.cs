using System.Text.RegularExpressions;
using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class RemoveParameterRefactoring : IRefactoring
    {
        public string Name => "Remove Parameter (Видалити Параметр)";
        public string Description => "Видаляє з методу параметр та оновлює усі виклики.";

        public bool CanApply(string code) => true;

        public string Apply(string code, RefactoringParameters parameters)
        {
            string methodName = parameters.Get<string>("methodName");
            string paramName = parameters.Get<string>("parameterToRemove");
            string defaultValue = parameters.Get<string>("defaultValue") ?? "";

            if (string.IsNullOrWhiteSpace(methodName) || string.IsNullOrWhiteSpace(paramName))
            {
                return code;
            }

            // Знаходимо оголошення методу
            // Використовуємо іменовану групу 'Args' для аргументів
            string declarationPattern = $@"\b{Regex.Escape(methodName)}\s*(?:<[^>]*>)?\s*\((?<Args>[^)]*)\)";

            string updatedCode = Regex.Replace(code, declarationPattern, (Match m) =>
            {
                string originalArgs = m.Groups["Args"].Value;

                // Патерн параметра
                // [^,)]*? -> пошук будь-чого, що не є комою або дужкою (тип, атрибути, модифікатори)
                // \b{paramName}\b -> Точне ім'я параметра
                string paramSignature = $@"(?:params\s+)?[^,)]*?\b{Regex.Escape(paramName)}\b\s*(?:=[^,)]*)?";

                string newArgs = originalArgs;

                // Логіка видалення
                string commaBeforePattern = $@"\s*,\s*{paramSignature}";

                if (Regex.IsMatch(newArgs, commaBeforePattern))
                {
                    newArgs = Regex.Replace(newArgs, commaBeforePattern, "");
                }
                else
                {
                    string commaAfterPattern = $@"\s*{paramSignature}\s*,\s*";

                    if (Regex.IsMatch(newArgs, commaAfterPattern))
                    {
                        newArgs = Regex.Replace(newArgs, commaAfterPattern, "");
                    }
                    else
                    {
                        string singlePattern = $@"\s*{paramSignature}\s*";
                        newArgs = Regex.Replace(newArgs, singlePattern, "");
                    }
                }

                // Формуємо новий заголовок методу
                string prefix = code.Substring(m.Index, m.Groups["Args"].Index - m.Index);
                return $"{prefix}{newArgs})";
            });

            // Заміна використання в тілі методу (якщо є дефолтне значення)
            if (!string.IsNullOrEmpty(defaultValue))
            {
                string usagePattern = $@"\b{Regex.Escape(paramName)}\b";
                updatedCode = Regex.Replace(updatedCode, usagePattern, defaultValue);
            }

            return updatedCode;
        }
    }
}