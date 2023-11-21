using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project_management.enums.methods;
using Project_management.objects;

namespace Project_management
{
    public class GanttCanvas : Canvas
    {
        public List<Task>? Tasks { get; set; }
        private Dictionary<int, DateTime>? _taskStartDates;

        public DateTime ProjectStartDate { get; set; }
        public Project? Project { get; set; }

        protected override void OnRender(DrawingContext dc)
        {
            _taskStartDates = new Dictionary<int, DateTime>();
            base.OnRender(dc);
            CalculateStartDates();
            UpdateCanvasSize();
            DrawTasks(dc);
            DrawTimeAxis(dc);
            DrawBorderLine(dc);
        }

        private void CalculateStartDates()
        {
            var rootTasks = Tasks?.Where(t => t?.Parent == null || t.Parent.Id == 0);
            if (rootTasks == null) return;
            foreach (var task in rootTasks)
            {
                CalculateStartDate(task, ProjectStartDate);
            }
        }

        private void CalculateStartDate(Task task, DateTime startDate)
        {
            _taskStartDates[task.Id] = startDate;
            var childTasks = Tasks?.Where(t => t.Parent != null && t.Parent.Id == task.Id);
            if (childTasks == null) return;
            foreach (var childTask in childTasks)
            {
                CalculateStartDate(childTask, startDate.AddDays(task.Duration));
            }
        }

        private void UpdateCanvasSize()
        {
            double maxWidth = 90;
            double maxHeight = 50;
            Tasks.ToList().ForEach(task =>
            {
                maxWidth += task.Duration * 20;
                maxHeight += 40;
            });
            Width = maxWidth > 1063 ? maxWidth : 1063;
            Height = maxHeight > 510 ? maxHeight : 510;
        }

        private void DrawTasks(DrawingContext dc)
        {
            double yOffset = 50;
            foreach (var task in Tasks.Select((x, i) => new { Value = x, Index = i }))
            {
                if (_taskStartDates == null) return;
                if (!_taskStartDates.ContainsKey(task.Value.Id)) return;
                var daysFromProjectStart = (_taskStartDates[task.Value.Id] - ProjectStartDate).Days;
                var xOffset = task.Index == 0 ? 80 : 80 + daysFromProjectStart * 20;
                var pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                dc.DrawText(
                    new FormattedText(
                        task.Value.Title,
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black,
                        pixelsPerDip
                    ), new Point(5, yOffset));
                var taskRect = new Rect(xOffset, yOffset, task.Value.Duration * 20, 30);
                var taskColor = TaskTypeMethodes.GetColor(task.Value);
                Console.WriteLine(taskColor.ToString());
                dc.DrawRectangle(taskColor, new Pen(Brushes.Black, 1), taskRect);
                yOffset += 40;
            }
        }

        private void DrawTimeAxis(DrawingContext dc)
        {
            var summe = 70;
            var d = Project.GetCompleteTaskDuration();
            if (d > int.MaxValue)
            {
                throw new InvalidOperationException("Die Aufgabendauer überschreitet den maximalen Wert für Int32.");
            }

            for (var i = 1; i <= d; i++)
            {
                summe += 20;
                var pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
                dc.DrawText(
                    new FormattedText(
                        i.ToString(),
                        System.Globalization.CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black,
                        pixelsPerDip
                    ), new Point(summe, 10));
            }
        }

        private static Brush GetTaskColor(string taskTitle)
        {
            switch (taskTitle)
            {
                case "Analyse": return Brushes.LightBlue;
                case "Planung": return Brushes.LightGreen;
                case "Modul 1": return Brushes.Orange;
                case "Design 1": return Brushes.Yellow;
                case "Modul 2": return Brushes.Gray;
                case "Testphase": return Brushes.Pink;
                default: return Brushes.Blue;
            }
        }

        private static void DrawBorderLine(DrawingContext dc)
        {
            //X-Achse
            dc.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 40), new Point(1000 * 10000, 40));

            //Y-Achse
            dc.DrawLine(new Pen(Brushes.Black, 1), new Point(75, 0), new Point(75, 1000 * 10000));
        }
    }
}