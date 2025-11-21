using System;
using System.Windows.Forms;
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

            // ПІДКЛЮЧЕННЯ ПОДІЇ та ІНІЦІАЛІЗАЦІЯ
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0; // Вибір першого елементу для початкового налаштування UI
            }

            // Встановлення дефолтного коду
            InputCodeTextBox.Text = @"public void UpdateUser(int userId, bool force)
            {
                int status = 1;
                SaveToDb();
            }";
            MethodNameTextBox.Text = "SaveToDb";
            MethodBodyTextBox.Text = "{ Console.WriteLine(\"Saved!\"); }";
        }

        // Обробник зміни вибору у списку. Оновлює підписи.
        private void UpdateUIForSelectedRefactoring(string refactoringName)
        {
            this.Text = $"Refactoring Tool - {refactoringName}";

            string param1Label = "Параметр 1";
            string param2Label = "Параметр 2";

            // За замовчуванням ховаємо додаткове поле
            bool showDefaultValue = false;

            switch (refactoringName)
            {
                case "InlineMethod":
                    param1Label = "Назва методу для вбудовування";
                    param2Label = "Тіло методу (наприклад: { return 10; })";
                    break;

                case "RemoveParameter":
                    param1Label = "Назва методу";
                    param2Label = "Назва параметра для видалення";
                    showDefaultValue = true; // <--- ПОКАЗУЄМО ТІЛЬКИ ТУТ!
                    break;

                case "Rename Variable":
                    param1Label = "Старе ім'я змінної";
                    param2Label = "Нове ім'я змінної";
                    break;

                default:
                    param1Label = "Введіть ім'я";
                    param2Label = "Введіть параметри";
                    break;
            }

            label2.Text = param1Label;
            label3.Text = param2Label;

            // Керуємо видимістю нових елементів
            labelDefaultValue.Visible = showDefaultValue;
            DefaultValueTextBox.Visible = showDefaultValue;

            // Очищення
            MethodNameTextBox.Text = "";
            MethodBodyTextBox.Text = "";
            DefaultValueTextBox.Text = "";
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
                MessageBox.Show("Будь ласка, оберіть рефакторинг зі списку.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRefactoringName = listBox1.SelectedItem.ToString();

            // Отримання вхідних даних з НОВИХ полів:
            string originalCode = InputCodeTextBox.Text; // <--- Використовуємо InputCodeTextBox
            string param1Value = MethodNameTextBox.Text.Trim();
            string param2Value = MethodBodyTextBox.Text.Trim();

            // Очищення поля результату перед виконанням
            ResultTextBox.Text = "";

            if (string.IsNullOrEmpty(param1Value))
            {
                MessageBox.Show("Будь ласка, введіть параметр 1.", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IRefactoring refactoring = null;
            var parameters = new RefactoringParameters();

            switch (selectedRefactoringName)
            {
                case "InlineMethod":
                    if (string.IsNullOrEmpty(param2Value))
                    {
                        MessageBox.Show("Для InlineMethod потрібне тіло методу.", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    refactoring = new InlineMethodRefactoring();
                    parameters.Parameters["methodName"] = param1Value;
                    parameters.Parameters["methodBody"] = param2Value;
                    break;

                case "RemoveParameter":
                    if (string.IsNullOrEmpty(param2Value))
                    {
                        MessageBox.Show("Введіть назву параметра.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    refactoring = new RemoveParameterRefactoring();
                    parameters.Parameters["methodName"] = param1Value;
                    parameters.Parameters["parameterToRemove"] = param2Value;

                    // === ДОДАЄМО ЦЕЙ БЛОК ===
                    // Якщо поле видиме і не порожнє — передаємо значення
                    if (DefaultValueTextBox.Visible && !string.IsNullOrWhiteSpace(DefaultValueTextBox.Text))
                    {
                        parameters.Parameters["defaultValue"] = DefaultValueTextBox.Text.Trim();
                    }
                    // ========================
                    break;

                case "Rename Variable":
                    if (string.IsNullOrEmpty(param2Value))
                    {
                        MessageBox.Show("Для Rename Variable потрібне Нове ім'я.", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    refactoring = new RenameVariableRefactoring();
                    parameters.Parameters["oldName"] = param1Value;
                    parameters.Parameters["newName"] = param2Value;
                    break;

                default:
                    MessageBox.Show($"Рефакторинг '{selectedRefactoringName}' не підтримується.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // Застосування рефакторингу
            if (refactoring != null)
            {
                try
                {
                    if (refactoring.CanApply(originalCode))
                    {
                        string refactoredCode = refactoring.Apply(originalCode, parameters);

                        // !!! ВІДОБРАЖЕННЯ РЕЗУЛЬТАТУ В НОВЕ ПОЛЕ !!!
                        ResultTextBox.Text = refactoredCode; // <--- Тут виводиться результат

                        MessageBox.Show($"{refactoring.Name} успішно застосовано!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ResultTextBox.Text = originalCode; // Якщо застосувати не можна, виводимо оригінал
                        MessageBox.Show($"Рефакторинг '{refactoring.Name}' не може бути застосовано до поточного коду.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Критична помилка при застосуванні {refactoring.Name}: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}