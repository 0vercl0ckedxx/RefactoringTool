using Xunit;
using Core.Refactorings;
using Core.Models;
using System.Text.RegularExpressions;

namespace Tests
{
    public class ExtractMethodRefactoringTests
    {
        // 1. Перевірка назви
        [Fact]
        public void Name_ShouldReturnExpectedValue()
        {
            var refactoring = new ExtractMethodRefactoring();
            Assert.Contains("Extract Method", refactoring.Name);
        }

        // 2. Перевірка опису
        [Fact]
        public void Description_ShouldReturnExpectedValue()
        {
            var refactoring = new ExtractMethodRefactoring();
            Assert.False(string.IsNullOrEmpty(refactoring.Description));
        }

        // 3. CanApply: Позитивний сценарій
        [Fact]
        public void CanApply_ShouldReturnTrue_ForValidBlock()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "public void Test() { int x = 1; }";
            Assert.True(refactoring.CanApply(code));
        }

        // 4. CanApply: Негативний сценарій (немає дужок)
        [Fact]
        public void CanApply_ShouldReturnFalse_ForCodeWithoutBraces()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "public void Test() => Console.WriteLine();";
            Assert.False(refactoring.CanApply(code));
        }

        // 5. CanApply: Негативний сценарій (порожнє тіло)
        [Fact]
        public void CanApply_ShouldReturnFalse_ForEmptyBody()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "public void Test() { }";
            // Поточна логіка вимагає, щоб тіло не було порожнім (IsNullOrWhiteSpace)
            Assert.False(refactoring.CanApply(code));
        }

        // 6. Apply: Базове виділення методу (з дефолтним ім'ям)
        [Fact]
        public void Apply_ShouldCreateNewMethod_WithDefaultName()
        {
            var refactoring = new ExtractMethodRefactoring();
            string inputCode = "public void Main() { Console.WriteLine(1); }";

            var parameters = new RefactoringParameters();
            // Не передаємо назву, має бути "NewMethod"

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Contains("NewMethod();", result);
            Assert.Contains("void NewMethod()", result);
        }

        // 7. Apply: Виділення з кастомним ім'ям (Основний сценарій)
        [Fact]
        public void Apply_ShouldUseCustomMethodName()
        {
            var refactoring = new ExtractMethodRefactoring();
            string inputCode = "public void Test() { DoWork(); }";

            var parameters = new RefactoringParameters();
            parameters.Parameters["newMethodName"] = "MyExtractedAction";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Contains("MyExtractedAction();", result);
            Assert.Contains("private void MyExtractedAction()", result);
        }

        // 8. Apply: Збереження форматування (відступів)
        [Fact]
        public void Apply_ShouldPreserveIndentation()
        {
            var refactoring = new ExtractMethodRefactoring();
            // Тут є відступи (4 пробіли)
            string inputCode = "void Main() {\n    int a = 1;\n}";

            var parameters = new RefactoringParameters();
            parameters.Parameters["newMethodName"] = "Calculations";

            string result = refactoring.Apply(inputCode, parameters);

            // Перевіряємо, що виклик вставлено з відступом
            Assert.Contains("    Calculations();", result);
        }

        // 9. Apply: Якщо CanApply false, код не змінюється
        [Fact]
        public void Apply_ShouldNotChangeCode_IfCanApplyIsFalse()
        {
            var refactoring = new ExtractMethodRefactoring();
            string inputCode = "invalid code without braces";

            var parameters = new RefactoringParameters();

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(inputCode, result);
        }

        // 10. Apply: Перевірка коректності тіла нового методу
        [Fact]
        public void Apply_NewMethod_ShouldContainOriginalCode()
        {
            var refactoring = new ExtractMethodRefactoring();
            string codeContent = "var x = 10; var y = 20;";
            string inputCode = $"void M() {{ {codeContent} }}";

            var parameters = new RefactoringParameters();
            parameters.Parameters["newMethodName"] = "Extracted";

            string result = refactoring.Apply(inputCode, parameters);

            // Перевіряємо, що старий код переїхав у новий метод
            Assert.Contains(codeContent, result);
        }

        // 11. Apply: Робота з багаторядковим кодом
        [Fact]
        public void Apply_ShouldHandleMultiLineBody()
        {
            var refactoring = new ExtractMethodRefactoring();
            string inputCode = @"void Main() {
    Step1();
    Step2();
}";
            var parameters = new RefactoringParameters();
            parameters.Parameters["newMethodName"] = "RunSteps";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Contains("RunSteps();", result);
            Assert.Contains("private void RunSteps()", result);
        }

        // 12. Apply: Стійкість до пробілів (Normalized Check)
        [Fact]
        public void Apply_ShouldProduceValidSyntaxStructure()
        {
            var refactoring = new ExtractMethodRefactoring();
            string inputCode = "void Test() { Call(); }";
            string expectedStructure = "void Test() { NewMethod(); } private void NewMethod() { Call(); }";

            var parameters = new RefactoringParameters();
            parameters.Parameters["newMethodName"] = "NewMethod";

            string result = refactoring.Apply(inputCode, parameters);

            // Порівнюємо без урахування зайвих пробілів/ентерів
            Assert.Equal(NormalizeSpace(expectedStructure), NormalizeSpace(result));
        }

        // Допоміжний метод для ігнорування різниці в пробілах/переносах
        private string NormalizeSpace(string input)
        {
            // Замінює будь-яку послідовність пробілів/табів/ентерів на один пробіл
            return Regex.Replace(input, @"\s+", " ").Trim();
        }
    }
}