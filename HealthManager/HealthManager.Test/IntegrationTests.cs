/*using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HealthManager;

namespace HealthManager.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private HealthManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _manager = new HealthManager();
        }

         

               // ТЕСТ ОБНОВЛЕНИЯ ЦЕЛЕЙ 

        [TestMethod]
        public void Goals_CanBeUpdated()
        {
            // Arrange
            _manager.SetGoals(30, 1000, 6);

            // Act
            _manager.SetGoals(60, 2000, 8);
            var goals = _manager.GetGoals();

            // Assert
            Assert.AreEqual(60, goals.DailyActivityGoalMinutes);
            Assert.AreEqual(2000, goals.DailyCalorieGoal);
            Assert.AreEqual(8, goals.DailySleepGoalHours);
        }

        //ТЕСТ СУММИРОВАНИЯ АКТИВНОСТЕЙ 

        [TestMethod]
        public void ActivitySummation_WithMultipleEntries_WorksCorrectly()
        {
            // Act
            _manager.TrackActivity("Бег", 15);
            _manager.TrackActivity("Бег", 25);
            _manager.TrackActivity("Бег", 10);

            var activities = _manager.GetActivityTracking();

            // Assert
            Assert.AreEqual(50, activities["Бег"]);
        }

        // ТЕСТ СУММИРОВАНИЯ ПРОДУКТОВ 

        [TestMethod]
        public void NutritionSummation_WithMultipleEntries_WorksCorrectly()
        {
            // Act
            _manager.TrackNutrition("Яблоко", 30);
            _manager.TrackNutrition("Яблоко", 20);
            _manager.TrackNutrition("Яблоко", 25);

            var nutrition = _manager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(75, nutrition["Яблоко"]);
        }

        //  ТЕСТ СУММИРОВАНИЯ СНА 

        [TestMethod]
        public void SleepSummation_WithMultipleEntries_WorksCorrectly()
        {
            // Act
            _manager.TrackSleep("01.01.2024", 4);
            _manager.TrackSleep("01.01.2024", 3);
            _manager.TrackSleep("01.01.2024", 2);

            var sleep = _manager.GetSleepTracking();

            // Assert
            Assert.AreEqual(9, sleep["01.01.2024"]);
        }
    }
}*/