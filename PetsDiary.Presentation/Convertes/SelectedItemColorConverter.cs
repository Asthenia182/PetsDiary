using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using static PetsDiary.Presentation.ViewModels.HomeViewModel;

namespace PetsDiary.Presentation.Convertes
{
    public class SelectedItemColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool)value;

            return isSelected ? new SolidColorBrush(Color.FromArgb(255, 129, 178, 154))
                : new SolidColorBrush(Color.FromArgb(255, 151, 196, 171));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}