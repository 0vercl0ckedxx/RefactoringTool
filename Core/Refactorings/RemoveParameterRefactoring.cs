using System.Text.RegularExpressions;
using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class RemoveParameterRefactoring : IRefactoring
    {
        public string Name => "Remove Parameter";
        public string Description => "Removes a parameter from a method and updates all call sites.";

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

            // 1. Знаходимо оголошення методу
            string declarationPattern = $@"\b{Regex.Escape(methodName)}\s*(?:<[^>]*>)?\s*\((?<Args>[^)]*)\)";

            string updatedCode = Regex.Replace(code, declarationPattern, (Match m) =>
            {
                string originalArgs = m.Groups["Args"].Value;

                // 2. Патерн параметра (Тип + Назва)
                // (Кома в типі заборонена, щоб не захопити зайве)
                string paramSignature = $@"(?:params\s+)?[\w\[\]<>?\s.]+\b{Regex.Escape(paramName)}\b\s*(?:=[^,)]*)?";

                string newArgs = originalArgs;

                // 3. Логіка "Розумного видалення":

                // Крок А: Видаляємо ", параметр" (Середній або Останній)
                string commaBeforePattern = $@"\s*,\s*{paramSignature}";

                if (Regex.IsMatch(newArgs, commaBeforePattern))
                {
                    newArgs = Regex.Replace(newArgs, commaBeforePattern, "");
                }
                else
                {
                    // Крок Б: Видаляємо "параметр, " (Перший)
                    // !!! ВИПРАВЛЕННЯ ТУТ !!!
                    // Додано \s* в кінці, щоб видалити пробіл після коми
                    string commaAfterPattern = $@"\s*{paramSignature}\s*,\s*";

                    if (Regex.IsMatch(newArgs, commaAfterPattern))
                    {
                        newArgs = Regex.Replace(newArgs, commaAfterPattern, "");
                    }
                    else
                    {
                        // Крок В: Видаляємо просто параметр (Єдиний)
                        string singlePattern = $@"\s*{paramSignature}";
                        newArgs = Regex.Replace(newArgs, singlePattern, "");
                    }
                }

                string prefix = code.Substring(m.Index, m.Groups["Args"].Index - m.Index);
                return $"{prefix}{newArgs})";
            });

            // 4. Заміна використання
            if (!string.IsNullOrEmpty(defaultValue))
            {
                string usagePattern = $@"\b{Regex.Escape(paramName)}\b";
                updatedCode = Regex.Replace(updatedCode, usagePattern, defaultValue);
            }

            return updatedCode;
        }
    }
}
