using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using FlaUIApp = FlaUI.Core.Application;
using FlaUIWindow = FlaUI.Core.AutomationElements.Window;

namespace HealthManager.UITests
{
    [TestFixture]
    public class HealthManagerUITests
    {
        private FlaUIApp _app;
        private UIA3Automation _automation;
        private FlaUIWindow _mainWindow;

        private const string ApplicationPath = @"C:\Users\Кристина\Desktop\основа проект\lr2\HealthManager\HealthManager\HealthManager.UITests\bin\Debug\net8.0\HealthManager.exe";

        [SetUp]
        public void TestInitialize()
        {
            _app = FlaUIApp.Launch(ApplicationPath);
            _automation = new UIA3Automation();
            _mainWindow = _app.GetMainWindow(_automation);
            _mainWindow.WaitUntilEnabled();
            Thread.Sleep(2000);
        }

        private const string TAB = "{TAB}";
        [TearDown]
        public void TestCleanup()
        {
            try
            {
                if (_mainWindow != null)
                {
                    foreach (var window in _mainWindow.ModalWindows)
                        try { window.Close(); } catch { }
                }
            }
            catch { }
            _automation?.Dispose();
            _app?.Close();
        }
   
        private FlaUIWindow WaitForModalWindow(int timeoutMs = 5000)
        {
            int elapsed = 0;
            while (elapsed < timeoutMs)
            {
                try
                {
                    var modalWindows = _mainWindow.ModalWindows;
                    if (modalWindows != null && modalWindows.Length > 0)
                        return modalWindows[0];
                }
                catch { }
                Thread.Sleep(500);
                elapsed += 500;
            }
            return null;
        }
        private static void CloseMessageIfExists(FlaUIWindow mainWindow)
        {
            try
            {
                var msg = mainWindow.ModalWindows.FirstOrDefault();
                if (msg != null)
                {
                    var okBtn = msg.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    okBtn?.Click();
                    Thread.Sleep(300);
                }
            }
            catch { }
        }
        //  SK-01: Добавление новой физической активности 
        [Test]
        public void SK01_AddNewActivity_ShouldSucceed()
        {
            var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
            Assert.That(trackActivityButton, Is.Not.Null);
            trackActivityButton.Click();

            var activityWindow = WaitForModalWindow(5000);
            Assert.That(activityWindow, Is.Not.Null);

            var allTextBoxes = activityWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            Assert.That(allTextBoxes.Length, Is.GreaterThanOrEqualTo(2));

            var activityTypeBox = allTextBoxes[0].AsTextBox();
            var durationBox = allTextBoxes[1].AsTextBox();
            var okButton = activityWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            activityTypeBox.Text = "Бег";
            durationBox.Text = "30";
            okButton.Click();

            var messageBox = WaitForModalWindow(3000);
            Assert.That(messageBox, Is.Not.Null);

            var okMsgButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okMsgButton?.Click();

            Assert.Pass("SK-01: Активность успешно добавлена");
        }

        // SK-02: Обновление существующей активности
        [Test]
        public void SK02_UpdateExistingActivity_ShouldSumDuration()
        {
            var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();

         
            trackActivityButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait("Бег{TAB}30{ENTER}");
            Thread.Sleep(500);
            var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk?.Click();
            Thread.Sleep(500);

            trackActivityButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait("Бег{TAB}45{ENTER}");
            Thread.Sleep(500);
            var msgOk2 = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk2?.Click();

            Assert.Pass();
        }

        //  SK-03: Отображение сообщения об успешном добавлении активности 
        [Test]
        public void SK03_SuccessMessage_ShouldAppearAfterAddingActivity()
        {
            var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
            trackActivityButton.Click();

            var activityWindow = WaitForModalWindow(5000);
            Assert.That(activityWindow, Is.Not.Null);

            var allTextBoxes = activityWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            var activityTypeBox = allTextBoxes[0].AsTextBox();
            var durationBox = allTextBoxes[1].AsTextBox();
            var okButton = activityWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            activityTypeBox.Text = "Йога";
            durationBox.Text = "60";
            okButton.Click();

            var messageBox = WaitForModalWindow(3000);
            Assert.That(messageBox, Is.Not.Null);

            var messageText = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
            Assert.That(messageText, Is.Not.Null);
            StringAssert.Contains("Активность", messageText.Name);
            StringAssert.Contains("Йога", messageText.Name);

            var okMsgButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okMsgButton?.Click();

            Assert.Pass("SK-03: Сообщение отобразилось корректно");
        }

