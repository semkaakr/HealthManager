using System;
using System.Windows.Forms;

namespace HealthManager
{
    public partial class SleepForm : Form
    {
        private TextBox dateTextBox;
        private TextBox hoursTextBox;
        private Label dateLabel;
        private Label hoursLabel;
        public string Date { get; private set; }
        public decimal Hours { get; private set; }

        public SleepForm()
        {
            this.Text = "Добавить сон";
            this.Width = 300;
            this.Height = 200;
            CreateControls();
        }

        private void CreateControls()
        {
            dateLabel = new Label
            {
                Text = "Дата:",
                Location = new System.Drawing.Point(10, 7),
                Name = "dateLabel"
            };

            dateTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 30),
                Size = new System.Drawing.Size(200, 20),
                Name = "dateTextBox"
            };

            hoursLabel = new Label
            {
                Text = "Количество часов:",
                Location = new System.Drawing.Point(10, 55),
                Size = new System.Drawing.Size(200, 20),
                AutoSize = false,
                Name = "hoursLabel"
            };

            hoursTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 75),
                Size = new System.Drawing.Size(200, 20),
                Name = "hoursTextBox"
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
                if (decimal.TryParse(hoursTextBox.Text, out decimal hours))
                {
                    if (hours < 0)
                    {
                        MessageBox.Show("Количество часов сна не может быть отрицательным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(dateTextBox.Text))
                    {
                        MessageBox.Show("Пожалуйста, введите дату.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Date = dateTextBox.Text;
                    Hours = hours;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное значение часов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var cancelButton = new Button
            {
                Text = "Отмена",
                Location = new System.Drawing.Point(130, 100),
                Size = new System.Drawing.Size(80, 25),
                Name = "cancelButton"  // ИСПРАВЛЕНО: было "okButton"
            };

            cancelButton.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            this.Controls.Add(dateLabel);
            this.Controls.Add(dateTextBox);
            this.Controls.Add(hoursLabel);
            this.Controls.Add(hoursTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}