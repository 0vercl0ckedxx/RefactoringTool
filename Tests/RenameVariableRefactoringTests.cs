namespace RefactorTool.Tests
{
    /// <summary>
    /// Модульні тести для перевірки функціональності перейменування змінних.
    /// </summary>
    public class RenameVariableTests
    {
        private readonly TextRefactorTool tool = new TextRefactorTool();

        [Fact]
        public void Test_1_SimpleRename()
        {
            string code = "int m = 7;";
            string expected = "int n = 7;";
            string result = tool.RenameVariable(code, "m", "n");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_2_MultipleVariables()
        {
            string code = "double p = 2.5, q = 4.1;";
            string expected = "double p = 2.5, r = 4.1;";
            string result = tool.RenameVariable(code, "q", "r");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_3_IgnoreCommentedName()
        {
            string code = "string city = \"Kyiv\"; // місто city";
            string expected = "string town = \"Kyiv\"; // місто city";
            string result = tool.RenameVariable(code, "city", "town");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_4_NoSuchVariable()
        {
            string code = "string title = \"Book\"; // назва title";
            string expected = "string title = \"Book\"; // назва title";
            string result = tool.RenameVariable(code, "unknown", "newTitle");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_5_ExpressionChange()
        {
            string code = "int r = 8, s = t + 12;";
            string expected = "int r = 8, r = t + 12;";
            string result = tool.RenameVariable(code, "s", "r");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_6_WhileLoop()
        {
            string code = "#include<iostream>\nint counter = 0; while(flag < 3)";
            string expected = "#include<iostream>\nint counter = 0; while(signal < 3)";
            string result = tool.RenameVariable(code, "flag", "signal");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_7_ForLoop()
        {
            string code = "#include<iostream>\nint sum = 0; for(int k = 0; k < 5; k++)";
            string expected = "#include<iostream>\nint sum = 0; for(int index = 0; index < 5; index++)";
            string result = tool.RenameVariable(code, "k", "index");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_8_FunctionParameters()
        {
            string code = "int multiply(int x, int y){return x*y;}";
            string expected = "int multiply(int a, int y){return a*y;}";
            string result = tool.RenameVariable(code, "x", "a");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_9_ConditionalRename()
        {
            string code = "int p = 2, q = 3; if(p < q){p = p + q;}";
            string expected = "int r = 2, q = 3; if(r < q){r = r + q;}";
            string result = tool.RenameVariable(code, "p", "r");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test_10_StringLiteral()
        {
            string code = "string user = \"user\"; // змінна user";
            string expected = "string account = \"user\"; // змінна user";
            string result = tool.RenameVariable(code, "user", "account");
            Assert.Equal(expected, result);
        }
    }
}
