using System.Text.RegularExpressions;
using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class DecomposeConditionalRefactoring : IRefactoring
    {
        public string Name => "Decompose Conditional (Декомпозиція Умовного Оператора)";
        public string Description => "Замінює складну умову викликом окремого методу.";

        public bool CanApply(string code) => true;

        public string Apply(string code, RefactoringParameters parameters)
        {
            string condition = parameters.Get<string>("condition");  // витягується умова, котру треба замінити
            string newMethodName = parameters.Get<string>("newConditionName");  // витягується назва нового методу, на який замінимо
            // перевірка
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(condition) || string.IsNullOrEmpty(newMethodName))
            {
                return code;
            }
            // формування коректного виклику методу
            string methodCall = $"{newMethodName}()";

            // перетворення умови на безпечний для Regex рядок: оператори Regex будуть ігноруватися
            string escapedCondition = Regex.Escape(condition);

            // шаблон для пошуку:
            // група 1 - умова в рядках
            // група 2 - умова в коментарях
            // група 3 - умова в дужках (щоб прибрати зайві дужки коло виклику методу при обробці тернарних операторів)
            // група 4 - нічим не оточена умова
            string pattern = $@"(""[^""]*"")|(//[^\n]*)|(\(\s*{escapedCondition}\s*\))|({escapedCondition})";

            return Regex.Replace(code, pattern, match =>
            {
                // якщо умова зустрічається у рядку чи коментарі, її буде повернено без змін
                if (match.Groups[1].Success || match.Groups[2].Success)
                {
                    return match.Value;
                }

                // якщо в коді зустрічається умова у дужках
                if (match.Groups[3].Success)
                {
                    // потрібно з'ясувати, чи це тернарний оператор
                    // перевірка регулярним виразом тексту перед знайденим блоком на наявність if, while, switch
                    string textBefore = code.Substring(0, match.Index);
                    bool requiresParentheses = Regex.IsMatch(textBefore, @"\b(if|while|switch)\s*$");
                    // якщо це if, while чи switch, то навколо методу потрібні дужки
                    if (requiresParentheses)
                    {
                        return $"({methodCall})";
                    }
                    // якщо це не if, не while, не switch, то обробка тернаного оператора
                    return methodCall;
                }

                // стандартна заміна
                return methodCall;
            });
        }
    }
}
