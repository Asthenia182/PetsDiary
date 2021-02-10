using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PetsDiary.Presentation.Controls
{
    public class IconButton : Button
    {
        public static readonly DependencyProperty IconSourceProperty =
           DependencyProperty.Register("IconSource", typeof(Geometry), typeof(IconButton), new FrameworkPropertyMetadata(null));

        public Geometry IconSource
        {
            get { return (Geometry)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(IconButton), new FrameworkPropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
       DependencyProperty.Register("IconHeight", typeof(double), typeof(IconButton), new FrameworkPropertyMetadata(null));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public static readonly DependencyProperty IconWidthProperty =
        DependencyProperty.Register("IconWidth", typeof(double), typeof(IconButton), new FrameworkPropertyMetadata(null));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
    }
}