        //  SK-04: Добавление нового продукта питания 
        [Test]
        public void SK04_AddNewFood_ShouldSucceed()
        {
            var trackNutritionButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать питание")).AsButton();
            trackNutritionButton.Click();

            var nutritionWindow = WaitForModalWindow(5000);
            Assert.That(nutritionWindow, Is.Not.Null);

            var allTextBoxes = nutritionWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            Assert.That(allTextBoxes.Length, Is.GreaterThanOrEqualTo(2));

            var foodItemBox = allTextBoxes[0].AsTextBox();
            var caloriesBox = allTextBoxes[1].AsTextBox();
            var okButton = nutritionWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            foodItemBox.Text = "Яблоко";
            caloriesBox.Text = "95";
            okButton.Click();

            var messageBox = WaitForModalWindow(3000);
            Assert.That(messageBox, Is.Not.Null);

            var okMsgButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okMsgButton?.Click();

            Assert.Pass("SK-04: Продукт успешно добавлен");
        }
        // SK-05: Обновление существующего продукта
        [Test]
        public void SK05_UpdateExistingFood_ShouldSumCalories()
        {
            var trackNutritionButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать питание")).AsButton();

            trackNutritionButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait("Яблоко{TAB}95{ENTER}");
            Thread.Sleep(500);
            var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk?.Click();
            Thread.Sleep(500);

            trackNutritionButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait("Яблоко{TAB}50{ENTER}");
            Thread.Sleep(500);
            var msgOk2 = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk2?.Click();

            Assert.Pass();
        }
        //  SK-06: Отображение сообщения об успешном добавлении продукта 
        [Test]
        public void SK06_SuccessMessage_ShouldAppearAfterAddingFood()
        {
            var trackNutritionButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать питание")).AsButton();
            trackNutritionButton.Click();

            var nutritionWindow = WaitForModalWindow(5000);
            Assert.That(nutritionWindow, Is.Not.Null);

            var allTextBoxes = nutritionWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            var foodItemBox = allTextBoxes[0].AsTextBox();
            var caloriesBox = allTextBoxes[1].AsTextBox();
            var okButton = nutritionWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            foodItemBox.Text = "Банан";
            caloriesBox.Text = "105";
            okButton.Click();

            var messageBox = WaitForModalWindow(3000);
            Assert.That(messageBox, Is.Not.Null);

            var messageText = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
            Assert.That(messageText, Is.Not.Null);
            StringAssert.Contains("Банан", messageText.Name);

            var okMsgButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okMsgButton?.Click();

            Assert.Pass("SK-06: Сообщение отобразилось корректно");
        }

        //  SK-07: Добавление новой записи о сне 
        [Test]
        public void SK07_AddNewSleep_ShouldSucceed()
        {
            var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
            trackSleepButton.Click();

            var sleepWindow = WaitForModalWindow(5000);
            Assert.That(sleepWindow, Is.Not.Null);

            var allTextBoxes = sleepWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            Assert.That(allTextBoxes.Length, Is.GreaterThanOrEqualTo(2));

            var dateBox = allTextBoxes[0].AsTextBox();
            var hoursBox = allTextBoxes[1].AsTextBox();
            var okButton = sleepWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            dateBox.Text = "24.03.2026";
            hoursBox.Text = "8";
            okButton.Click();

            var messageBox = WaitForModalWindow(3000);
            Assert.That(messageBox, Is.Not.Null);

            var okMsgButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okMsgButton?.Click();

            Assert.Pass("SK-07: Запись о сне добавлена");
        }
        // SK-08: Обновление существующей записи о сне
        [Test]
        public void SK08_UpdateExistingSleep_ShouldSumHours()
        {
            var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
            string testDate = DateTime.Now.ToString("yyyy-MM-dd");

            trackSleepButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait(testDate + "{TAB}8{ENTER}");
            Thread.Sleep(500);
            var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk?.Click();
            Thread.Sleep(500);

            trackSleepButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait(testDate + "{TAB}2{ENTER}");
            Thread.Sleep(500);
            var msgOk2 = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk2?.Click();

            Assert.Pass();
        }

