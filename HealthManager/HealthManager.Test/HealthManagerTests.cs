using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HealthManager;

namespace HealthManager.Tests
{
    [TestClass]
    public class HealthManagerTests
    {
        //  ТЕСТЫ ДЛЯ АКТИВНОСТИ 

        [TestMethod]
        public void TrackActivity_SameActivity_ShouldSumDuration()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.TrackActivity("Бег", 30);
            manager.TrackActivity("Бег", 20);
            var activities = manager.GetActivityTracking();

            // Assert
            Assert.AreEqual(50, activities["Бег"]);
        }

   
        [TestMethod]
        public void TrackActivity_WithNegativeDuration_ShouldNotAdd()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.TrackActivity("Бег", -30);
            var activities = manager.GetActivityTracking();

            // Assert
            Assert.AreEqual(0, activities.Count);
        }

    

        // ТЕСТЫ ДЛЯ ПИТАНИЯ 


        [TestMethod]
        public void TrackNutrition_SameFood_ShouldSumCalories()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.TrackNutrition("Яблоко", 50);
            manager.TrackNutrition("Яблоко", 30);
            var nutrition = manager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(80, nutrition["Яблоко"]);
        }

    
       

        // ТЕСТЫ ДЛЯ СНА 

        [TestMethod]
        public void TrackSleep_ShouldAddNewSleep()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.TrackSleep("01.01.2024", 8);
            var sleep = manager.GetSleepTracking();

            // Assert
            Assert.IsTrue(sleep.ContainsKey("01.01.2024"));
            Assert.AreEqual(8, sleep["01.01.2024"]);
        }

        [TestMethod]
        public void TrackSleep_SameDate_ShouldSumHours()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.TrackSleep("01.01.2024", 6);
            manager.TrackSleep("01.01.2024", 2);
            var sleep = manager.GetSleepTracking();

            // Assert
            Assert.AreEqual(8, sleep["01.01.2024"]);
        }


        [TestMethod]
        public void TrackSleep_WithNegativeHours_ShouldNotAdd()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.TrackSleep("01.01.2024", -8);
            var sleep = manager.GetSleepTracking();

            // Assert
            Assert.AreEqual(0, sleep.Count);
        }

      

        // ТЕСТЫ ДЛЯ ЦЕЛЕЙ

        [TestMethod]
        public void SetGoals_ShouldUpdateGoals()
        {
            // Arrange
            var manager = new HealthManager();

            // Act
            manager.SetGoals(60, 2000, 8);
            var goals = manager.GetGoals();

            // Assert
            Assert.AreEqual(60, goals.DailyActivityGoalMinutes);
            Assert.AreEqual(2000, goals.DailyCalorieGoal);
            Assert.AreEqual(8, goals.DailySleepGoalHours);
        }


        
    }
}