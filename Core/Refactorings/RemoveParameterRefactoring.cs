using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core.Interfaces;
using Core.Models;

namespace Core.Refactorings
{
    public class RemoveParameterRefactoring : IRefactoring
    {
        public string Name => "Remove Parameter (Видалити Параметр)";
        public string Description => "Видаляє параметр з оголошення та викликів.";

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

            // Знаходимо базовий індекс параметра в оголошенні методу
            int paramIndex = GetParameterDefinitionIndex(code, methodName, paramName);

            if (paramIndex == -1)
            {
                return code;
            }

            // Видалення аргументів, враховуючи іменовані параметри
            string updatedCode = RemoveArgumentSmart(code, methodName, paramIndex, paramName);

            // Безпечна заміна в тілі методу, ігноруючи рядки
            if (!string.IsNullOrEmpty(defaultValue))
            {
                updatedCode = ReplaceIdentifierSafe(updatedCode, paramName, defaultValue);
            }

            return updatedCode;
        }

        private int GetParameterDefinitionIndex(string code, string methodName, string paramName)
        {
            // Шукаємо оголошення методу
            string pattern = $@"\b{Regex.Escape(methodName)}\s*(?:<[^>]*>)?\s*\((?<Args>[^)]*)\)";
            var matches = Regex.Matches(code, pattern);

            foreach (Match match in matches)
            {
                var args = SplitArguments(match.Groups["Args"].Value);
                for (int i = 0; i < args.Count; i++)
                {
                    if (Regex.IsMatch(args[i], $@"\b{Regex.Escape(paramName)}\b") &&
                        !args[i].Trim().StartsWith(paramName + ":"))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private string RemoveArgumentSmart(string code, string methodName, int defaultIndex, string paramName)
        {
            // Знаходимо всі місця (виклики та оголошення)
            string pattern = $@"\b{Regex.Escape(methodName)}\s*(?:<[^>]*>)?\s*\((?<Args>.*?)\)";

            return Regex.Replace(code, pattern, m =>
            {
                string originalArgs = m.Groups["Args"].Value;
                var argsList = SplitArguments(originalArgs);

                int indexToRemove = -1;

                // Шукаємо іменований аргумент
                for (int i = 0; i < argsList.Count; i++)
                {
                    // Перевірка: чи починається аргумент з "paramName:"
                    if (Regex.IsMatch(argsList[i].Trim(), $@"^{Regex.Escape(paramName)}\s*:"))
                    {
                        indexToRemove = i;
                        break;
                    }
                }
                if (indexToRemove == -1)
                {
                    indexToRemove = defaultIndex;
                }

                // Виконуємо видалення, якщо індекс валідний
                if (indexToRemove >= 0 && indexToRemove < argsList.Count)
                {
                    argsList.RemoveAt(indexToRemove);
                }

                // Збираємо рядок назад
                string newArgs = string.Join(", ", argsList.Select(a => a.Trim()));

                // Відновлюємо префікс до дужок
                int argsIndexInMatch = m.Groups["Args"].Index - m.Index;
                string prefix = m.Value.Substring(0, argsIndexInMatch);

                return $"{prefix}{newArgs})";
            }, RegexOptions.Singleline);
        }

        private List<string> SplitArguments(string argsStr)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(argsStr)) return result;

            int parenthesisLevel = 0;
            int angleBracketLevel = 0;
            int curlyBracketLevel = 0;
            bool insideString = false;
            char stringChar = '\0';

            StringBuilder currentArg = new StringBuilder();

            for (int i = 0; i < argsStr.Length; i++)
            {
                char c = argsStr[i];

                if ((c == '"' || c == '\'') && (i == 0 || argsStr[i - 1] != '\\'))
                {
                    if (insideString && c == stringChar) insideString = false;
                    else if (!insideString) { insideString = true; stringChar = c; }
                }

                if (!insideString)
                {
                    if (c == '(') parenthesisLevel++;
                    else if (c == ')') parenthesisLevel--;
                    else if (c == '{') curlyBracketLevel++;
                    else if (c == '}') curlyBracketLevel--;
                    else if (c == '<') angleBracketLevel++;
                    else if (c == '>') angleBracketLevel--;
                }

                if (c == ',' && parenthesisLevel == 0 && angleBracketLevel == 0 && curlyBracketLevel == 0 && !insideString)
                {
                    result.Add(currentArg.ToString());
                    currentArg.Clear();
                }
                else
                {
                    currentArg.Append(c);
                }
            }

            if (currentArg.Length > 0)
                result.Add(currentArg.ToString());

            return result;
        }

        private string ReplaceIdentifierSafe(string code, string identifier, string replacement)
        {
            // Ігноруємо рядки та char, замінюємо тільки код
            string pattern = $@"(@?""(?:""""|[^""])*"")|('[^']*')|(\b{Regex.Escape(identifier)}\b)";

            return Regex.Replace(code, pattern, m =>
            {
                if (m.Groups[1].Success || m.Groups[2].Success) return m.Value;
                if (m.Groups[3].Success) return replacement;
                return m.Value;
            }, RegexOptions.Singleline);
        }
    }
}
