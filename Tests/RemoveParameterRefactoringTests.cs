using Core.Models;
using Core.Refactorings;

namespace Tests
{
    public class RemoveParameterRefactoringTests
    {
        // Тест 1: Видалення єдиного параметра
        [Fact]
        public void Apply_Removes_SingleParameter()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void MyMethod(int x) { }";
            string expectedCode = "public void MyMethod() { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "MyMethod";
            parameters.Parameters["parameterToRemove"] = "x";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 2: Видалення першого з кількох параметрів
        [Fact]
        public void Apply_Removes_FirstOfMultipleParameters()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void Calculate(int a, string b, bool c) { }";
            string expectedCode = "public void Calculate(string b, bool c) { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Calculate";
            parameters.Parameters["parameterToRemove"] = "a";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 3: Видалення середнього з кількох параметрів
        [Fact]
        public void Apply_Removes_MiddleOfMultipleParameters()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void Process(string name, int age, double score) { }";
            string expectedCode = "public void Process(string name, double score) { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Process";
            parameters.Parameters["parameterToRemove"] = "age";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 4: Видалення останнього з кількох параметрів
        [Fact]
        public void Apply_Removes_LastOfMultipleParameters()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void Greet(string title, string name) { }";
            string expectedCode = "public void Greet(string title) { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Greet";
            parameters.Parameters["parameterToRemove"] = "name";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 5: Видалення параметра з тілом методу, що його використовує
        [Fact]
        public void Apply_Removes_ParameterUsedInMethodBody()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public int GetValue(int multiplier) { return 10 * multiplier; }";
            string expectedCode = "public int GetValue() { return 10 * 1; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "GetValue";
            parameters.Parameters["parameterToRemove"] = "multiplier";
            parameters.Parameters["defaultValue"] = "1";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 6: Видалення параметра з абстрактного методу
        [Fact]
        public void Apply_Removes_ParameterFromAbstractMethod()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public abstract void Setup(string config);";
            string expectedCode = "public abstract void Setup();";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Setup";
            parameters.Parameters["parameterToRemove"] = "config";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 7: Видалення параметра з конструктора
        [Fact]
        public void Apply_Removes_ParameterFromConstructor()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public class Person { public Person(string name) { } }";
            string expectedCode = "public class Person { public Person() { } }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Person";
            parameters.Parameters["parameterToRemove"] = "name";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 8: Видалення параметра типу масив
        [Fact]
        public void Apply_Removes_ArrayTypeParameter()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void ProcessData(string[] data) { }";
            string expectedCode = "public void ProcessData() { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "ProcessData";
            parameters.Parameters["parameterToRemove"] = "data";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 9: Видалення параметра типу Generic
        [Fact]
        public void Apply_Removes_GenericTypeParameter()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void SetItems<T>(List<T> items) { }";
            string expectedCode = "public void SetItems<T>() { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "SetItems";
            parameters.Parameters["parameterToRemove"] = "items";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }

        // Тест 10: Видалення параметра з ключовим словом 'params'
        [Fact]
        public void Apply_Removes_ParameterWithKeyword()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode = "public void Sum(params int[] numbers) { }";
            string expectedCode = "public void Sum() { }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Sum";
            parameters.Parameters["parameterToRemove"] = "numbers";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }
        // Тест 11: Видалення параметра при використанні іменованих аргументів (Named Arguments)
        [Fact]
        public void Apply_Removes_CorrectParameter_When_NamedArgumentsUsed()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode =
                "public void Resize(int width, int height) { }\n" +
                "public void Main() { Resize(height: 100, width: 50); }";

            string expectedCode =
                "public void Resize(int height) { }\n" +
                "public void Main() { Resize(height: 100); }";

            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Resize";
            parameters.Parameters["parameterToRemove"] = "width";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }
        // Тест 12: Видалення параметра в іменованих аргументах, де порядок змінено
        [Fact]
        public void Apply_Removes_CorrectParameter_When_NamedArguments_Are_Swapped()
        {
            // Arrange
            var refactoring = new RemoveParameterRefactoring();
            string inputCode =
                "public void Save(string text, bool encrypt) { }\n" +
                "public void Main() { Save(encrypt: true, text: \"data\"); }";

            string expectedCode =
                "public void Save(string text) { }\n" +
                "public void Main() { Save(text: \"data\"); }";

            var parameters = new RefactoringParameters();
            parameters.Parameters["methodName"] = "Save";
            parameters.Parameters["parameterToRemove"] = "encrypt";

            // Act
            string actualCode = refactoring.Apply(inputCode, parameters);

            // Assert
            Assert.Equal(expectedCode, actualCode);
        }
    }
}