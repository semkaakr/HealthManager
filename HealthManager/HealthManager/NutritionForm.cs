using System;
using System.Windows.Forms;

namespace HealthManager
{
    public partial class NutritionForm : Form
    {
        private TextBox foodItemTextBox;
        private TextBox caloriesTextBox;
        private Label foodItemLabel;
        private Label caloriesLabel;
        public string FoodItem { get; private set; }
        public decimal Calories { get; private set; }

        public NutritionForm()
        {
            this.Text = "Добавить питание";
            this.Width = 250;
            this.Height = 200;
            this.Name = "nutritionForm";
            CreateControls();
        }

        private void CreateControls()
        {
            // ДОБАВЛЕНА СТРОКА - создание foodItemLabel
            foodItemLabel = new Label
            {
                Text = "Название пищи:",
                Location = new System.Drawing.Point(10, 7),
                Name = "foodItemLabel"
            };

            foodItemTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 30),
                Size = new System.Drawing.Size(200, 20),
                Name = "foodItemTextBox"
            };

            caloriesLabel = new Label
            {
                Text = "Калорийность:",
                Location = new System.Drawing.Point(10, 52),
                Name = "caloriesLabel"
            };

            caloriesTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 75),
                Size = new System.Drawing.Size(200, 20),
                Name = "caloriesTextBox"
            };

            var okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(10, 100),
                Size = new System.Drawing.Size(80, 25),
                Name = "okButton"
            };
            okButton.Click += (sender, e) =>
            {
                if (decimal.TryParse(caloriesTextBox.Text, out decimal calories))
                {
                    if (calories < 0)
                    {
                        MessageBox.Show("Калории должны быть неотрицательным числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(foodItemTextBox.Text))
                    {
                        MessageBox.Show("Пожалуйста, введите название пищи.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FoodItem = foodItemTextBox.Text;
                    Calories = calories;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное значение калорийности.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var cancelButton = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(130, 100),
                Size = new System.Drawing.Size(80, 25),
                Name = "cancelButton"
            };
            cancelButton.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            this.Controls.Add(foodItemLabel);
            this.Controls.Add(foodItemTextBox);
            this.Controls.Add(caloriesLabel);
            this.Controls.Add(caloriesTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}