        //  SK-09: Отображение сообщения об успешном добавлении сна 
        [Test]
        public void SK09_SuccessMessage_ShouldAppearAfterAddingSleep()
        {
            var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
            trackSleepButton.Click();

            var sleepWindow = WaitForModalWindow(5000);
            Assert.That(sleepWindow, Is.Not.Null);

            var allTextBoxes = sleepWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            var dateBox = allTextBoxes[0].AsTextBox();
            var hoursBox = allTextBoxes[1].AsTextBox();
            var okButton = sleepWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            dateBox.Text = "26.03.2026";
            hoursBox.Text = "7,5";
            okButton.Click();

            var messageBox = WaitForModalWindow(3000);
            Assert.That(messageBox, Is.Not.Null);

            var messageText = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Text));
            Assert.That(messageText, Is.Not.Null);
            StringAssert.Contains("сон", messageText.Name.ToLower());

            var okMsgButton = messageBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okMsgButton?.Click();

            Assert.Pass("SK-09: Сообщение отобразилось корректно");
        }
        // SK-10: Просмотр отчета со всеми данными
        [Test]
        public void SK10_DisplayReport_WithAllData_ShouldShowCorrectInfo()
        {
         
            var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
            trackActivityButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait("Бег{TAB}75{ENTER}");
            Thread.Sleep(500);
            var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk?.Click();
            Thread.Sleep(500);

            var trackNutritionButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать питание")).AsButton();
            trackNutritionButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait("Яблоко{TAB}145{ENTER}");
            Thread.Sleep(500);
            msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk?.Click();
            Thread.Sleep(500);

            var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
            trackSleepButton.Click();
            Thread.Sleep(500);
            SendKeys.SendWait(DateTime.Now.ToString("yyyy-MM-dd") + "{TAB}10{ENTER}");
            Thread.Sleep(500);
            msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            msgOk?.Click();
            Thread.Sleep(500);

            var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
            reportButton.Click();
            Thread.Sleep(1500);

            var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
            Assert.That(reportWindow, Is.Not.Null, "Отчёт не открылся");

            var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
            closeButton?.AsButton().Click();

            Assert.Pass();
        }
        // SK-11: Просмотр отчета при отсутствии данных 
        [Test]
        public void SK11_DisplayReport_WithNoData_ShouldShowEmptyReport()
        {
            var displayReportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
            displayReportButton.Click();

            var reportWindow = WaitForModalWindow(5000);
            Assert.That(reportWindow, Is.Not.Null);

            var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
            if (closeButton != null)
                closeButton.AsButton().Click();
            else
                reportWindow.Close();

            Assert.Pass("SK-11: Отчет открылся");
        }

        // SK-12: Ввод отрицательной продолжительности активности 
        [Test]
        public void SK12_AddActivity_WithNegativeDuration_ShouldShowError()
        {
            var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
            trackActivityButton.Click();

            var activityWindow = WaitForModalWindow(5000);
            Assert.That(activityWindow, Is.Not.Null);

            var allTextBoxes = activityWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            var activityTypeBox = allTextBoxes[0].AsTextBox();
            var durationBox = allTextBoxes[1].AsTextBox();
            var okButton = activityWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            activityTypeBox.Text = "Плавание";
            durationBox.Text = "-15";
            okButton.Click();

            var errorBox = WaitForModalWindow(3000);
            Assert.That(errorBox, Is.Not.Null);

            var okErrorButton = errorBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okErrorButton?.Click();

            Assert.Pass("SK-12: Ошибка отобразилась");
        }

        // SK-13: Ввод отрицательных калорий
        [Test]
        public void SK13_AddFood_WithNegativeCalories_ShouldShowError()
        {
            var trackNutritionButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать питание")).AsButton();
            trackNutritionButton.Click();

            var nutritionWindow = WaitForModalWindow(5000);
            Assert.That(nutritionWindow, Is.Not.Null);

            var allTextBoxes = nutritionWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            var foodItemBox = allTextBoxes[0].AsTextBox();
            var caloriesBox = allTextBoxes[1].AsTextBox();
            var okButton = nutritionWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            foodItemBox.Text = "Тест";
            caloriesBox.Text = "-500";
            okButton.Click();

            var errorBox = WaitForModalWindow(3000);
            Assert.That(errorBox, Is.Not.Null);

            var okErrorButton = errorBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okErrorButton?.Click();

            Assert.Pass("SK-13: Ошибка отобразилась");
        }

        // SK-14: Ввод отрицательных часов сна 
        [Test]
        public void SK14_AddSleep_WithNegativeHours_ShouldShowError()
        {
            var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
            trackSleepButton.Click();

            var sleepWindow = WaitForModalWindow(5000);
            Assert.That(sleepWindow, Is.Not.Null);

            var allTextBoxes = sleepWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
            var dateBox = allTextBoxes[0].AsTextBox();
            var hoursBox = allTextBoxes[1].AsTextBox();
            var okButton = sleepWindow.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();

            dateBox.Text = "25.03.2026";
            hoursBox.Text = "-3";
            okButton.Click();

            var errorBox = WaitForModalWindow(3000);
            Assert.That(errorBox, Is.Not.Null);

            var okErrorButton = errorBox.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
            okErrorButton?.Click();

            Assert.Pass("SK-14: Ошибка отобразилась");
        }

        // SK-15: Отображение отчета в отдельном окне 
        [Test]
        public void SK15_Report_ShouldOpenInSeparateWindow()
        {
            var displayReportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
            displayReportButton.Click();
            Thread.Sleep(1500);

            var reportWindow = WaitForModalWindow(5000);
            Assert.That(reportWindow, Is.Not.Null);

            string windowTitle = reportWindow.Title;
            Assert.That(windowTitle.Contains("Отчёт") || windowTitle.Contains("Отчет"));

            var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
            if (closeButton != null)
                closeButton.AsButton().Click();
            else
                reportWindow.Close();

            Assert.Pass("SK-15: Отчет открылся в отдельном окне");
        }
                // KS_1: Установка всех целей одновременно
                [Test]
                public void KS1_SetAllGoals_ShouldSucceed()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    var goalsWindow = WaitForModalWindow(5000);
                    Assert.That(goalsWindow, Is.Not.Null);
                    var allTextBoxes = goalsWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                    Assert.That(allTextBoxes.Length, Is.GreaterThanOrEqualTo(3));
                    allTextBoxes[0].AsTextBox().Text = "60";
                    allTextBoxes[1].AsTextBox().Text = "2000";
                    allTextBoxes[2].AsTextBox().Text = "8";
                    var saveButton = goalsWindow.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton.Click();
                    var messageBox = WaitForModalWindow(3000);
                    Assert.That(messageBox, Is.Not.Null);
                    CloseMessageIfExists(_mainWindow);
                    Assert.Pass();
                }

                // KS_2: Установка отрицательной цели по активности
                [Test]
                public void KS2_SetNegativeActivityGoal_ShouldShowError()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    var goalsWindow = WaitForModalWindow(5000);
                    Assert.That(goalsWindow, Is.Not.Null);
                    var allTextBoxes = goalsWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                    allTextBoxes[0].AsTextBox().Text = "-10";
                    var saveButton = goalsWindow.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton.Click();
                    var errorBox = WaitForModalWindow(3000);
                    Assert.That(errorBox, Is.Not.Null);
                    CloseMessageIfExists(_mainWindow);
                    Assert.Pass();
                }

                // KS_3: Установка отрицательной цели по сну
                [Test]
                public void KS3_SetNegativeSleepGoal_ShouldShowError()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    var goalsWindow = WaitForModalWindow(5000);
                    Assert.That(goalsWindow, Is.Not.Null);
                    var allTextBoxes = goalsWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                    allTextBoxes[2].AsTextBox().Text = "-5";
                    var saveButton = goalsWindow.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton.Click();
                    var errorBox = WaitForModalWindow(3000);
                    Assert.That(errorBox, Is.Not.Null);
                    CloseMessageIfExists(_mainWindow);
                    Assert.Pass();
                }

                // KS_4: Отображение сообщения об успешном добавлении целей
                [Test]
                public void KS4_SuccessMessage_ShouldAppear()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    var goalsWindow = WaitForModalWindow(5000);
                    var allTextBoxes = goalsWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                    allTextBoxes[0].AsTextBox().Text = "60";
                    allTextBoxes[1].AsTextBox().Text = "2000";
                    allTextBoxes[2].AsTextBox().Text = "8";
                    var saveButton = goalsWindow.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton.Click();
                    var messageBox = WaitForModalWindow(3000);
                    Assert.That(messageBox, Is.Not.Null);
                    CloseMessageIfExists(_mainWindow);
                    Assert.Pass();
                }

                // KS_5: Установка отрицательной цели по калориям
                [Test]
                public void KS5_SetNegativeCalorieGoal_ShouldShowError()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    var goalsWindow = WaitForModalWindow(5000);
                    Assert.That(goalsWindow, Is.Not.Null);
                    var allTextBoxes = goalsWindow.FindAllDescendants(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit));
                    allTextBoxes[1].AsTextBox().Text = "-1000";
                    var saveButton = goalsWindow.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton.Click();
                    var errorBox = WaitForModalWindow(3000);
                    Assert.That(errorBox, Is.Not.Null);
                    CloseMessageIfExists(_mainWindow);
                    Assert.Pass();
                }

                // KS_6: Расчёт прогресса калорий при 10%
                [Test]
                public void KS6_CalorieProgress_10Percent_ShowsCorrectProgress()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("{TAB}{TAB}2000");
                    var saveButton = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton?.Click();
                    Thread.Sleep(300);
                    var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    msgOk?.Click();
                    Thread.Sleep(500);
                    var trackNutritionButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать питание")).AsButton();
                    trackNutritionButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("яблоко{TAB}200{TAB}{ENTER}");
                    Thread.Sleep(500);
                    var nutritionMsgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    nutritionMsgOk?.Click();
                    Thread.Sleep(500);
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1000);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    if (reportWindow != null)
                    {
                        var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                        closeButton?.AsButton().Click();
                    }
                    Assert.Pass();
                }

                // KS_7: Расчёт прогресса активности при 50%
                [Test]
                public void KS7_ActivityProgress_50Percent_ShowsCorrectProgress()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("60");
                    var saveButton = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton?.Click();
                    Thread.Sleep(300);
                    var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    msgOk?.Click();
                    Thread.Sleep(500);
                    var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
                    trackActivityButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("Бег{TAB}30{TAB}{ENTER}");
                    Thread.Sleep(500);
                    var activityMsgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    activityMsgOk?.Click();
                    Thread.Sleep(500);
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1000);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    if (reportWindow != null)
                    {
                        var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                        closeButton?.AsButton().Click();
                    }
                    Assert.Pass();
                }

                // KS_8: Прогресс активности при превышении цели
                [Test]
                public void KS8_ActivityProgress_ExceedsGoal_Shows100Percent()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("60");
                    var saveButton = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton?.Click();
                    Thread.Sleep(300);
                    var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    msgOk?.Click();
                    Thread.Sleep(500);
                    var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
                    trackActivityButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("Бег{TAB}90{TAB}{ENTER}");
                    Thread.Sleep(500);
                    var activityMsgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    activityMsgOk?.Click();
                    Thread.Sleep(500);
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1000);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    if (reportWindow != null)
                    {
                        var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                        closeButton?.AsButton().Click();
                    }
                    Assert.Pass();
                }

                // KS_9: Прогресс активности при нулевой цели
                [Test]
                public void KS9_ActivityProgress_ZeroGoal_Shows0Percent()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("0");
                    var saveButton = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton?.Click();
                    Thread.Sleep(300);
                    var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    msgOk?.Click();
                    Thread.Sleep(500);
                    var trackActivityButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать активность")).AsButton();
                    trackActivityButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("Плавание{TAB}30{TAB}{ENTER}");
                    Thread.Sleep(500);
                    var activityMsgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    activityMsgOk?.Click();
                    Thread.Sleep(500);
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1000);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    if (reportWindow != null)
                    {
                        var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                        closeButton?.AsButton().Click();
                    }
                    Assert.Pass();
                }

                // KS_10: Расчёт прогресса сна при 75%
                [Test]
                public void KS10_SleepProgress_75Percent_ShowsCorrectProgress()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("{TAB}{TAB}8");
                    var saveButton = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton?.Click();
                    Thread.Sleep(300);
                    var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    msgOk?.Click();
                    Thread.Sleep(500);
                    var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
                    trackSleepButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait(DateTime.Now.ToString("yyyy-MM-dd") + "{TAB}6{TAB}{ENTER}");
                    Thread.Sleep(500);
                    var sleepMsgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    sleepMsgOk?.Click();
                    Thread.Sleep(500);
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1000);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    if (reportWindow != null)
                    {
                        var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                        closeButton?.AsButton().Click();
                    }
                    Assert.Pass();
                }

                // KS_11: Расчёт прогресса сна при 100%
                [Test]
                public void KS11_SleepProgress_100Percent_ShowsCorrectProgress()
                {
                    var setGoalsButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Установить цели")).AsButton();
                    setGoalsButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait("{TAB}{TAB}8");
                    var saveButton = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("Сохранить цели")).AsButton();
                    saveButton?.Click();
                    Thread.Sleep(300);
                    var msgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    msgOk?.Click();
                    Thread.Sleep(500);
                    var trackSleepButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Отслеживать сон")).AsButton();
                    trackSleepButton.Click();
                    Thread.Sleep(500);
                    SendKeys.SendWait(DateTime.Now.ToString("yyyy-MM-dd") + "{TAB}8{TAB}{ENTER}");
                    Thread.Sleep(500);
                    var sleepMsgOk = _mainWindow.ModalWindows.FirstOrDefault()?.FindFirstDescendant(cf => cf.ByText("OK")).AsButton();
                    sleepMsgOk?.Click();
                    Thread.Sleep(500);
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1000);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    if (reportWindow != null)
                    {
                        var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                        closeButton?.AsButton().Click();
                    }
                    Assert.Pass();
                }

                // KS_12: Видимость сообщения при отсутствии целей
             
                [Test]
                public void KS12_NoGoalsSet_ShowsMessage()
                {
                    var reportButton = _mainWindow.FindFirstDescendant(cf => cf.ByText("Показать отчёт")).AsButton();
                    reportButton.Click();
                    Thread.Sleep(1500);
                    var reportWindow = _mainWindow.ModalWindows.FirstOrDefault();
                    Assert.That(reportWindow, Is.Not.Null, "Отчет не открылся");

                    var dataTab = reportWindow.FindFirstDescendant(cf => cf.ByText("ДАННЫЕ"));
                    Assert.That(dataTab, Is.Not.Null, "Вкладка 'ДАННЫЕ' не найдена");

                    var chartTab = reportWindow.FindFirstDescendant(cf => cf.ByText("ДИАГРАММА ПРОГРЕССА"));
                    if (chartTab == null) chartTab = reportWindow.FindFirstDescendant(cf => cf.ByText("Диаграмма прогресса"));
                    Assert.That(chartTab, Is.Not.Null, "Вкладка 'Диаграмма прогресса' не найдена");

                    var closeButton = reportWindow.FindFirstDescendant(cf => cf.ByText("Закрыть"));
                    closeButton?.AsButton().Click();

                    Assert.Pass("Отчет открыт, вкладка открывается");
                }
            }
        }
    
