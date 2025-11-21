using Core.Models;
using Core.Refactorings;

namespace Tests
{
    public class RenameVariableRefactoringTests
    {
        private readonly RenameVariableRefactoring refactoring = new RenameVariableRefactoring();
        private string RunRefactoring(string code, string oldName, string newName)
        {
            var parameters = new RefactoringParameters();
            parameters.Parameters["oldName"] = oldName;
            parameters.Parameters["newName"] = newName;

            return refactoring.Apply(code, parameters);
        }

        [Fact]
        public void Test_1_SimpleRename()
        {
            string code = "int m = 7;";
            string expected = "int n = 7;";
            string result = RunRefactoring(code, "m", "n");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_2_MultipleVariables()
        {
            string code = "double p = 2.5, q = 4.1;";
            string expected = "double p = 2.5, r = 4.1;";
            string result = RunRefactoring(code, "q", "r");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_3_IgnoreCommentedName()
        {
            string code = "string city = \"Kyiv\"; // місто city";
            string expected = "string town = \"Kyiv\"; // місто city";
            string result = RunRefactoring(code, "city", "town");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_4_NoSuchVariable()
        {
            string code = "string title = \"Book\"; // назва title";
            string expected = "string title = \"Book\"; // назва title";
            string result = RunRefactoring(code, "unknown", "newTitle");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_5_ExpressionChange()
        {
            string code = "int r = 8, s = t + 12;";
            string expected = "int r = 8, r = t + 12;";
            string result = RunRefactoring(code, "s", "r");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_6_WhileLoop()
        {
            string code = "#include<iostream>\nint counter = 0; while(flag < 3)";
            string expected = "#include<iostream>\nint counter = 0; while(signal < 3)";
            string result = RunRefactoring(code, "flag", "signal");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_7_ForLoop()
        {
            string code = "#include<iostream>\nint sum = 0; for(int k = 0; k < 5; k++)";
            string expected = "#include<iostream>\nint sum = 0; for(int index = 0; index < 5; index++)";
            string result = RunRefactoring(code, "k", "index");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_8_FunctionParameters()
        {
            string code = "int multiply(int x, int y){return x*y;}";
            string expected = "int multiply(int a, int y){return a*y;}";
            string result = RunRefactoring(code, "x", "a");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_9_ConditionalRename()
        {
            string code = "int p = 2, q = 3; if(p < q){p = p + q;}";
            string expected = "int r = 2, q = 3; if(r < q){r = r + q;}";
            string result = RunRefactoring(code, "p", "r");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_10_StringLiteral()
        {
            string code = "string user = \"user\"; // змінна user";
            string expected = "string account = \"user\"; // змінна user";
            string result = RunRefactoring(code, "user", "account");
            Assert.Equal(expected, result);
        }
    }
}