using Core.Interfaces;
using Core.Models;
using Core.Refactorings;

namespace WinFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Підключення події
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);

            // Вибираємо перший елемент при старті
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }

            // Дефолтний код для тестування
            InputCodeTextBox.Text = @"public void CheckAccess()
{
    if (user.Age > 18 && user.HasLicense && !user.IsBanned)
    {
        GrantAccess();
    }
}";
        }

        // Метод оновлення написів та полів
        private void UpdateUIForSelectedRefactoring(string refactoringName)
        {
            this.Text = $"Refactoring Tool - {refactoringName}";

            string param1Label = "Параметр 1";
            string param2Label = "Параметр 2";
            bool showDefaultValue = false; // За замовчуванням ховаємо поле

            switch (refactoringName)
            {
                case "InlineMethod":
                    param1Label = "Назва методу для вбудовування";
                    param2Label = "Тіло методу (наприклад: { return 10; })";
                    break;

                case "RemoveParameter":
                    param1Label = "Назва методу";
                    param2Label = "Назва параметра для видалення";
                    showDefaultValue = true; // Показуємо для цього рефакторингу
                    break;

                case "Rename Variable":
                    param1Label = "Старе ім'я змінної";
                    param2Label = "Нове ім'я змінної";
                    break;

                case "DecomposeConditional":
                    param1Label = "Складна умова (напр. x > 5 && y < 10)";
                    param2Label = "Назва нового методу (напр. IsValid)";
                    break;

                default:
                    param1Label = "Введіть ім'я";
                    param2Label = "Введіть параметри";
                    break;
            }

            label2.Text = param1Label;
            label3.Text = param2Label;

            // Керування видимістю поля DefaultValue
            if (labelDefaultValue != null && DefaultValueTextBox != null)
            {
                labelDefaultValue.Visible = showDefaultValue;
                DefaultValueTextBox.Visible = showDefaultValue;
                DefaultValueTextBox.Text = "";
            }

            // Очищення полів
            MethodNameTextBox.Text = "";
            MethodBodyTextBox.Text = "";
            ResultTextBox.Text = "";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                UpdateUIForSelectedRefactoring(listBox1.SelectedItem.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, оберіть рефакторинг зі списку.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRefactoringName = listBox1.SelectedItem.ToString();
            string originalCode = InputCodeTextBox.Text;

            string param1Value = MethodNameTextBox.Text.Trim();
            string param2Value = MethodBodyTextBox.Text.Trim();

            ResultTextBox.Text = ""; // Очищення результату

            if (string.IsNullOrEmpty(param1Value))
            {
                MessageBox.Show("Будь ласка, заповніть перше поле.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IRefactoring refactoring = null;
            var parameters = new RefactoringParameters();

            switch (selectedRefactoringName)
            {
                case "InlineMethod":
                    if (string.IsNullOrEmpty(param2Value)) { MessageBox.Show("Введіть тіло методу.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    refactoring = new InlineMethodRefactoring();
                    parameters.Parameters["methodName"] = param1Value;
                    parameters.Parameters["methodBody"] = param2Value;
                    break;

                case "RemoveParameter":
                    if (string.IsNullOrEmpty(param2Value)) { MessageBox.Show("Введіть назву параметра.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    refactoring = new RemoveParameterRefactoring();
                    parameters.Parameters["methodName"] = param1Value;
                    parameters.Parameters["parameterToRemove"] = param2Value;
                    // Додаємо defaultValue, якщо поле видиме і заповнене
                    if (DefaultValueTextBox.Visible && !string.IsNullOrWhiteSpace(DefaultValueTextBox.Text))
                    {
                        parameters.Parameters["defaultValue"] = DefaultValueTextBox.Text.Trim();
                    }
                    break;

                case "Rename Variable":
                    if (string.IsNullOrEmpty(param2Value)) { MessageBox.Show("Введіть нове ім'я.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    refactoring = new RenameVariableRefactoring();
                    parameters.Parameters["oldName"] = param1Value;
                    parameters.Parameters["newName"] = param2Value;
                    break;

                case "DecomposeConditional":
                    if (string.IsNullOrEmpty(param2Value)) { MessageBox.Show("Введіть назву нового методу.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    refactoring = new DecomposeConditionalRefactoring();
                    parameters.Parameters["condition"] = param1Value;       // Умова
                    parameters.Parameters["newConditionName"] = param2Value; // Нова назва методу
                    break;

                default:
                    MessageBox.Show($"Рефакторинг '{selectedRefactoringName}' не підтримується або ще не реалізований.", "Інфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
            }

            // Виконання рефакторингу
            if (refactoring != null)
            {
                try
                {
                    if (refactoring.CanApply(originalCode))
                    {
                        string result = refactoring.Apply(originalCode, parameters);
                        ResultTextBox.Text = result;
                        MessageBox.Show($"{refactoring.Name} виконано успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ResultTextBox.Text = originalCode;
                        MessageBox.Show("Неможливо застосувати рефакторинг до цього коду.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}", "Критична помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResultTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}