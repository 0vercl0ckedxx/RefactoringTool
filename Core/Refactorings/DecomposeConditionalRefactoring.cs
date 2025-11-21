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

            // створення версії умови, загорнутої у дужки; приклад: "(score >= threshold ...)"
            // щоб прибрати зайві дужки коло виклику методу при обробці тернарних операторів
            string patternWithParentheses = $"({condition})";

            if (code.Contains(patternWithParentheses))  // якщо в коді зустрічається умова в дужках
            {
                // потрібно з'ясувати, чи це тернарний оператор
                // застосування регулярного виразу
                bool requiresParentheses = Regex.IsMatch(code, $@"\b(if|while|switch)\s*{Regex.Escape(patternWithParentheses)}");

                // якщо це не if, не while, не switch, то обробка тернаного оператора
                if (!requiresParentheses)
                {
                    return code.Replace(patternWithParentheses, methodCall);
                }
            }

            // стандартна заміна
            return code.Replace(condition, methodCall);
        }
    }
}