using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HealthManager
{
    public class HealthManager
    {
        private Dictionary<string, decimal> activityTracking = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> nutritionTracking = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> sleepTracking = new Dictionary<string, decimal>();
        private HealthGoals healthGoals;

        public HealthManager()
        {
            healthGoals = new HealthGoals();
        }

        public void TrackActivity(string activityType, decimal duration)
        {
            if (string.IsNullOrWhiteSpace(activityType))
            {
                MessageBox.Show("Тип активности не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (duration < 0)
            {
                MessageBox.Show("Продолжительность не может быть отрицательной.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (activityTracking.ContainsKey(activityType))
            {
                activityTracking[activityType] += duration;
            }
            else
            {
                activityTracking.Add(activityType, duration);
            }
            MessageBox.Show($"Активность '{activityType}' отслежена на {duration} минут.",
                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void TrackNutrition(string foodItem, decimal calories)
        {
            if (string.IsNullOrWhiteSpace(foodItem))
            {
                MessageBox.Show("Название пищи не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (calories < 0)
            {
                MessageBox.Show("Калории не могут быть отрицательными.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (nutritionTracking.ContainsKey(foodItem))
            {
                nutritionTracking[foodItem] += calories;
            }
            else
            {
                nutritionTracking.Add(foodItem, calories);
            }
            MessageBox.Show($"Пища '{foodItem}' отслежена: {calories} калорий.",
                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void TrackSleep(string date, decimal hours)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                MessageBox.Show("Дата не может быть пустой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (hours < 0)
            {
                MessageBox.Show("Количество часов сна не может быть отрицательным.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sleepTracking.ContainsKey(date))
            {
                sleepTracking[date] += hours;
            }
            else
            {
                sleepTracking.Add(date, hours);
            }
            MessageBox.Show($"Сон на {date} отслежен: {hours} часов.",
                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SetGoals(decimal activityGoal, decimal calorieGoal, decimal sleepGoal)
        {
            healthGoals.DailyActivityGoalMinutes = activityGoal;
            healthGoals.DailyCalorieGoal = calorieGoal;
            healthGoals.DailySleepGoalHours = sleepGoal;
            MessageBox.Show("Цели успешно установлены! Теперь вы можете видеть прогресс и рекомендации в отчёте.",
                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public HealthGoals GetGoals()
        {
            return healthGoals;
        }

        public void DisplayActivityReport()
        {
            var reportForm = new ReportForm();
            reportForm.ActivityTracking = activityTracking;
            reportForm.NutritionTracking = nutritionTracking;
            reportForm.SleepTracking = sleepTracking;
            reportForm.Goals = healthGoals;
            reportForm.GenerateReport();
           
            reportForm.ShowDialog();
        }

        public Dictionary<string, decimal> GetActivityTracking()
        {
            return new Dictionary<string, decimal>(activityTracking);
        }

        public Dictionary<string, decimal> GetNutritionTracking()
        {
            return new Dictionary<string, decimal>(nutritionTracking);
        }

        public Dictionary<string, decimal> GetSleepTracking()
        {
            return new Dictionary<string, decimal>(sleepTracking);
        }

        public void ClearAllData()
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить все данные?\nЭто действие нельзя отменить.",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                activityTracking.Clear();
                nutritionTracking.Clear();
                sleepTracking.Clear();
                healthGoals = new HealthGoals();
                MessageBox.Show("Все данные успешно удалены.", "Очистка данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}