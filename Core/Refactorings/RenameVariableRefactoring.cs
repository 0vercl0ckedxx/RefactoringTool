using System.Text.RegularExpressions;
using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class RenameVariableRefactoring : IRefactoring
    {
        public string Name => "Rename Variable (Перейменувати Змінну)";
        public string Description => "Перейменовує змінну та оновлює всі її використання.";

        public bool CanApply(string code) => true;

        public string Apply(string code, RefactoringParameters parameters)
        {
            string oldName = parameters.Get<string>("oldName");
            string newName = parameters.Get<string>("newName");

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(oldName) || string.IsNullOrWhiteSpace(newName))
                return code;

            // Патерн для безпечної заміни. Додано обробку символьних літералів та багаторядкових коментарів.
            // Групи: 1. Рядок | 2. Символ | 3. Однорядковий коментар | 4. Багаторядковий коментар | 5. Змінна
            string pattern = $@"(""[^""]*"")|('[^']*')|(//[^\n]*)|(/\*[\s\S]*?\*/)|\b{Regex.Escape(oldName)}\b";

            return Regex.Replace(code, pattern, match =>
            {
                string value = match.Value;

                // Ігноруємо рядки, символи та всі типи коментарів
                if (value.StartsWith("\"") || value.StartsWith("'") || value.StartsWith("//") || value.StartsWith("/*"))
                    return value;

                // Замінюємо, якщо це справжня змінна
                if (value == oldName)
                    return newName;

                return value;
            });
        }
    }
}
