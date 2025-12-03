using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Refactorings
{
public class ExtractMethodRefactoring : IRefactoring
{
        public string Name => "Extract Method";
        public string Description => "Refactors code by extracting a block into a new method.";
        public bool CanApply(string code)
    {
        int braceOpen = code.IndexOf('{');
        if (braceOpen == -1) return false;
        int braceClose = code.LastIndexOf('}');
        if (braceClose == -1) return false;
        string body = code.Substring(braceOpen + 1, braceClose - braceOpen - 1).Trim();
        int semiCount = body.Count(c => c == ';');
        return semiCount > 1;
    }

    public string Apply(string code, RefactoringParameters parameters)
    {
        if (!CanApply(code)) return code;

        int braceOpen = code.IndexOf('{');
        int braceClose = code.LastIndexOf('}');
        string before = code.Substring(0, braceOpen + 1);
        string body = code.Substring(braceOpen + 1, braceClose - braceOpen - 1);
        string after = code.Substring(braceClose);

        string indent = GetIndent(body);
        string call = indent + "NewMethod();";
        bool hasNewlines = body.Contains('\n');
        if (hasNewlines)
        {
            call = "\n" + call + "\n";
        }
        else
        {
            call = " " + call + " ";
        }

        string newFoo = before + call + after;
        string newMethod = " void NewMethod() {" + body + "}";
        return newFoo + newMethod;
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