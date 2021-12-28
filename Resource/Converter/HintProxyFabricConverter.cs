using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleTimer.Resources.Converter
{
    public class HintProxyFabricConverter : IValueConverter
    {
        private static readonly Lazy<HintProxyFabricConverter> _instance = new Lazy<HintProxyFabricConverter>();

        public static HintProxyFabricConverter Instance => _instance.Value;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return HintProxyFabric.Get(value as Control);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
