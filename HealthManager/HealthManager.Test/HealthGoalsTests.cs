using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HealthManager;

namespace HealthManager.Tests
{
    [TestClass]
    public class HealthGoalsTests
    {
        //  ТЕСТЫ ДЛЯ УСТАНОВКИ ЦЕЛЕЙ 
                
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

        //  ТЕСТЫ ДЛЯ РАСЧЕТА ПРОГРЕССА 

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
        public void GetActivityProgress_WhenGoalIsZero_Returns0() // Проверяет граничный случай - если цель не установлена (0 минут), прогресс всегда 0, даже если есть данные.
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
            Assert.IsTrue(hasSetupMessage);
        }

       
    }
}