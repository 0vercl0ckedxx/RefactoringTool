using Core.Models;
using Core.Refactorings;

namespace Tests
{
    public class InlineMethodRefactoringTests
    {
        [Fact]
        // Тест 1: проста підстановка тіла методу замість виклику
        public void Apply_Inlines_SimpleMethodCall()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { Foo(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int x = 5; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { { int x = 5; } }", result);
        }

        [Fact]
        // Тест 2: якщо виклик методу відсутній, код не повинен змінюватися
        public void Apply_DoesNotChangeCode_IfMethodNotCalled()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { Bar(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int x = 5; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { Bar(); }", result);
        }

        [Fact]
        // Тест 3: перевірка, що всі виклики Foo() замінюються
        public void Apply_Inlines_MultipleCalls()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { Foo(); Foo(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int y = 10; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { { int y = 10; } { int y = 10; } }", result);
        }

        [Fact]
        // Тест 4: порожнє тіло методу не повинно "ламати" код
        public void Apply_EmptyBody_DoesNotBreakCode()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { Foo(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{}";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { {} }", result);
        }

        [Fact]
        // Тест 5: якщо між ім’ям методу і дужками є пробіли — заміна не відбувається
        public void Apply_WithSpaces_StillWorks()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { Foo (); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int z = 9; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { Foo (); }", result); // бо Replace шукає точне Foo();
        }

        [Fact]
        // Тест 6: перевірка, що метод CanApply() завжди повертає true
        public void CanApply_ReturnsTrueAlways()
        {
            var refactoring = new InlineMethodRefactoring();
            bool canApply = refactoring.CanApply("any code");
            Assert.True(canApply);
        }

        [Fact]
        // Тест 7: якщо тіло методу не передано, код не змінюється
        public void Apply_HandlesNullBodyParameter()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { Foo(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { Foo(); }", result);
        }

        [Fact]
        // Тест 8: якщо код порожній, нічого не змінюється
        public void Apply_HandlesEmptyCode()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int x = 5; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("", result);
        }

        [Fact]
        // Тест 9: перевірка чутливості до регістру
        public void Apply_HandlesMethodNameCaseSensitive()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { foo(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int x = 5; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { foo(); }", result);
        }

        [Fact]
        // Тест 10: замінюється лише точний виклик, а не схожі імена
        public void Apply_ReplacesOnlyExactCall()
        {
            var refactoring = new InlineMethodRefactoring();
            string inputCode = "void Main() { FooBar(); Foo(); }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Foo";
            parameters.Parameters["methodBody"] = "{ int a = 1; }";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal("void Main() { FooBar(); { int a = 1; } }", result);
        }
    }
}
