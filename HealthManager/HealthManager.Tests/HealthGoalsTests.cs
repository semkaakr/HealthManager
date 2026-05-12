using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HealthManager;
namespace HealthManager.Tests
{
    [TestClass]
    public class HealthGoalsTests
    {
        // ==================== ТЕСТЫ ДЛЯ УСТАНОВКИ ЦЕЛЕЙ ====================

        [TestMethod]
        public void SetActivityGoal_ShouldUpdateActivityGoal()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailyActivityGoalMinutes = 60;

            // Assert
            Assert.AreEqual(60, goals.DailyActivityGoalMinutes);
        }

        [TestMethod]
        public void SetCalorieGoal_ShouldUpdateCalorieGoal()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailyCalorieGoal = 2000;

            // Assert
            Assert.AreEqual(2000, goals.DailyCalorieGoal);
        }

        [TestMethod]
        public void SetSleepGoal_ShouldUpdateSleepGoal()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailySleepGoalHours = 8;

            // Assert
            Assert.AreEqual(8, goals.DailySleepGoalHours);
        }

        [TestMethod]
        public void SetAllGoals_ShouldUpdateAllGoals()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailyActivityGoalMinutes = 60;
            goals.DailyCalorieGoal = 2000;
            goals.DailySleepGoalHours = 8;

            // Assert
            Assert.AreEqual(60, goals.DailyActivityGoalMinutes);
            Assert.AreEqual(2000, goals.DailyCalorieGoal);
            Assert.AreEqual(8, goals.DailySleepGoalHours);
        }

        [TestMethod]
        public void SetNegativeActivityGoal_ShouldSetToZero()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailyActivityGoalMinutes = -10;

            // Assert
            Assert.AreEqual(0, goals.DailyActivityGoalMinutes);
        }

        [TestMethod]
        public void SetNegativeCalorieGoal_ShouldSetToZero()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailyCalorieGoal = -500;

            // Assert
            Assert.AreEqual(0, goals.DailyCalorieGoal);
        }

        [TestMethod]
        public void SetNegativeSleepGoal_ShouldSetToZero()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            goals.DailySleepGoalHours = -5;

            // Assert
            Assert.AreEqual(0, goals.DailySleepGoalHours);
        }

        // ==================== ТЕСТЫ ДЛЯ РАСЧЕТА ПРОГРЕССА ====================

        [TestMethod]
        public void GetActivityProgress_WhenHalfCompleted_Returns50()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 30;

            // Act
            var result = goals.GetActivityProgressPercentage();

            // Assert
            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public void GetActivityProgress_WhenFullyCompleted_Returns100()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 60;

            // Act
            var result = goals.GetActivityProgressPercentage();

            // Assert
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void GetActivityProgress_WhenExceedsGoal_Returns100()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 90;

            // Act
            var result = goals.GetActivityProgressPercentage();

            // Assert
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void GetActivityProgress_WhenNoProgress_Returns0()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 0;

            // Act
            var result = goals.GetActivityProgressPercentage();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetActivityProgress_WhenGoalIsZero_Returns0()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 0;
            goals.TotalActivityMinutes = 30;

            // Act
            var result = goals.GetActivityProgressPercentage();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetCalorieProgress_WhenQuarterCompleted_Returns25()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyCalorieGoal = 2000;
            goals.TotalCalories = 500;

            // Act
            var result = goals.GetCalorieProgressPercentage();

            // Assert
            Assert.AreEqual(25, result);
        }

        [TestMethod]
        public void GetCalorieProgress_WhenHalfCompleted_Returns50()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyCalorieGoal = 2000;
            goals.TotalCalories = 1000;

            // Act
            var result = goals.GetCalorieProgressPercentage();

            // Assert
            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public void GetCalorieProgress_WhenGoalIsZero_Returns0()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyCalorieGoal = 0;
            goals.TotalCalories = 1500;

            // Act
            var result = goals.GetCalorieProgressPercentage();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetSleepProgress_WhenThreeQuartersCompleted_Returns75()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailySleepGoalHours = 8;
            goals.TotalSleepHours = 6;

            // Act
            var result = goals.GetSleepProgressPercentage();

            // Assert
            Assert.AreEqual(75, result);
        }

        [TestMethod]
        public void GetSleepProgress_WhenFullyCompleted_Returns100()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailySleepGoalHours = 8;
            goals.TotalSleepHours = 8;

            // Act
            var result = goals.GetSleepProgressPercentage();

            // Assert
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void GetSleepProgress_WhenGoalIsZero_Returns0()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailySleepGoalHours = 0;
            goals.TotalSleepHours = 7;

            // Act
            var result = goals.GetSleepProgressPercentage();

            // Assert
            Assert.AreEqual(0, result);
        }

        // ==================== ТЕСТЫ ДЛЯ РЕКОМЕНДАЦИЙ ====================

        [TestMethod]
        public void GetRecommendations_WhenActivityGoalNotMet_ReturnsActivityRecommendation()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 30;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasRecommendation = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Активность") && rec.Contains("нужно еще"))
                {
                    hasRecommendation = true;
                    break;
                }
            }
            Assert.IsTrue(hasRecommendation, "Должна быть рекомендация о недостатке активности");
        }

        [TestMethod]
        public void GetRecommendations_WhenActivityGoalMet_ReturnsPositiveMessage()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 65;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasPositive = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Активность") && rec.Contains("Отлично"))
                {
                    hasPositive = true;
                    break;
                }
            }
            Assert.IsTrue(hasPositive, "Должно быть положительное сообщение об активности");
        }

        [TestMethod]
        public void GetRecommendations_WhenCalorieGoalExceeded_ReturnsCalorieRecommendation()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyCalorieGoal = 2000;
            goals.TotalCalories = 2500;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasRecommendation = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Питание") && rec.Contains("превысили"))
                {
                    hasRecommendation = true;
                    break;
                }
            }
            Assert.IsTrue(hasRecommendation, "Должна быть рекомендация о превышении калорий");
        }

        [TestMethod]
        public void GetRecommendations_WhenCalorieGoalUnder_ReturnsDeficitRecommendation()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyCalorieGoal = 2000;
            goals.TotalCalories = 1500;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasRecommendation = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Питание") && rec.Contains("не хватает"))
                {
                    hasRecommendation = true;
                    break;
                }
            }
            Assert.IsTrue(hasRecommendation, "Должна быть рекомендация о недоборе калорий");
        }

        [TestMethod]
        public void GetRecommendations_WhenSleepGoalNotMet_ReturnsSleepRecommendation()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailySleepGoalHours = 8;
            goals.TotalSleepHours = 6;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasRecommendation = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Сон") && rec.Contains("не хватает"))
                {
                    hasRecommendation = true;
                    break;
                }
            }
            Assert.IsTrue(hasRecommendation, "Должна быть рекомендация о недостатке сна");
        }

        [TestMethod]
        public void GetRecommendations_WhenSleepGoalMet_ReturnsPositiveMessage()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailySleepGoalHours = 8;
            goals.TotalSleepHours = 8;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasPositive = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Сон") && rec.Contains("высыпаетесь"))
                {
                    hasPositive = true;
                    break;
                }
            }
            Assert.IsTrue(hasPositive, "Должно быть положительное сообщение о сне");
        }

        [TestMethod]
        public void GetRecommendations_WhenNoGoalsSet_ReturnsSetupMessage()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            bool hasSetupMessage = false;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Установите цели"))
                {
                    hasSetupMessage = true;
                    break;
                }
            }
            Assert.IsTrue(hasSetupMessage, "Должно быть сообщение об установке целей");
        }

        [TestMethod]
        public void GetRecommendations_WhenAllGoalsNotMet_ReturnsThreeRecommendations()
        {
            // Arrange
            var goals = new HealthGoals();
            goals.DailyActivityGoalMinutes = 60;
            goals.TotalActivityMinutes = 30;
            goals.DailyCalorieGoal = 2000;
            goals.TotalCalories = 2500;
            goals.DailySleepGoalHours = 8;
            goals.TotalSleepHours = 6;

            // Act
            var recommendations = goals.GetRecommendations();

            // Assert
            int count = 0;
            foreach (var rec in recommendations)
            {
                if (rec.Contains("Активность") || rec.Contains("Питание") || rec.Contains("Сон"))
                    count++;
            }
            Assert.IsTrue(count >= 3, "Должно быть 3 рекомендации");
        }

        // ==================== ТЕСТЫ ДЛЯ ПРОГРЕСС-БАРА ====================

        [TestMethod]
        public void GetProgressBar_WithZeroPercent_ReturnsEmptyBar()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            string result = goals.GetProgressBar(0);

            // Assert
            Assert.IsTrue(result.Contains("░░░░░░░░░░░░░░░░░░░░"));
            Assert.IsTrue(result.Contains("0%"));
        }

        [TestMethod]
        public void GetProgressBar_WithFiftyPercent_ReturnsHalfFilledBar()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            string result = goals.GetProgressBar(50);

            // Assert
            Assert.IsTrue(result.Contains("██████████"));
            Assert.IsTrue(result.Contains("50%"));
        }

        [TestMethod]
        public void GetProgressBar_WithHundredPercent_ReturnsFullBar()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            string result = goals.GetProgressBar(100);

            // Assert
            Assert.IsTrue(result.Contains("████████████████████"));
            Assert.IsTrue(result.Contains("100%"));
        }

        [TestMethod]
        public void GetProgressBar_WithTwentyFivePercent_ReturnsQuarterFilledBar()
        {
            // Arrange
            var goals = new HealthGoals();

            // Act
            string result = goals.GetProgressBar(25);

            // Assert
            int filledCount = 0;
            foreach (char c in result)
            {
                if (c == '█') filledCount++;
                if (c == '░') break;
            }
            Assert.AreEqual(5, filledCount);
        }
    }
}