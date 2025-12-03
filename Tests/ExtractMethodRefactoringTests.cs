using Core.Refactorings;

namespace Tests
{
    public class ExtractMethodRefactoringTests
    {
        // 1 - перевіряє, чи властивість Name повертає очікуване ім’я рефакторингу
        [Fact]
        public void Name_ShouldReturnExpectedValue()
        {
            var refactoring = new ExtractMethodRefactoring();
            Assert.Equal("Extract Method", refactoring.Name);
        }

        // 2 - перевіряє, чи властивість Description містить правильний опис
        [Fact]
        public void Description_ShouldReturnExpectedValue()
        {
            var refactoring = new ExtractMethodRefactoring();
            Assert.Equal("Refactors code by extracting a block into a new method.", refactoring.Description);
        }

        // 3 - перевіряє, що CanApply повертає true для коректного коду
        [Fact]
        public void CanApply_ShouldReturnTrue_ForValidCode()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "void Foo() { int a = 1; int b = 2; int c = a + b; }";
            Assert.True(refactoring.CanApply(code));
        }

        // 4 - перевіряє, що CanApply повертає false для порожнього рядка
        [Fact]
        public void CanApply_ShouldReturnFalse_ForEmptyString()
        {
            var refactoring = new ExtractMethodRefactoring();
            Assert.False(refactoring.CanApply(""));
        }

        // 5 - перевіряє, що CanApply повертає false для некоректного коду
        [Fact]
        public void CanApply_ShouldReturnFalse_ForInvalidCode()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "this is not code";
            Assert.False(refactoring.CanApply(code));
        }

        // 6 - перевіряє, що Apply повертає змінений код після успішного виділення методу
        [Fact]
        public void Apply_ShouldReturnModifiedCode_WhenBlockExtracted()
        {
            var refactoring = new ExtractMethodRefactoring();
            string original = "void Foo() { int x = 1; int y = 2; Console.WriteLine(x + y); }";
            string expected = "void Foo() { NewMethod(); } void NewMethod() { int x = 1; int y = 2; Console.WriteLine(x + y); }";
            Assert.Equal(expected, refactoring.Apply(original, null));
        }

        // 7 - перевіряє, що Apply не змінює код, якщо немає що виділяти
        [Fact]
        public void Apply_ShouldNotChangeCode_WhenNoExtractableBlock()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "void Foo() { Console.WriteLine(\"Hello\"); }";
            Assert.Equal(code, refactoring.Apply(code, null));
        }

        // 8 - перевіряє, що Apply зберігає форматування вихідного коду
        [Fact]
        public void Apply_ShouldPreserveFormatting()
        {
            var refactoring = new ExtractMethodRefactoring();
            string original = "void Foo()\n{\n    int x = 10;\n    int y = 20;\n    Console.WriteLine(x + y);\n}";
            string expected = "void Foo()\n{\n    NewMethod();\n} void NewMethod() {\n    int x = 10;\n    int y = 20;\n    Console.WriteLine(x + y);\n}";
            Assert.Equal(expected, refactoring.Apply(original, null));
        }

        // 9 - перевіряє, що Apply створює новий метод з ім’ям NewMethod
        [Fact]
        public void Apply_ShouldCreateNewMethodName()
        {
            var refactoring = new ExtractMethodRefactoring();
            string original = "void Foo() { int result = 5 + 3; Console.WriteLine(result); }";
            string expected = "void Foo() { NewMethod(); } void NewMethod() { int result = 5 + 3; Console.WriteLine(result); }";

            // Ensure the Apply method is called with the correct parameters
            string actual = refactoring.Apply(original, null);

            Assert.Equal(expected, actual);
        }

        // 10 - перевіряє, що CanApply розпізнає блок із кількома інструкціями, який можна виділити
        [Fact]
        public void CanApply_ShouldDetectBlockWithMultipleStatements()
        {
            var refactoring = new ExtractMethodRefactoring();
            string code = "void Bar() { int a = 1; int b = 2; int c = a + b; Console.WriteLine(c); }";
            Assert.True(refactoring.CanApply(code));
        }
    }
}
