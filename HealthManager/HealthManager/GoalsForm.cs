using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthManager
{
    public partial class GoalsForm : Form
    {
        private TextBox activityGoalTextBox;
        private TextBox calorieGoalTextBox;
        private TextBox sleepGoalTextBox;
        private Label activityGoalLabel;
        private Label calorieGoalLabel;
        private Label sleepGoalLabel;
        private Button okButton;
        private Button cancelButton;
        private GroupBox goalsGroupBox;
        private Label infoLabel;

        public decimal ActivityGoal { get; private set; }
        public decimal CalorieGoal { get; private set; }
        public decimal SleepGoal { get; private set; }

        public GoalsForm()
        {
            InitializeComponents();
        }

        public GoalsForm(HealthGoals currentGoals)
        {
            InitializeComponents();

            if (currentGoals != null)
            {
                activityGoalTextBox.Text = currentGoals.DailyActivityGoalMinutes.ToString();
                calorieGoalTextBox.Text = currentGoals.DailyCalorieGoal.ToString();
                sleepGoalTextBox.Text = currentGoals.DailySleepGoalHours.ToString();
            }
        }


        private void InitializeComponents()
        {
          

            this.Text = "Установка целей здоровья";
            this.Width = 400;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            goalsGroupBox = new GroupBox();
            goalsGroupBox.Text = "Мои цели";
            goalsGroupBox.Location = new System.Drawing.Point(12, 12);
            goalsGroupBox.Size = new System.Drawing.Size(360, 230);
            goalsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);

            activityGoalLabel = new Label();
            activityGoalLabel.Text = "Цель по активности (минут в день):";
            activityGoalLabel.Location = new System.Drawing.Point(10, 35);
            activityGoalLabel.Size = new System.Drawing.Size(320, 25);
            activityGoalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);

            activityGoalTextBox = new TextBox();
            activityGoalTextBox.Location = new System.Drawing.Point(10, 65);
            activityGoalTextBox.Size = new System.Drawing.Size(330, 25);
            activityGoalTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            activityGoalTextBox.Text = "0";
            activityGoalTextBox.Name = "activityGoalTextBox";

            calorieGoalLabel = new Label();
            calorieGoalLabel.Text = "Цель по калориям (калорий в день):";
            calorieGoalLabel.Location = new System.Drawing.Point(10, 100);
            calorieGoalLabel.Size = new System.Drawing.Size(320, 25);
            calorieGoalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);

            calorieGoalTextBox = new TextBox();
            calorieGoalTextBox.Location = new System.Drawing.Point(10, 130);
            calorieGoalTextBox.Size = new System.Drawing.Size(330, 25);
            calorieGoalTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            calorieGoalTextBox.Text = "0";
            calorieGoalTextBox.Name = "calorieGoalTextBox";

            sleepGoalLabel = new Label();
            sleepGoalLabel.Text = "Цель по сну (часов в день):";
            sleepGoalLabel.Location = new System.Drawing.Point(10, 165);
            sleepGoalLabel.Size = new System.Drawing.Size(320, 25);
            sleepGoalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);

            sleepGoalTextBox = new TextBox();
            sleepGoalTextBox.Location = new System.Drawing.Point(10, 195);
            sleepGoalTextBox.Size = new System.Drawing.Size(330, 25);
            sleepGoalTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            sleepGoalTextBox.Text = "0";
            sleepGoalTextBox.Name = "sleepGoalTextBox";

            goalsGroupBox.Controls.Add(activityGoalLabel);
            goalsGroupBox.Controls.Add(activityGoalTextBox);
            goalsGroupBox.Controls.Add(calorieGoalLabel);
            goalsGroupBox.Controls.Add(calorieGoalTextBox);
            goalsGroupBox.Controls.Add(sleepGoalLabel);
            goalsGroupBox.Controls.Add(sleepGoalTextBox);

            infoLabel = new Label();
            infoLabel.Text = "Подсказка: Рекомендуемые значения - 30-60 минут активности, 2000-2500 калорий, 7-8 часов сна.";
            infoLabel.Location = new System.Drawing.Point(12, 250);
            infoLabel.Size = new System.Drawing.Size(360, 40);
            infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            infoLabel.ForeColor = System.Drawing.Color.Gray;

            okButton = new Button();
            okButton.Text = "Сохранить цели";
            okButton.Location = new System.Drawing.Point(12, 300);
            okButton.Size = new System.Drawing.Size(120, 30);
            okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            okButton.Click += OkButton_Click;

            cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location = new System.Drawing.Point(252, 300);
            cancelButton.Size = new System.Drawing.Size(120, 30);
            cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            cancelButton.Click += CancelButton_Click;

            this.Controls.Add(goalsGroupBox);
            this.Controls.Add(infoLabel);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(activityGoalTextBox.Text, out decimal actGoal))
            {
                MessageBox.Show("Пожалуйста, введите корректное числовое значение для цели по активности.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                activityGoalTextBox.Focus();
                return;
            }

            if (!decimal.TryParse(calorieGoalTextBox.Text, out decimal calGoal))
            {
                MessageBox.Show("Пожалуйста, введите корректное числовое значение для цели по калориям.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                calorieGoalTextBox.Focus();
                return;
            }

            if (!decimal.TryParse(sleepGoalTextBox.Text, out decimal slpGoal))
            {
                MessageBox.Show("Пожалуйста, введите корректное числовое значение для цели по сну.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sleepGoalTextBox.Focus();
                return;
            }

            if (actGoal < 0 || calGoal < 0 || slpGoal < 0)
            {
                MessageBox.Show("Цели не могут быть отрицательными числами. Пожалуйста, введите неотрицательные значения.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (actGoal == 0 && calGoal == 0 && slpGoal == 0)
            {
                DialogResult result = MessageBox.Show("Вы установили все цели в ноль. Это отключит рекомендации. Продолжить?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            ActivityGoal = actGoal;
            CalorieGoal = calGoal;
            SleepGoal = slpGoal;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
