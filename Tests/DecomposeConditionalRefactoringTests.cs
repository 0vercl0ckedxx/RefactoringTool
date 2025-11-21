using Core.Models;
using Core.Refactorings;

namespace Tests
{
    public class DecomposeConditionalRefactoringTests
    {
        [Fact]
        // тест 1: заміна умови на виклик методу
        public void Apply_ReplacesConditionWithMethodCall()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"if (score >= threshold || hasBonusPoints) { status = ""Pass""; } else { status = ""Fail""; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(@"if (isPassing()) { status = ""Pass""; } else { status = ""Fail""; }", result);
        }

        [Fact]
        // тест 2: якщо умова потрібного вигляду в коді відсутня, змін не буде
        public void Apply_MakesNoChanges_IfConditionNotFound()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"if (score > threshold) { status = ""Pass""; } else { status = ""Fail""; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(inputCode, result);
        }

        [Fact]
        // тест 3: якщо передано порожній аргумент умови, змін не буде
        public void Apply_MakesNoChanges_IfNoConditionParameter()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"if (score >= threshold || hasBonusPoints) { status = ""Pass""; } else { status = ""Fail""; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(inputCode, result);
        }

        [Fact]
        // тест 4: якщо в коді умова записана без пробілів в тих самих місцях, змін не буде
        public void Apply_MakesNoChanges_WithDifferentSpacing()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"if (score>=threshold||hasBonusPoints) { status = ""Pass""; } else { status = ""Fail""; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(inputCode, result);
        }

        [Fact]
        // тест 5: декомпозиція для тернарного оператора
        public void Apply_HandlesTernaryOperator()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"status = (score >= threshold || hasBonusPoints) ? ""Pass"" : ""Fail"";";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(@"status = isPassing() ? ""Pass"" : ""Fail"";", result);
        }

        [Fact]
        // тест 6: якщо не передано newConditionName, змін не буде
        public void Apply_HandlesMissingNewConditionName()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"if (score >= threshold || hasBonusPoints) { status = ""Pass""; } else { status = ""Fail""; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(inputCode, result);
        }

        [Fact]
        // тест 7: якщо код порожній, змін не буде
        public void Apply_HandlesEmptyCode()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(@"", result);
        }

        [Fact]
        // тест 8: чутливість до регістру
        public void Apply_IsCaseSensitive()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"if (Score >= threshold || hasBonusPoints) { status = ""Pass""; } else { status = ""Fail""; }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(inputCode, result);
        }

        [Fact]
        // тест 9: декомпозиція працює для вкладених операторів
        public void Apply_HandlesNestedConditionals()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"
               if (score < 0) { 
                   status = ""Error""; 
               } else { 
                   if (score >= threshold || hasBonusPoints) { 
                    status = ""Pass""; 
                   } else { 
                    status = ""Fail""; 
                   } 
               }";
            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(@"
               if (score < 0) { 
                   status = ""Error""; 
               } else { 
                   if (isPassing()) { 
                       status = ""Pass""; 
                   } else { 
                       status = ""Fail""; 
                   } 
               }", result);
        }

        [Fact]
        // тест 10: декомпозиція працює для else if
        public void Apply_HandlesConditionInElseIf()
        {
            var refactoring = new DecomposeConditionalRefactoring();
            string inputCode = @"
               if (score < 0) { 
                   status = ""Error""; 
               } else if (score >= threshold || hasBonusPoints) { 
                   status = ""Pass""; 
               } else { 
                   status = ""Fail""; 
               }";

            var parameters = new RefactoringParameters();
            parameters.Parameters["condition"] = "score >= threshold || hasBonusPoints";
            parameters.Parameters["newConditionName"] = "isPassing";
            parameters.Parameters["branchTrue"] = @"status = ""Pass"";";
            parameters.Parameters["branchFalse"] = @"status = ""Fail"";";

            string result = refactoring.Apply(inputCode, parameters);

            Assert.Equal(@"
               if (score < 0) { 
                   status = ""Error""; 
               } else if (isPassing()) { 
                   status = ""Pass""; 
               } else { 
                   status = ""Fail""; 
               }", result);
        }

    }
}
