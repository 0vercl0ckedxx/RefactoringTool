using System.Text.RegularExpressions;
using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class RenameVariableRefactoring : IRefactoring
    {
        public string Name => "Rename Variable (Перейменування Змінної)";
        public string Description => "Перейменовує змінну та оновлює всі її використання, ігноруючи коментарі та рядкові літерали.";

        public bool CanApply(string code) => true;

        public string Apply(string code, RefactoringParameters parameters)
        {
            string oldName = parameters.Get<string>("oldName");
            string newName = parameters.Get<string>("newName");

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
                return code;

            // Патерн для безпечної заміни
            string pattern = $@"(""[^""]*"")|(//[^\n]*)|\b{Regex.Escape(oldName)}\b";

            return Regex.Replace(code, pattern, match =>
            {
                string value = match.Value;

                // Ігноруємо коментарі та рядки
                if (value.StartsWith("\"") || value.StartsWith("//"))
                    return value;

                // Замінюємо, якщо це справжня змінна
                if (value == oldName)
                    return newName;

                return value;
            });
        }
    }
}