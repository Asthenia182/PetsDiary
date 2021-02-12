using PetsDiary.Common.Models;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PetsDiary.Presentation.Views
{
    /// <summary>
    /// Interaction logic for Weights.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    public partial class WeightsView : UserControl
    {
        //private Line xAxisLine, yAxisLine;
        //private double xAxisStart = 100, yAxisStart = 100, interval = 100;
        //private Polyline chartPolyline;
        //private Point origin;
        //private List<Holder> holders;
        //private List<Value> values;

        //Dictionary<DateTime, double> dateToX;
        //Dictionary<double, double> weightToY;

        

        public WeightsView()
        {
            InitializeComponent();
            //holders = new List<Holder>();
            //values = new List<Value>();

            //var weightModels = new List<WeightModel>()
            //{
            //    new WeightModel() {Id = 1, PetId = 1, Date = DateTime.Today, Weight = 10},
            //    new WeightModel() {Id = 3, PetId = 1, Date = DateTime.Today.AddDays(5), Weight = 7},
            //    new WeightModel() {Id = 2, PetId = 1, Date = DateTime.Today.AddDays(6), Weight = 12},
            //    new WeightModel() {Id = 2, PetId = 1, Date = DateTime.Today.AddDays(10), Weight = 5},
            //};

            /////find x - points
            //dateToX = new Dictionary<DateTime, double>();
            //double xPosition = 0;
            //foreach (var model in weightModels.OrderBy(x => x.Date))
            //{
            //    dateToX.Add(model.Date, xPosition);
            //    xPosition += interval;
            //}

            /////find y - points
            //weightToY = new Dictionary<double, double>();
            //double yPosition = 0;

            //foreach (var model in weightModels.OrderBy(x => x.Weight))
            //{
            //    weightToY.Add(model.Weight, yPosition);
            //    yPosition += interval;
            //}

            //foreach (var weight in weightModels.OrderBy(x => x.Date))
            //{
            //    var x = dateToX[weight.Date];
            //    var y = weightToY[weight.Weight];

            //    values.Add(new Value(x, y, weight.Date, weight.Weight));
            //}

            //Paint();

            //this.SizeChanged += (sender, e) => Paint();
        }

        //public void Paint()
        //{
        //    try
        //    {
        //        if (this.ActualWidth > 0 && this.ActualHeight > 0)
        //        {
        //            chartCanvas.Children.Clear();
        //            holders.Clear();

        //            // axis lines
        //            xAxisLine = new Line()
        //            {
        //                X1 = xAxisStart,
        //                Y1 = this.ActualHeight - yAxisStart,
        //                X2 = this.ActualWidth - xAxisStart,
        //                Y2 = this.ActualHeight - yAxisStart,
        //                Stroke = new SolidColorBrush(Color.FromArgb(255, 166, 204, 182)),
        //                StrokeThickness = 1,
        //            };
        //            yAxisLine = new Line()
        //            {
        //                X1 = xAxisStart,
        //                Y1 = yAxisStart - 50,
        //                X2 = xAxisStart,
        //                Y2 = this.ActualHeight - yAxisStart,
        //                Stroke = new SolidColorBrush(Color.FromArgb(255, 166, 204, 182)),
        //                StrokeThickness = 1,
        //            };

        //            chartCanvas.Children.Add(xAxisLine);
        //            chartCanvas.Children.Add(yAxisLine);

        //            origin = new Point(xAxisLine.X1, yAxisLine.Y2);


        //            var textInX0 = string.Empty;

        //            if (dateToX.ContainsValue(0))
        //            { 
        //                textInX0 = dateToX.FirstOrDefault(x => x.Value == 0).Key.ToString("dd-MM-yy");
        //            }

        //            var xTextBlock0 = new TextBlock() { Text = textInX0 };
        //            chartCanvas.Children.Add(xTextBlock0);
        //            Canvas.SetLeft(xTextBlock0, origin.X);
        //            Canvas.SetTop(xTextBlock0, origin.Y + 5);

        //            // y axis lines
        //            var xValue = xAxisStart;
        //            double xPoint = origin.X + interval;
        //            while (xPoint < xAxisLine.X2)
        //            {
        //                var line = new Line()
        //                {
        //                    X1 = xPoint,
        //                    Y1 = yAxisStart - 50,
        //                    X2 = xPoint,
        //                    Y2 = this.ActualHeight - yAxisStart,
        //                    Stroke = new SolidColorBrush(Color.FromArgb(200, 166, 204, 182)),
        //                    StrokeThickness = 1,
        //                    Opacity = 1,
        //                };

        //                chartCanvas.Children.Add(line);

        //                var textInLineX = string.Empty;

        //                if (dateToX.ContainsValue(xValue)) 
        //                    textInLineX = dateToX.FirstOrDefault(x => x.Value == xValue).Key.ToString("dd-MM-yy") ;

        //                var textBlock = new TextBlock { Text = textInLineX, }; 

        //                chartCanvas.Children.Add(textBlock);
        //                Canvas.SetLeft(textBlock, xPoint - 12.5);
        //                Canvas.SetTop(textBlock, line.Y2 + 5);

        //                xPoint += interval;
        //                xValue += interval;
        //            }

        //            var textInY0 = string.Empty;
        //            if (weightToY.ContainsValue(0))
        //                textInY0 = weightToY.FirstOrDefault(x => x.Value == 0).Key.ToString() + " kg";
        //            var yTextBlock0 = new TextBlock() { Text = textInY0.ToString() };
        //            chartCanvas.Children.Add(yTextBlock0);
        //            Canvas.SetLeft(yTextBlock0, origin.X - 20);
        //            Canvas.SetTop(yTextBlock0, origin.Y - 10);

        //            // x axis lines
        //            var yValue = yAxisStart;
        //            double yPoint = origin.Y - interval;
        //            while (yPoint > yAxisLine.Y1)
        //            {
        //                var line = new Line()
        //                {
        //                    X1 = xAxisStart,
        //                    Y1 = yPoint,
        //                    X2 = this.ActualWidth - xAxisStart,
        //                    Y2 = yPoint,
        //                    Stroke = new SolidColorBrush(Color.FromArgb(160,166, 204, 182)),
        //                    StrokeThickness = 1,
        //                    Opacity = 1,
        //                };

        //                chartCanvas.Children.Add(line);

        //                string textInLineY=string.Empty;

        //                if (weightToY.ContainsValue(yValue))
        //                    textInLineY = weightToY.FirstOrDefault(x => x.Value == yValue).Key.ToString() + " kg";

        //                var textBlock = new TextBlock() { Text = textInLineY }; 
        //                chartCanvas.Children.Add(textBlock);
        //                Canvas.SetLeft(textBlock, line.X1 - 30);
        //                Canvas.SetTop(textBlock, yPoint - 10);

        //                yPoint -= interval;
        //                yValue += interval;
        //            }

        //            // connections
        //            double x = 0, y = 0;
        //            xPoint = origin.X;
        //            yPoint = origin.Y;
        //            while (xPoint < xAxisLine.X2)
        //            {
        //                while (yPoint > yAxisLine.Y1)
        //                {
        //                    var holder = new Holder()
        //                    {
        //                        X = x,
        //                        Y = y,
        //                        Point = new Point(xPoint, yPoint),
        //                    };

        //                    holders.Add(holder);

        //                    yPoint -= interval;
        //                    y += interval;
        //                }

        //                xPoint += interval;
        //                yPoint = origin.Y;
        //                x += 100;
        //                y = 0;
        //            }

        //            // polyline
        //            chartPolyline = new Polyline()
        //            {
        //                Stroke = new SolidColorBrush(Color.FromRgb(61, 64, 91)),
        //                StrokeThickness = 3,
        //            };
        //            chartCanvas.Children.Add(chartPolyline);

        //            // add connection points to polyline
        //            foreach (var value in values)
        //            {
        //                var holder = holders.FirstOrDefault(h => h.X == value.X && h.Y == value.Y);
        //                if (holder != null)
        //                    chartPolyline.Points.Add(holder.Point);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }

    //public class Value
    //    {
    //        public DateTime Date { get; set; }
    //        public double Weight { get; set; }

    //        public double X { get; set; }//data
    //        public double Y { get; set; }//waga

    //        public Value(double x, double y, DateTime date, double weight)
    //        {
    //            X = x;
    //            Y = y;
    //            Date = date;
    //            Weight = weight;
    //        }
    //    }

    //public class Holder
    //{
    //    public double X { get; set; }
    //    public double Y { get; set; }
    //    public Point Point { get; set; }

    //    public Holder()
    //    {
    //    }
    //}

}
