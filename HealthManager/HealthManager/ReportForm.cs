using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HealthManager
{
    public partial class ReportForm : Form
    {
        private Dictionary<string, decimal> activityTracking = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> nutritionTracking = new Dictionary<string, decimal>();
        private Dictionary<string, decimal> sleepTracking = new Dictionary<string, decimal>();

        private TableLayoutPanel mainTableLayout;
        private TabControl tabControl;
        private TabPage dataTab;
        private TabPage chartTab;
        private RichTextBox dataRichTextBox;
        private Panel chartPanel;
        private Panel legendPanel;
        private Panel recommendationsPanel;
        private FlowLayoutPanel buttonPanel;
        private Button exportButton;
        private Button refreshButton;
        private Button closeButton;

        public Dictionary<string, decimal> ActivityTracking
        {
            set { activityTracking = value != null ? value : new Dictionary<string, decimal>(); }
        }

        public Dictionary<string, decimal> NutritionTracking
        {
            set { nutritionTracking = value != null ? value : new Dictionary<string, decimal>(); }
        }

        public Dictionary<string, decimal> SleepTracking
        {
            set { sleepTracking = value != null ? value : new Dictionary<string, decimal>(); }
        }

        public HealthGoals Goals { get; set; }

        public ReportForm()
        {
            InitializeReportComponents();
            this.Resize += ReportForm_Resize;
        }

        private void InitializeReportComponents()
        {
            this.Text = "Отчёт по здоровью";
            this.MinimumSize = new Size(800, 600);
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.WhiteSmoke;

            // Главный TableLayoutPanel
            mainTableLayout = new TableLayoutPanel();
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Padding = new Padding(10);
            mainTableLayout.RowCount = 2;
            mainTableLayout.ColumnCount = 1;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));

            // TabControl
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Вкладка с данными
            dataTab = new TabPage("ДАННЫЕ");
            dataTab.Font = new Font("Segoe UI", 9F);
            dataTab.BackColor = Color.White;

            dataRichTextBox = new RichTextBox();
            dataRichTextBox.Dock = DockStyle.Fill;
            dataRichTextBox.ReadOnly = true;
            dataRichTextBox.Font = new Font("Consolas", 10F);
            dataRichTextBox.BackColor = Color.White;
            dataRichTextBox.BorderStyle = BorderStyle.None;

            dataTab.Controls.Add(dataRichTextBox);

            // Вкладка с диаграммой
            chartTab = new TabPage("ДИАГРАММА ПРОГРЕССА");
            chartTab.Font = new Font("Segoe UI", 9F);
            chartTab.BackColor = Color.White;

            // Panel для диаграммы
            chartPanel = new Panel();
            chartPanel.Dock = DockStyle.Fill;
            chartPanel.BackColor = Color.White;
            chartPanel.Paint += ChartPanel_Paint;
            chartPanel.Resize += ChartPanel_Resize;

            chartTab.Controls.Add(chartPanel);

            tabControl.Controls.Add(dataTab);
            tabControl.Controls.Add(chartTab);

            // Панель с кнопками
            buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Padding = new Padding(0, 10, 0, 10);
            buttonPanel.BackColor = Color.WhiteSmoke;

            closeButton = new Button();
            closeButton.Text = "Закрыть";
            closeButton.Size = new Size(100, 32);
            closeButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            closeButton.BackColor = SystemColors.Control;
            closeButton.ForeColor = SystemColors.ControlText;
            closeButton.FlatStyle = FlatStyle.Standard;
            closeButton.Cursor = Cursors.Hand;
            closeButton.Click += (s, e) => this.Close();

            refreshButton = new Button();
            refreshButton.Text = "Обновить";
            refreshButton.Size = new Size(100, 32);
            refreshButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            refreshButton.BackColor = SystemColors.Control;
            refreshButton.ForeColor = SystemColors.ControlText;
            refreshButton.FlatStyle = FlatStyle.Standard;
            refreshButton.Cursor = Cursors.Hand;
            refreshButton.Click += RefreshButton_Click;

            /*exportButton = new Button();
            exportButton.Text = "Экспорт";
            exportButton.Size = new Size(100, 32);
            exportButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            exportButton.BackColor = SystemColors.Control;
            exportButton.ForeColor = SystemColors.ControlText;
            exportButton.FlatStyle = FlatStyle.Standard;
            exportButton.Cursor = Cursors.Hand;
            exportButton.Click += ExportButton_Click;*/

            buttonPanel.Controls.Add(closeButton);
            buttonPanel.Controls.Add(refreshButton);
            buttonPanel.Controls.Add(exportButton);

            mainTableLayout.Controls.Add(tabControl, 0, 0);
            mainTableLayout.Controls.Add(buttonPanel, 0, 1);

            this.Controls.Add(mainTableLayout);
        }

        private void ReportForm_Resize(object sender, EventArgs e)
        {
            if (chartPanel != null)
                chartPanel.Invalidate();
        }

        private void ChartPanel_Resize(object sender, EventArgs e)
        {
            chartPanel.Invalidate();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == chartTab && chartPanel != null)
            {
                chartPanel.Invalidate();
            }
        }

        private void ChartPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (Goals == null || (Goals.DailyActivityGoalMinutes == 0 && Goals.DailyCalorieGoal == 0 && Goals.DailySleepGoalHours == 0))
            {
                DrawNoGoalsMessage(g);
                return;
            }

            // Получаем размеры панели
            int panelWidth = chartPanel.ClientSize.Width;
            int panelHeight = chartPanel.ClientSize.Height;

            if (panelWidth < 100 || panelHeight < 100) return;

            // Рассчитываем прогресс
            decimal activityProgress = Goals.GetActivityProgressPercentage();
            decimal calorieProgress = Goals.GetCalorieProgressPercentage();
            decimal sleepProgress = Goals.GetSleepProgressPercentage();

            // Заголовок
            DrawTitle(g, panelWidth);

            // Определяем область для диаграммы
            Rectangle chartArea = new Rectangle(50, 70, panelWidth - 250, 300);
            if (panelWidth < 700) chartArea = new Rectangle(20, 70, panelWidth - 200, 280);

            // Рисуем диаграмму
            DrawBarChart(g, chartArea, activityProgress, calorieProgress, sleepProgress);

            // Рисуем легенду
            DrawLegend(g, panelWidth, chartArea.Y);

            // Рисуем рекомендации
            DrawRecommendations(g, panelWidth, panelHeight);
        }

        private void DrawTitle(Graphics g, int panelWidth)
        {
            using (Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold))
            using (Font subtitleFont = new Font("Segoe UI", 11, FontStyle.Regular))
            {
                string title = "ПРОГРЕСС ВЫПОЛНЕНИЯ ЦЕЛЕЙ";
                SizeF titleSize = g.MeasureString(title, titleFont);
                float titleX = (panelWidth - titleSize.Width) / 2;

                g.DrawString(title, titleFont, Brushes.DarkSlateGray, titleX, 15);
                g.DrawString("На основе текущих данных", subtitleFont, Brushes.Gray, titleX + titleSize.Width / 2 - 100, 48);
            }
        }

        private void DrawBarChart(Graphics g, Rectangle area, decimal activityProgress, decimal calorieProgress, decimal sleepProgress)
        {
            int barWidth = (area.Width - 100) / 3;
            int startX = area.X + 20;
            int startY = area.Y;
            int maxBarHeight = area.Height - 80;
            int barSpacing = 30;

            string[] barNames = { "Активность", "Питание", "Сон" };
            decimal[] progressValues = { activityProgress, calorieProgress, sleepProgress };
            Color[] barColors = { Color.FromArgb(52, 152, 219), Color.FromArgb(46, 204, 113), Color.FromArgb(241, 196, 15) };
            string[] goalTexts = {
                $"{Goals.DailyActivityGoalMinutes:F0} мин/день",
                $"{Goals.DailyCalorieGoal:F0} кал/день",
                $"{Goals.DailySleepGoalHours:F1} ч/день"
            };
            string[] currentTexts = {
                $"{Goals.TotalActivityMinutes:F0} мин",
                $"{Goals.TotalCalories:F0} кал",
                $"{Goals.TotalSleepHours:F1} ч"
            };

            for (int i = 0; i < 3; i++)
            {
                int barX = startX + i * (barWidth + barSpacing);
                int barHeight = (int)((progressValues[i] / 100) * maxBarHeight);
                if (barHeight < 5 && progressValues[i] > 0) barHeight = 5;

                // Фон столбца
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(240, 240, 240)))
                {
                    g.FillRectangle(bgBrush, barX, startY + maxBarHeight - barHeight, barWidth, barHeight);
                }

                // Заполненная часть
                using (Brush brush = new SolidBrush(barColors[i]))
                {
                    g.FillRectangle(brush, barX, startY + maxBarHeight - barHeight, barWidth, barHeight);
                }

                // Обводка
                g.DrawRectangle(Pens.DarkGray, barX, startY, barWidth, maxBarHeight);

                // Название
                using (Font nameFont = new Font("Segoe UI", 10, FontStyle.Bold))
                {
                    g.DrawString(barNames[i], nameFont, Brushes.Black, barX + 5, startY + maxBarHeight + 8);
                }

                // Цель и текущее значение
                using (Font smallFont = new Font("Segoe UI", 8))
                {
                    g.DrawString($"Цель: {goalTexts[i]}", smallFont, Brushes.DimGray, barX + 5, startY + maxBarHeight + 30);
                    g.DrawString($"Текущий: {currentTexts[i]}", smallFont, Brushes.DimGray, barX + 5, startY + maxBarHeight + 48);
                }

                // Процент на столбце
                using (Font percentFont = new Font("Segoe UI", 11, FontStyle.Bold))
                {
                    string percentText = $"{progressValues[i]:F0}%";
                    SizeF textSize = g.MeasureString(percentText, percentFont);
                    float percentX = barX + (barWidth - textSize.Width) / 2;
                    float percentY = startY + maxBarHeight - barHeight - 22;

                    if (progressValues[i] < 30)
                        percentY = startY + maxBarHeight - barHeight + 8;

                    g.DrawString(percentText, percentFont, Brushes.White, percentX, percentY);
                }
            }
        }

        private void DrawLegend(Graphics g, int panelWidth, int chartY)
        {
            int legendX = panelWidth - 180;
            int legendY = chartY + 20;

            using (Font legendTitleFont = new Font("Segoe UI", 10, FontStyle.Bold))
            using (Font legendFont = new Font("Segoe UI", 9))
            {
                g.DrawString("Легенда", legendTitleFont, Brushes.DarkSlateGray, legendX, legendY);

                string[] legendItems = { "Активность", "Питание", "Сон" };
                Color[] legendColors = { Color.FromArgb(52, 152, 219), Color.FromArgb(46, 204, 113), Color.FromArgb(241, 196, 15) };

                for (int i = 0; i < 3; i++)
                {
                    using (Brush brush = new SolidBrush(legendColors[i]))
                    {
                        g.FillRectangle(brush, legendX, legendY + 28 + i * 25, 18, 14);
                    }
                    g.DrawRectangle(Pens.DarkGray, legendX, legendY + 28 + i * 25, 18, 14);
                    g.DrawString(legendItems[i], legendFont, Brushes.Black, legendX + 24, legendY + 26 + i * 25);
                }
            }
        }

        private void DrawRecommendations(Graphics g, int panelWidth, int panelHeight)
        {
            int recY = panelHeight - 140;
            if (recY < 380) recY = 380;

            using (Font recTitleFont = new Font("Segoe UI", 12, FontStyle.Bold))
            using (Font recFont = new Font("Segoe UI", 9))
            {
                g.DrawString("Рекомендации", recTitleFont, Brushes.DarkRed, 50, recY);

                var recommendations = Goals.GetRecommendations();
                int recLineY = recY + 30;

                foreach (var rec in recommendations)
                {
                    string wrappedText = WrapText(rec, panelWidth - 100, recFont, g);
                    foreach (string line in wrappedText.Split('\n'))
                    {
                        g.DrawString($"• {line}", recFont, Brushes.Black, 50, recLineY);
                        recLineY += 22;
                    }
                }
            }
        }

        private string WrapText(string text, int maxWidth, Font font, Graphics g)
        {
            string result = "";
            string[] words = text.Split(' ');
            string line = "";

            foreach (string word in words)
            {
                string testLine = line + (line == "" ? "" : " ") + word;
                SizeF size = g.MeasureString(testLine, font);

                if (size.Width > maxWidth)
                {
                    result += line + "\n";
                    line = word;
                }
                else
                {
                    line = testLine;
                }
            }
            result += line;
            return result;
        }

        private void DrawNoGoalsMessage(Graphics g)
        {
            using (Font titleFont = new Font("Segoe UI", 16, FontStyle.Bold))
            using (Font messageFont = new Font("Segoe UI", 11, FontStyle.Regular))
            using (Font hintFont = new Font("Segoe UI", 9))
            {
                string message = "Цели не установлены";
                SizeF messageSize = g.MeasureString(message, titleFont);
                float x = (chartPanel.Width - messageSize.Width) / 2;
                float y = chartPanel.Height / 2 - 60;

                g.DrawString(message, titleFont, Brushes.Gray, x, y);
                g.DrawString("Чтобы получить рекомендации и видеть прогресс:", messageFont, Brushes.DimGray, x - 120, y + 40);
                g.DrawString("1. Нажмите кнопку 'Установить цели' на главном окне", hintFont, Brushes.Gray, x - 120, y + 75);
                g.DrawString("2. Введите свои цели по активности, питанию и сну", hintFont, Brushes.Gray, x - 120, y + 100);
                g.DrawString("3. Сохраните цели и вернитесь к этому отчёту", hintFont, Brushes.Gray, x - 120, y + 125);
            }
        }

        public void GenerateReport()
        {
            dataRichTextBox.Clear();

            decimal totalActivity = 0;
            decimal totalCalories = 0;
            decimal totalSleep = 0;

            foreach (var act in activityTracking) totalActivity += act.Value;
            foreach (var nut in nutritionTracking) totalCalories += nut.Value;
            foreach (var slp in sleepTracking) totalSleep += slp.Value;

            if (Goals != null)
            {
                Goals.TotalActivityMinutes = totalActivity;
                Goals.TotalCalories = totalCalories;
                Goals.TotalSleepHours = totalSleep;
            }

            dataRichTextBox.AppendText("                              ОТЧЁТ ПО ЗДОРОВЬЮ                              \n");
       

            if (activityTracking.Count > 0)
            {
             
                dataRichTextBox.AppendText("                            АКТИВНОСТЬ                                    \n");

                foreach (var activity in activityTracking)
                {
                    dataRichTextBox.AppendText($"  {activity.Key.PadRight(40)} : {activity.Value,8:F0} минут                    \n");
                }
                dataRichTextBox.AppendText($"  ИТОГО за все дни{new string(' ', 32)} : {totalActivity,8:F0} минут                    \n");
            }

            if (nutritionTracking.Count > 0)
            {
                dataRichTextBox.AppendText("                            ПИТАНИЕ                                     \n");

                foreach (var food in nutritionTracking)
                {
                    dataRichTextBox.AppendText($"  {food.Key.PadRight(40)} : {food.Value,8:F0} калорий                  \n");
                }
                dataRichTextBox.AppendText($"  ИТОГО за все дни{new string(' ', 32)} : {totalCalories,8:F0} калорий                  │n");
            }

            if (sleepTracking.Count > 0)
            {
                dataRichTextBox.AppendText("                             СОН                                       \n");

                foreach (var sleep in sleepTracking)
                {
                    dataRichTextBox.AppendText($"│  {sleep.Key.PadRight(40)} : {sleep.Value,8:F1} часов                     \n");
                }
                dataRichTextBox.AppendText($"│  ИТОГО за все дни{new string(' ', 32)} : {totalSleep,8:F1} часов                     \n");
            }

            if (activityTracking.Count == 0 && nutritionTracking.Count == 0 && sleepTracking.Count == 0)
            {
                dataRichTextBox.AppendText("\n\n");
                dataRichTextBox.AppendText("                         ДАННЫЕ ОТСУТСТВУЮТ                                 \n");
                dataRichTextBox.AppendText("              Добавьте информацию о здоровье, чтобы увидеть отчёт            \n");
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.FileName = $"HealthReport_{DateTime.Now:yyyy-MM-dd_HH-mm}.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string reportContent = dataRichTextBox.Text;

                    // Добавляем информацию о целях
                    if (Goals != null)
                    {
                        reportContent += "\n\n";
                        reportContent += "=== ЦЕЛИ ===\n";
                        reportContent += $"Активность: {Goals.DailyActivityGoalMinutes} минут в день\n";
                        reportContent += $"Питание: {Goals.DailyCalorieGoal} калорий в день\n";
                        reportContent += $"Сон: {Goals.DailySleepGoalHours} часов в день\n\n";
                        reportContent += "=== ПРОГРЕСС ===\n";
                        reportContent += $"Активность: {Goals.GetActivityProgressPercentage():F0}%\n";
                        reportContent += $"Питание: {Goals.GetCalorieProgressPercentage():F0}%\n";
                        reportContent += $"Сон: {Goals.GetSleepProgressPercentage():F0}%\n\n";
                        reportContent += "=== РЕКОМЕНДАЦИИ ===\n";
                        foreach (var rec in Goals.GetRecommendations())
                        {
                            reportContent += $"• {rec}\n";
                        }
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, reportContent, System.Text.Encoding.UTF8);
                    MessageBox.Show($"Отчёт успешно сохранён в файл:\n{saveFileDialog.FileName}",
                        "Экспорт отчёта", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            GenerateReport();
            if (chartPanel != null)
                chartPanel.Invalidate();
            MessageBox.Show("Отчёт обновлён!", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}