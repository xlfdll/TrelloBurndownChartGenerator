using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TrelloBurndownChartGenerator
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Int32 sprint = 3;
            Int32 sprintLength = 9;
            DateTime startDate = new DateTime(2017, 5, 9);

            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Trello JSON (*.json)|*.json|All Files (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                JObject json = JObject.Parse(File.ReadAllText(dlg.FileName, Encoding.UTF8));

                Double totalTime = (from c in json["cards"]
                                   select c).Sum(c => c["labels"].Sum(l => Int32.Parse(l["name"].Value<String>())));

                Double[] sprintProgress = GetSprintProgress(json, startDate, sprintLength, totalTime);

                DrawBurndownChart(sprint, startDate, totalTime, sprintProgress);
            }
        }

        private static Double[] GetSprintProgress(JObject json, DateTime startDate, Int32 days, Double totalTime)
        {
            Double[] sprintProgress = new Double[days + 1];
            sprintProgress[0] = totalTime;

            for (Int32 i = 1; i < sprintProgress.Length; i++)
            {
                Double totalDoneTime = 0;
                DateTime targetDate = startDate.AddDays(i);

                if (targetDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    targetDate.AddDays(2);
                }
                else if (targetDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    targetDate.AddDays(1);
                }

                var doneCards = from a in json["actions"]
                                where a["type"].Value<String>() == "updateCard"
                                && a["date"].Value<DateTime>() < targetDate.AddDays(2)
                                && a["data"]["listAfter"] != null && a["data"]["listAfter"]["name"].Value<String>() == "Done"
                                select a["data"]["card"]["id"].Value<String>();

                foreach (String card in doneCards)
                {
                    totalDoneTime += (from c in json["cards"]
                                      where c["labels"].HasValues && c["id"].Value<String>() == card
                                      select c).Sum(c => c["labels"].Sum(l => Int32.Parse(l["name"].Value<String>())));
                }

                sprintProgress[i] = totalDoneTime;
            }

            return sprintProgress;
        }

        private static void DrawBurndownChart(Int32 sprint, DateTime startDate, Double totalTime, Double[] sprintProgress)
        {
            PlotModel plotModel = new PlotModel()
            {
                Title = $"Burndown Chart - Sprint {sprint} (murcS)",
                TitleColor = OxyColors.DarkBlue,
                Subtitle = $"From {startDate.ToShortDateString()} to {startDate.AddDays(sprintProgress.Length - 1).ToShortDateString()}",
                SubtitleColor = OxyColors.DarkRed
            };

            LinearAxis xAxis = new LinearAxis()
            {
                Title = "Sprint Day",
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = sprintProgress.Length - 1,
                MajorStep = 1,
                IsPanEnabled = false,
                IsZoomEnabled = false
            };
            LinearAxis yAxis = new LinearAxis()
            {
                Title = "Task Remaining Time",
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = totalTime,
                MajorStep = 2,
                IsPanEnabled = false,
                IsZoomEnabled = false,
            };

            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            LineSeries estimateSeries = new LineSeries()
            {
                Title = "Estimated Progress"
            };
            LineSeries actualSeries = new LineSeries()
            {
                Title = "Actual Progress"
            };

            for (Int32 i = 0; i < sprintProgress.Length; i++)
            {
                estimateSeries.Points.Add(new DataPoint(i, totalTime - (totalTime / (sprintProgress.Length - 1)) * i));
                actualSeries.Points.Add(new DataPoint(i, i != 0 ? totalTime - sprintProgress[i] : totalTime));
            }

            plotModel.Series.Add(estimateSeries);
            plotModel.Series.Add(actualSeries);

            PlotWindow plotWindow = new PlotWindow(plotModel);

            plotWindow.ShowDialog();
        }
    }
}