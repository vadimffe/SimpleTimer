﻿using System.Windows;

namespace SimpleTimer.Resources.Converter
{
    public static class Constants
    {
        public static readonly Thickness TextBoxDefaultPadding = new Thickness(0, 4, 0, 4);
        public static readonly Thickness TextBoxFloatingHintPadding = new Thickness(0, 12, 0, 0);
        public static readonly Thickness DefaultTextBoxViewMargin = new Thickness(1, 0, 1, 0);
        public static readonly Thickness DefaultTextBoxViewMarginEmbedded = new Thickness(0);
        public const double TextBoxNotEnabledOpacity = 0.56;
        public const double PickerTextBoxInnerButtonSpacing = 4;
        public static readonly Thickness InnerButtonMargin = new Thickness(PickerTextBoxInnerButtonSpacing, 0, 0, 0);
    }
}
