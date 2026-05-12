using System;
using System.Windows.Forms;

namespace HealthManager
{
    public partial class HealthForm : Form
    {
        private HealthManager healthManager;
        private Button trackActivityButton;
        private Button trackNutritionButton;
        private Button trackSleepButton;
        private Button displayReportButton;
        private Button setGoalsButton;
        private Button clearDataButton;
        private Label titleLabel;
        private GroupBox mainGroupBox;

        public HealthForm()
        {
            InitializeFormComponents(); 
            healthManager = new HealthManager();
        }

        private void InitializeFormComponents() 
        {
            this.Text = "Управление здоровьем - Персональный трекер";
            this.Width = 350;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "healthForm";

            titleLabel = new Label();
            titleLabel.Text = "Управление здоровьем";
            titleLabel.Location = new System.Drawing.Point(10, 10);
            titleLabel.Size = new System.Drawing.Size(310, 30);
            titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            titleLabel.Name = "titleLabel";

            mainGroupBox = new GroupBox();
            mainGroupBox.Text = "Выберите действие";
            mainGroupBox.Location = new System.Drawing.Point(10, 50);
            mainGroupBox.Size = new System.Drawing.Size(310, 260);
            mainGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            mainGroupBox.Name = "mainGroupBox";

            trackActivityButton = new Button();
            trackActivityButton.Location = new System.Drawing.Point(15, 30);
            trackActivityButton.Text = "Отслеживать активность";
            trackActivityButton.Size = new System.Drawing.Size(280, 35);
            trackActivityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
           
            trackActivityButton.FlatStyle = FlatStyle.Flat;
            trackActivityButton.Name = "trackActivityButton";
            trackActivityButton.Click += TrackActivityButton_Click;

            trackSleepButton = new Button();
            trackSleepButton.Location = new System.Drawing.Point(15, 75);
            trackSleepButton.Text = "Отслеживать сон";
            trackSleepButton.Size = new System.Drawing.Size(280, 35);
            trackSleepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            
            trackSleepButton.FlatStyle = FlatStyle.Flat;
            trackSleepButton.Name = "trackSleepButton";
            trackSleepButton.Click += TrackSleepButton_Click;

            trackNutritionButton = new Button();
            trackNutritionButton.Location = new System.Drawing.Point(15, 120);
            trackNutritionButton.Text = "Отслеживать питание";
            trackNutritionButton.Size = new System.Drawing.Size(280, 35);
            trackNutritionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
           
            trackNutritionButton.FlatStyle = FlatStyle.Flat;
            trackNutritionButton.Name = "trackNutritionButton";
            trackNutritionButton.Click += TrackNutritionButton_Click;

            setGoalsButton = new Button();
            setGoalsButton.Location = new System.Drawing.Point(15, 165);
            setGoalsButton.Text = "Установить цели";
            setGoalsButton.Size = new System.Drawing.Size(280, 35);
            setGoalsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
           
            setGoalsButton.FlatStyle = FlatStyle.Flat;
            setGoalsButton.Name = "setGoalsButton";
            setGoalsButton.Click += SetGoalsButton_Click;

            displayReportButton = new Button();
            displayReportButton.Location = new System.Drawing.Point(15, 210);
            displayReportButton.Text = "Показать отчёт";
            displayReportButton.Size = new System.Drawing.Size(280, 35);
            displayReportButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
           
            displayReportButton.FlatStyle = FlatStyle.Flat;
            displayReportButton.Name = "displayReportButton";
            displayReportButton.Click += DisplayReportButton_Click;

            clearDataButton = new Button();
            clearDataButton.Location = new System.Drawing.Point(220, 320);
            clearDataButton.Text = "Очистить данные";
            clearDataButton.Size = new System.Drawing.Size(100, 30);
            clearDataButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
          
            clearDataButton.FlatStyle = FlatStyle.Flat;
            clearDataButton.Name = "clearDataButton";
            clearDataButton.Click += ClearDataButton_Click;

            mainGroupBox.Controls.Add(trackActivityButton);
            mainGroupBox.Controls.Add(trackSleepButton);
            mainGroupBox.Controls.Add(trackNutritionButton);
            mainGroupBox.Controls.Add(setGoalsButton);
            mainGroupBox.Controls.Add(displayReportButton);

            this.Controls.Add(titleLabel);
            this.Controls.Add(mainGroupBox);
            this.Controls.Add(clearDataButton);
        }

        private void TrackActivityButton_Click(object sender, EventArgs e)
        {
            var activityForm = new ActivityForm();
            if (activityForm.ShowDialog() == DialogResult.OK)
            {
                healthManager.TrackActivity(activityForm.ActivityType, activityForm.Duration);
            }
        }

        private void TrackSleepButton_Click(object sender, EventArgs e)
        {
            var sleepForm = new SleepForm();
            if (sleepForm.ShowDialog() == DialogResult.OK)
            {
                healthManager.TrackSleep(sleepForm.Date, sleepForm.Hours);
            }
        }

        private void TrackNutritionButton_Click(object sender, EventArgs e)
        {
            var nutritionForm = new NutritionForm();
            if (nutritionForm.ShowDialog() == DialogResult.OK)
            {
                healthManager.TrackNutrition(nutritionForm.FoodItem, nutritionForm.Calories);
            }
        }

        private void SetGoalsButton_Click(object sender, EventArgs e)
        {
            var goalsForm = new GoalsForm(healthManager.GetGoals());
            if (goalsForm.ShowDialog() == DialogResult.OK)
            {
                healthManager.SetGoals(goalsForm.ActivityGoal, goalsForm.CalorieGoal, goalsForm.SleepGoal);
            }
        }

        private void DisplayReportButton_Click(object sender, EventArgs e)
        {
            healthManager.DisplayActivityReport();
        }

        private void ClearDataButton_Click(object sender, EventArgs e)
        {
            healthManager.ClearAllData();
        }
    }
}