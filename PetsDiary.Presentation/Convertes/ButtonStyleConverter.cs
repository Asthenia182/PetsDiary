using System;
using System.Windows;
using System.Windows.Data;

namespace PetsDiary.Presentation.Convertes
{
    public class ButtonStyleConverter : IValueConverter
    {
        public Style EditButtonStyle { get; set; }
        public Style SaveButtonStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var isInEdit = (bool)value;
            return isInEdit ? SaveButtonStyle : EditButtonStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}