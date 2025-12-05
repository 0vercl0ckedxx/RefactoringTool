using Core.Interfaces;
using Core.Models;
using System.Linq;

namespace Core.Refactorings
{
    public class ExtractMethodRefactoring : IRefactoring
    {
        public string Name => "Extract Method (Виділити метод)";
        public string Description => "Виносить тіло методу в новий окремий метод.";

        public bool CanApply(string code)
        {
            // Перевірка на наявність фігурних дужок та хоча б однієї крапки з комою всередині
            int braceOpen = code.IndexOf('{');
            if (braceOpen == -1) return false;
            int braceClose = code.LastIndexOf('}');
            if (braceClose == -1) return false;

            // Проста перевірка, чи є всередині код
            string body = code.Substring(braceOpen + 1, braceClose - braceOpen - 1).Trim();
            return !string.IsNullOrWhiteSpace(body);
        }

        public string Apply(string code, RefactoringParameters parameters)
        {
            if (!CanApply(code)) return code;

            // Отримуємо назву нового методу з параметрів UI
            string newMethodName = parameters.Get<string>("newMethodName");
            if (string.IsNullOrEmpty(newMethodName))
            {
                newMethodName = "NewMethod"; // Значення за замовчуванням
            }

            int braceOpen = code.IndexOf('{');
            int braceClose = code.LastIndexOf('}');

            string before = code.Substring(0, braceOpen + 1);
            string body = code.Substring(braceOpen + 1, braceClose - braceOpen - 1);
            string after = code.Substring(braceClose);

            string indent = GetIndent(body);

            // Формуємо виклик нового методу
            string call = indent + $"{newMethodName}();";

            bool hasNewlines = body.Contains('\n');
            if (hasNewlines)
            {
                call = "\n" + call + "\n" + indent; // Додаємо відступи для краси
            }
            else
            {
                call = " " + call + " ";
            }

            // Формуємо оновлений старий метод
            string updatedOriginalMethod = before + call + after;

            // Формуємо код нового методу (додаємо його в кінець)
            // Примітка: у реальному коді краще шукати кінець класу, але для прикладу додаємо в кінець рядка
            string newMethodCode = $"\n\n    private void {newMethodName}()\n    {{\n{body}\n    }}";

            return updatedOriginalMethod + newMethodCode;
        }

        private static string GetIndent(string body)
        {
            if (!body.Contains('\n')) return "";

            var lines = body.Split('\n');
            var nonEmptyLines = lines.Where(l => !string.IsNullOrWhiteSpace(l));
            if (!nonEmptyLines.Any()) return "";

            string firstNonEmpty = nonEmptyLines.First();
            int indentCount = firstNonEmpty.TakeWhile(c => c == ' ' || c == '\t').Count();
            return new string(' ', indentCount);
        }
    }
}