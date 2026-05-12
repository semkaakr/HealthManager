using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager
{
    public class HealthGoals
    {
        private decimal dailyActivityGoalMinutes;
        private decimal dailyCalorieGoal;
        private decimal dailySleepGoalHours;
        private decimal totalActivityMinutes;
        private decimal totalCalories;
        private decimal totalSleepHours;

        public decimal DailyActivityGoalMinutes
        {
            get { return dailyActivityGoalMinutes; }
            set { dailyActivityGoalMinutes = value >= 0 ? value : 0; }
        }

        public decimal DailyCalorieGoal
        {
            get { return dailyCalorieGoal; }
            set { dailyCalorieGoal = value >= 0 ? value : 0; }
        }

        public decimal DailySleepGoalHours
        {
            get { return dailySleepGoalHours; }
            set { dailySleepGoalHours = value >= 0 ? value : 0; }
        }

        public decimal TotalActivityMinutes
        {
            get { return totalActivityMinutes; }
            set { totalActivityMinutes = value >= 0 ? value : 0; }
        }

        public decimal TotalCalories
        {
            get { return totalCalories; }
            set { totalCalories = value >= 0 ? value : 0; }
        }

        public decimal TotalSleepHours
        {
            get { return totalSleepHours; }
            set { totalSleepHours = value >= 0 ? value : 0; }
        }

        public HealthGoals()
        {
            dailyActivityGoalMinutes = 0;
            dailyCalorieGoal = 0;
            dailySleepGoalHours = 0;
            totalActivityMinutes = 0;
            totalCalories = 0;
            totalSleepHours = 0;
        }

        public decimal GetActivityProgressPercentage()
        {
            if (dailyActivityGoalMinutes <= 0) return 0;
            decimal percentage = (totalActivityMinutes / dailyActivityGoalMinutes) * 100;
            return Math.Min(100, percentage);
        }

        public decimal GetCalorieProgressPercentage()
        {
            if (dailyCalorieGoal <= 0) return 0;
            decimal percentage = (totalCalories / dailyCalorieGoal) * 100;
            return Math.Min(100, percentage);
        }

        public decimal GetSleepProgressPercentage()
        {
            if (dailySleepGoalHours <= 0) return 0;
            decimal percentage = (totalSleepHours / dailySleepGoalHours) * 100;
            return Math.Min(100, percentage);
        }

        public List<string> GetRecommendations()
        {
            var recommendations = new List<string>();

            if (dailyActivityGoalMinutes > 0)
            {
                if (totalActivityMinutes < dailyActivityGoalMinutes)
                {
                    decimal remaining = dailyActivityGoalMinutes - totalActivityMinutes;
                    recommendations.Add($"АКТИВНОСТЬ: Вы выполнили {GetActivityProgressPercentage():F0}% от цели. " +
                                        $"Вам нужно еще {remaining:F0} минут активности, чтобы достичь цели.");
                }
                else
                {
                    recommendations.Add($"АКТИВНОСТЬ: Отлично! Вы достигли и даже превысили свою цель по активности!");
                }
            }

            if (dailyCalorieGoal > 0)
            {
                if (totalCalories > dailyCalorieGoal)
                {
                    decimal excess = totalCalories - dailyCalorieGoal;
                    recommendations.Add($"ПИТАНИЕ: Вы превысили дневную норму калорий на {excess:F0} калорий. " +
                                        $"Попробуйте выбрать более легкие продукты.");
                }
                else if (totalCalories < dailyCalorieGoal && dailyCalorieGoal > 0)
                {
                    decimal deficit = dailyCalorieGoal - totalCalories;
                    recommendations.Add($"ПИТАНИЕ: Вы недобрали {deficit:F0} калорий. " +
                                        $"Убедитесь, что вы получаете достаточно энергии.");
                }
                else if (totalCalories == dailyCalorieGoal && dailyCalorieGoal > 0)
                {
                    recommendations.Add($"ПИТАНИЕ: Отлично, вы точно укладываетесь в норму калорий!");
                }
            }

            if (dailySleepGoalHours > 0)
            {
                if (totalSleepHours < dailySleepGoalHours)
                {
                    decimal deficit = dailySleepGoalHours - totalSleepHours;
                    recommendations.Add($"СОН: Вам не хватает {deficit:F1} часов сна. " +
                                        $"Постарайтесь лечь спать пораньше.");
                }
                else
                {
                    recommendations.Add($"СОН: Хороший результат! Вы высыпаетесь.");
                }
            }

            if (dailyActivityGoalMinutes == 0 && dailyCalorieGoal == 0 && dailySleepGoalHours == 0)
            {
                recommendations.Add("Установите цели (активность, питание, сон), чтобы получать персонализированные рекомендации.");
            }

            return recommendations;
        }

        public string GetProgressBar(decimal percentage)
        {
            int barLength = 20;
            int filledLength = (int)((percentage / 100) * barLength);
            filledLength = Math.Min(barLength, Math.Max(0, filledLength));

            string progressBar = "[";
            for (int i = 0; i < barLength; i++)
            {
                if (i < filledLength)
                    progressBar += "█";
                else
                    progressBar += "░";
            }
            progressBar += $"] {percentage:F0}%";

            return progressBar;
        }
    }
}