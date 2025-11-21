namespace WinFormsUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            label1 = new Label();
            InputCodeTextBox = new RichTextBox();
            button1 = new Button();
            label2 = new Label();
            label3 = new Label();
            MethodNameTextBox = new TextBox();
            MethodBodyTextBox = new TextBox();
            listBox1 = new ListBox();
            ResultTextBox = new RichTextBox();
            label4 = new Label();
            labelDefaultValue = new Label();
            DefaultValueTextBox = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(18, 28);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(92, 20);
            label1.TabIndex = 0;
            label1.Text = "Вхідний код";
            // 
            // InputCodeTextBox
            // 
            InputCodeTextBox.Location = new Point(20, 60);
            InputCodeTextBox.Margin = new Padding(2);
            InputCodeTextBox.Name = "InputCodeTextBox";
            InputCodeTextBox.Size = new Size(321, 121);
            InputCodeTextBox.TabIndex = 1;
            InputCodeTextBox.Text = "public void UpdateUser(int userId, bool force)\n{\n    int status = 1;\n    SaveToDb();\n}";
            // 
            // button1
            // 
            button1.Location = new Point(384, 378);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(161, 72);
            button1.TabIndex = 2;
            button1.Text = "Натисни для рефакторинга";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 196);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(90, 20);
            label2.TabIndex = 3;
            label2.Text = "Введіть ім'я";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(297, 196);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(141, 20);
            label3.TabIndex = 4;
            label3.Text = "Введіть параметри";
            // 
            // MethodNameTextBox
            // 
            MethodNameTextBox.Location = new Point(20, 220);
            MethodNameTextBox.Margin = new Padding(2);
            MethodNameTextBox.Name = "MethodNameTextBox";
            MethodNameTextBox.Size = new Size(161, 27);
            MethodNameTextBox.TabIndex = 5;
            // 
            // MethodBodyTextBox
            // 
            MethodBodyTextBox.Location = new Point(297, 220);
            MethodBodyTextBox.Margin = new Padding(2);
            MethodBodyTextBox.Name = "MethodBodyTextBox";
            MethodBodyTextBox.Size = new Size(161, 27);
            MethodBodyTextBox.TabIndex = 6;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Items.AddRange(new object[] { "InlineMethod", "Rename Variable", "RemoveParameter", "Ref4" });
            listBox1.Location = new Point(384, 60);
            listBox1.Margin = new Padding(2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(161, 104);
            listBox1.TabIndex = 8;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // ResultTextBox
            // 
            ResultTextBox.Location = new Point(20, 296);
            ResultTextBox.Margin = new Padding(2);
            ResultTextBox.Name = "ResultTextBox";
            ResultTextBox.ReadOnly = true;
            ResultTextBox.Size = new Size(321, 179);
            ResultTextBox.TabIndex = 9;
            ResultTextBox.Text = "";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 264);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 10;
            label4.Text = "Результат";
            // 
            // labelDefaultValue
            // 
            labelDefaultValue.AutoSize = true;
            labelDefaultValue.Location = new Point(384, 272);
            labelDefaultValue.Margin = new Padding(2, 0, 2, 0);
            labelDefaultValue.Name = "labelDefaultValue";
            labelDefaultValue.Size = new Size(210, 20);
            labelDefaultValue.TabIndex = 4;
            labelDefaultValue.Text = "Значення за замовчуванням";
            labelDefaultValue.Visible = false;
            // 
            // DefaultValueTextBox
            // 
            DefaultValueTextBox.Location = new Point(384, 296);
            DefaultValueTextBox.Margin = new Padding(2);
            DefaultValueTextBox.Name = "DefaultValueTextBox";
            DefaultValueTextBox.Size = new Size(161, 27);
            DefaultValueTextBox.TabIndex = 6;
            DefaultValueTextBox.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(702, 515);
            Controls.Add(label4);
            Controls.Add(ResultTextBox);
            Controls.Add(listBox1);
            Controls.Add(DefaultValueTextBox);
            Controls.Add(MethodBodyTextBox);
            Controls.Add(MethodNameTextBox);
            Controls.Add(labelDefaultValue);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(InputCodeTextBox);
            Controls.Add(label1);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Refactoring Tool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private RichTextBox InputCodeTextBox; // ЗМІНЕНО
        private Button button1;
        private Label label2;
        private Label label3;
        private TextBox MethodNameTextBox;
        private TextBox MethodBodyTextBox;
        private ListBox listBox1;
        private RichTextBox ResultTextBox; // ДОДАНО
        private Label label4; // ДОДАНО
        private Label labelDefaultValue;
        private TextBox DefaultValueTextBox;
    }
}