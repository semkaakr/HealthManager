using System;
using System.Windows.Forms;

namespace HealthManager
{
    public partial class ActivityForm : Form
    {
        private TextBox activityTypeTextBox;
        private TextBox durationTextBox;
        private Label activityTypeLabel;
        private Label durationLabel;
        public string ActivityType { get; private set; }
        public decimal Duration { get; private set; }
        public ActivityForm()
        {
            this.Text = "Добавить активность";
            this.Width = 300;
            this.Height = 200;
            CreateControls();
        }
        private void CreateControls()
        {
            activityTypeLabel = new Label
            {
                Text = "Тип активности:",
                Location = new System.Drawing.Point(10, 7),
                Name = "activityTypeLabel"  // Добавлено
            };
            activityTypeTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 30),
                Size = new System.Drawing.Size(200, 20),
                AutoSize = false,
                Name = "activityTypeTextBox"  // Добавлено
            };

            durationLabel = new Label
            {
                Text = "Продолжительность (минут):",
                Location = new System.Drawing.Point(10, 55),
                Size = new System.Drawing.Size(200, 20),
                AutoSize = false,
                Name = "durationLabel"  // Добавлено
            };

            durationTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 75),
                Size = new System.Drawing.Size(200, 20),
                Name = "durationTextBox"  // Добавлено
            };

            var okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(10, 100),
                Size = new System.Drawing.Size(80, 25),
                Name = "okButton"  // Добавлено
            };
            okButton.Click += (sender, e) =>
            {
                // Добавлена проверка на отрицательное значение
                if (decimal.TryParse(durationTextBox.Text, out decimal duration))
                {
                    if (duration < 0)
                    {
                        MessageBox.Show("Продолжительность не может быть отрицательной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(activityTypeTextBox.Text))
                    {
                        MessageBox.Show("Пожалуйста, введите тип активности.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ActivityType = activityTypeTextBox.Text;
                    Duration = duration;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное значение продолжительности.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            activityTypeTextBox.Name = "activityTypeTextBox";
            durationTextBox.Name = "durationTextBox";
            okButton.Name = "okButton";
            var cancelButton = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(130, 100),
                Size = new System.Drawing.Size(80, 25)
            };
            cancelButton.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            this.Controls.Add(activityTypeLabel);
            this.Controls.Add(activityTypeTextBox);
            this.Controls.Add(durationLabel);
            this.Controls.Add(durationTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}