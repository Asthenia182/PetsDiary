using PetsDiary.Common.Models;
using PetsDiary.Presentation.Interfaces;
using PetsDiary.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PetsDiary.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for LineChart.xaml
    /// </summary>
    public partial class LineChart : UserControl
    {
        private Line xAxisLine, yAxisLine;
        private double xAxisStart = 50, yAxisStart = 50, interval = 50;
        private Polyline chartPolyline;
        private Point origin;
        private List<Holder> holders;
        private List<Value> values;

        private Dictionary<DateTime, double> dateToX;
        private Dictionary<double, double> weightToY;

        public LineChart()
        {
            InitializeComponent();

            holders = new List<Holder>();
            values = new List<Value>();          

            Paint();

            this.SizeChanged += (sender, e) => Paint();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == nameof(Weights))
            {
                chartCanvas.Children.Clear();
                Paint();
            }
        }

        public void Paint()
        {
            try
            {
                if (this.ActualWidth > 0 && this.ActualHeight > 0)
                {
                    chartCanvas.Children.Clear();
                    holders.Clear();
                    values.Clear();

                    if (Weights == null || Weights.Count == 0) return;

                    ///find x - points
                    dateToX = new Dictionary<DateTime, double>();
                    double xPosition = 0;
                    foreach (var model in Weights.OrderBy(x => x.Date))
                    {
                        if(!dateToX.ContainsKey(model.Date)) 
                         {
                            dateToX.Add(model.Date, xPosition);                            
                        }
                        xPosition += interval;
                    }
                    ///find y - points
                    weightToY = new Dictionary<double, double>();
                    double yPosition = 0;

                    foreach (var model in Weights.OrderBy(x => x.Weight))
                    {
                        if (!weightToY.ContainsKey(model.Weight))
                        {
                            weightToY.Add(model.Weight, yPosition);                         
                        }

                        yPosition += interval;
                    }

                    foreach (var weight in Weights.OrderBy(x => x.Date))
                    {
                        var xw = dateToX[weight.Date];
                        var yw = weightToY[weight.Weight];

                        values.Add(new Value(xw, yw, weight.Date, weight.Weight));
                    }

                    // axis lines
                    xAxisLine = new Line()
                    {
                        X1 = xAxisStart,
                        Y1 = this.ActualHeight - yAxisStart,
                        X2 = this.ActualWidth - xAxisStart,
                        Y2 = this.ActualHeight - yAxisStart,
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 166, 204, 182)),
                        StrokeThickness = 1,
                    };
                    yAxisLine = new Line()
                    {
                        X1 = xAxisStart,
                        Y1 = yAxisStart - 50,
                        X2 = xAxisStart,
                        Y2 = this.ActualHeight - yAxisStart,
                        Stroke = new SolidColorBrush(Color.FromArgb(255, 166, 204, 182)),
                        StrokeThickness = 1,
                    };

                    chartCanvas.Children.Add(xAxisLine);
                    chartCanvas.Children.Add(yAxisLine);

                    origin = new Point(xAxisLine.X1, yAxisLine.Y2);

                    var textInX0 = string.Empty;

                    if (dateToX.ContainsValue(0))
                    {
                        textInX0 = dateToX.FirstOrDefault(x => x.Value == 0).Key.ToString("dd-MM-yy");
                    }

                    var xTextBlock0 = new TextBlock() { Text = textInX0, FontSize=10, LayoutTransform = new RotateTransform(30) };
                    chartCanvas.Children.Add(xTextBlock0);
                    Canvas.SetLeft(xTextBlock0, origin.X);
                    Canvas.SetTop(xTextBlock0, origin.Y + 5);

                    // y axis lines
                    var xValue = xAxisStart;
                    double xPoint = origin.X + interval;
                    while (xPoint < xAxisLine.X2)
                    {
                        var line = new Line()
                        {
                            X1 = xPoint,
                            Y1 = yAxisStart - 50,
                            X2 = xPoint,
                            Y2 = this.ActualHeight - yAxisStart,
                            Stroke = new SolidColorBrush(Color.FromArgb(200, 166, 204, 182)),
                            StrokeThickness = 1,
                            Opacity = 1,
                        };

                        chartCanvas.Children.Add(line);

                        var textInLineX = string.Empty;

                        if (dateToX.ContainsValue(xValue))
                            textInLineX = dateToX.FirstOrDefault(x => x.Value == xValue).Key.ToString("dd-MM-yy");

                        var textBlock = new TextBlock { Text = textInLineX, FontSize = 10, LayoutTransform = new RotateTransform(30) };

                        chartCanvas.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, xPoint - 12.5);
                        Canvas.SetTop(textBlock, line.Y2 + 5);

                        xPoint += interval;
                        xValue += interval;
                    }

                    var textInY0 = string.Empty;
                    if (weightToY.ContainsValue(0))
                        textInY0 = weightToY.FirstOrDefault(x => x.Value == 0).Key.ToString() + " kg";
                    var yTextBlock0 = new TextBlock() { Text = textInY0.ToString(), FontSize = 10 };
                    chartCanvas.Children.Add(yTextBlock0);
                    Canvas.SetLeft(yTextBlock0, origin.X - 20);
                    Canvas.SetTop(yTextBlock0, origin.Y - 10);

                    // x axis lines
                    var yValue = yAxisStart;
                    double yPoint = origin.Y - interval;
                    while (yPoint > yAxisLine.Y1)
                    {
                        var line = new Line()
                        {
                            X1 = xAxisStart,
                            Y1 = yPoint,
                            X2 = this.ActualWidth - xAxisStart,
                            Y2 = yPoint,
                            Stroke = new SolidColorBrush(Color.FromArgb(160, 166, 204, 182)),
                            StrokeThickness = 1,
                            Opacity = 1,
                        };

                        chartCanvas.Children.Add(line);

                        string textInLineY = string.Empty;

                        if (weightToY.ContainsValue(yValue))
                            textInLineY = weightToY.FirstOrDefault(x => x.Value == yValue).Key.ToString() + " kg";

                        var textBlock = new TextBlock() { Text = textInLineY };
                        chartCanvas.Children.Add(textBlock);
                        Canvas.SetLeft(textBlock, line.X1 - 30);
                        Canvas.SetTop(textBlock, yPoint - 10);

                        yPoint -= interval;
                        yValue += interval;
                    }

                    // connections
                    double x = 0, y = 0;
                    xPoint = origin.X;
                    yPoint = origin.Y;
                    while (xPoint < xAxisLine.X2)
                    {
                        while (yPoint > yAxisLine.Y1)
                        {
                            var holder = new Holder()
                            {
                                X = x,
                                Y = y,
                                Point = new Point(xPoint, yPoint),
                            };

                            holders.Add(holder);

                            yPoint -= interval;
                            y += interval;
                        }

                        xPoint += interval;
                        yPoint = origin.Y;
                        x += 50;
                        y = 0;
                    }

                    // polyline
                    chartPolyline = new Polyline()
                    {
                        Stroke = new SolidColorBrush(Color.FromRgb(61, 64, 91)),
                        StrokeThickness = 3,
                    };
                    chartCanvas.Children.Add(chartPolyline);

                    // add connection points to polyline
                    foreach (var value in values)
                    {
                        var holder = holders.FirstOrDefault(h => h.X == value.X && h.Y == value.Y);
                        if (holder != null)
                            chartPolyline.Points.Add(holder.Point);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// true - pokazanie sie popupa
        /// </summary>
        public ObservableCollection<IWeightViewModel> Weights
        {
            get { return (ObservableCollection<IWeightViewModel>)GetValue(WeightsProperty); }
            set {
                SetValue(WeightsProperty, value); }
        }

        public static readonly DependencyProperty WeightsProperty = DependencyProperty.Register(
          name: nameof(Weights),
          propertyType: typeof(ObservableCollection<IWeightViewModel>),
          ownerType: typeof(LineChart),
          typeMetadata: new UIPropertyMetadata(null));


    }

    public class Value
    {
        public DateTime Date { get; set; }
        public double Weight { get; set; }

        public double X { get; set; }//data
        public double Y { get; set; }//waga

        public Value(double x, double y, DateTime date, double weight)
        {
            X = x;
            Y = y;
            Date = date;
            Weight = weight;
        }
    }

    public class Holder
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point Point { get; set; }

        public Holder()
        {
        }
    }
}