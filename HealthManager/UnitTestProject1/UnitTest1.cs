using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HealthManager.Tests
{
    [TestClass]
    public class HealthManagerTests
    {
        private HealthManager healthManager;

        [TestInitialize]
        public void Setup()
        {
            healthManager = new HealthManager();
        }

        [TestMethod]
        public void TrackActivity_NewAddsActivity()
        {
            // Arrange
            string activityType = "Бег";
            decimal prodoljitelnost = 30;

            // Act
            healthManager.TrackActivity(activityType, prodoljitelnost);
            var activities = healthManager.GetActivityTracking();

            // Assert
            Assert.AreEqual(prodoljitelnost, activities[activityType]);
        }

        [TestMethod]
        public void AddSameActivity_SumtwoActivity()
        {
            // Arrange
            string activityType = "Ходьба";

            // Act
            healthManager.TrackActivity(activityType, 20);
            healthManager.TrackActivity(activityType, 15);
            var activities = healthManager.GetActivityTracking();

            // Assert
            Assert.AreEqual(35, activities[activityType]);
        }

        [TestMethod]
        public void TrackActivity_AddRasnieActivnosti()
        {
            // Arrange
            string activityType1 = "Плавание";
            string activityType2 = "Езда на велосипеде";

            // Act
            healthManager.TrackActivity(activityType1, 45);
            healthManager.TrackActivity(activityType2, 60);
            var activities = healthManager.GetActivityTracking();

            // Assert
            Assert.AreEqual(45, activities[activityType1]);
            Assert.AreEqual(60, activities[activityType2]);
        }

        [TestMethod]
        public void TrackActivity_NegativeTime()
        {
            // Arrange
            string activityType = "Тест";
            decimal duration = -10;

            // Act
            healthManager.TrackActivity(activityType, duration);
            var activities = healthManager.GetActivityTracking();

            // Assert
            Assert.AreEqual(duration, activities[activityType]);
        }

        [TestMethod]
        public void TrackNutrition_AddsFood_NewFood()
        {
            // Arrange
            string foodItem = "Яблоко";
            decimal calories = 95;

            // Act
            healthManager.TrackNutrition(foodItem, calories);
            var nutrition = healthManager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(calories, nutrition[foodItem]);
        }

        [TestMethod]
        public void TrackNutrition_SumCalories()
        {
            // Arrange
            string foodItem = "Банан";

            // Act
            healthManager.TrackNutrition(foodItem, 105);
            healthManager.TrackNutrition(foodItem, 50);
            var nutrition = healthManager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(155, nutrition[foodItem]);
        }

        [TestMethod]
        public void TrackNutrition_AddSomeFood()
        {
            // Arrange
            string foodItem1 = "Апельсин";
            string foodItem2 = "Пицца";

            // Act
            healthManager.TrackNutrition(foodItem1, 62);
            healthManager.TrackNutrition(foodItem2, 285);
            var nutrition = healthManager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(62, nutrition[foodItem1]);
            Assert.AreEqual(285, nutrition[foodItem2]);
        }

        [TestMethod]
        public void TrackNutrition_NegativeCalories()
        {
            // Arrange
            string foodItem = "Diet Food";
            decimal calories = -50;

            // Act
            healthManager.TrackNutrition(foodItem, calories);
            var nutrition = healthManager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(calories, nutrition[foodItem]);
        }

        [TestMethod]
        public void TrackSleep_NewDate_AddsSleep()
        {
            // Arrange
            string date = "2026-03-19";
            decimal hours = 8;

            // Act
            healthManager.TrackSleep(date, hours);
            var sleep = healthManager.GetSleepTracking();

            // Assert
            Assert.AreEqual(hours, sleep[date]);
        }

        [TestMethod]
        public void TrackSleep_Perezapishour()
        {
            // Arrange
            string date = "2026-03-18";

            // Act
            healthManager.TrackSleep(date, 7);
            healthManager.TrackSleep(date, 9);
            var sleep = healthManager.GetSleepTracking();

            // Assert
            Assert.AreEqual(9, sleep[date]);
        }

        [TestMethod]
        public void TrackSleep_AddSleep()
        {
            // Arrange
            string date1 = "2026-03-19";
            string date2 = "2026-03-20";

            // Act
            healthManager.TrackSleep(date1, 8);
            healthManager.TrackSleep(date2, 7.5m);
            var sleep = healthManager.GetSleepTracking();

            // Assert
            Assert.AreEqual(8, sleep[date1]);
            Assert.AreEqual(7.5m, sleep[date2]);
        }

        [TestMethod]
        public void TrackSleep_NegativeHours()
        {
            // Arrange
            string date = "2026-03-21";
            decimal hours = -2;

            // Act
            healthManager.TrackSleep(date, hours);
            var sleep = healthManager.GetSleepTracking();

            // Assert
            Assert.AreEqual(hours, sleep[date]);
        }

        [TestMethod]
        public void TrackSleep_ZeroDate_AddsSleep()
        {
            // Arrange
            string date = "";
            decimal hours = 8;

            // Act
            healthManager.TrackSleep(date, hours);
            var sleep = healthManager.GetSleepTracking();

            // Assert
            Assert.AreEqual(hours, sleep[date]);
        }

        [TestMethod]
        public void TrackActivity_ZeroNameActivity()
        {
            // Arrange
            string activityType = "";
            decimal duration = 30;

            // Act
            healthManager.TrackActivity(activityType, duration);
            var activities = healthManager.GetActivityTracking();

            // Assert
            Assert.AreEqual(duration, activities[activityType]);
        }

        [TestMethod]
        public void TrackNutrition_ZeroNameFood()
        {
            // Arrange
            string foodItem = "";
            decimal calories = 100;

            // Act
            healthManager.TrackNutrition(foodItem, calories);
            var nutrition = healthManager.GetNutritionTracking();

            // Assert
            Assert.AreEqual(calories, nutrition[foodItem]);
        }

        [TestMethod]
        public void DisplayActivityReport_DoesNotThrowException()
        {
            // Arrange
            healthManager.TrackActivity("Бег", 30);
            healthManager.TrackNutrition("Яблоко", 95);
            healthManager.TrackSleep("2026-03-19", 8);

            // Act & Assert
            try
            {
                healthManager.DisplayActivityReport();
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("DisplayActivityReport выбросил исключение");
            }
        }
    }
}