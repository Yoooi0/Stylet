﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace Stylet.Xaml
{
    /// <summary>
    /// Converter which passes through values, but uses Debug.WriteLine to log them. Useful for debugging
    /// </summary>
    public class DebugConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Singleton instance of this DebugConverter. Usage e.g. Converter={x:Static s:DebugConverter.Instance}"
        /// </summary>
        public static readonly DebugConverter Instance;

        /// <summary>
        /// Category to use with Debug.WriteLine
        /// </summary>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Property specifying the category to use with Debug.WriteLine
        /// </summary>
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(DebugConverter), new PropertyMetadata("DebugConverter"));


        /// <summary>
        /// Logger to use. Defaults to Debug.WriteLine. Arguments are: Message, Name
        /// </summary>
        public Action<string, string> Logger
        {
            get { return (Action<string, string>)GetValue(LoggerProperty); }
            set { SetValue(LoggerProperty, value); }
        }

        /// <summary>
        /// Property specifying an action, which when called will log an entry.
        /// </summary>
        public static readonly DependencyProperty LoggerProperty =
            DependencyProperty.Register("Logger", typeof(Action<string, string>), typeof(DebugConverter), new PropertyMetadata(null));


        static DebugConverter()
        {
            // Have to set this from within a static constructor, as it's run after the field initializers
            // Otherwise it gets called before the DependencyProperties have been created, and that causes the (normal) constructor to fall over
            Instance = new DebugConverter();
        }

        /// <summary>
        /// Create a new DebugConverter instance
        /// </summary>
        public DebugConverter()
        {
            if (this.Logger == null)
                this.Logger = (msg, name) => Debug.WriteLine(msg, name);
        }

        /// <summary>
        /// Perform the conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                this.Logger(String.Format("Convert: Value = '{0}' TargetType = '{1}'", value, targetType), this.Name);
            else
                this.Logger(String.Format("Convert: Value = '{0}' TargetType = '{1}' Parameter = '{2}'", value, targetType, parameter), this.Name);

            return value;
        }

        /// <summary>
        /// Perform the reverse conversion
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                this.Logger(String.Format("ConvertBack: Value = '{0}' TargetType = '{1}'", value, targetType), this.Name);
            else
                this.Logger(String.Format("ConvertBack: Value = '{0}' TargetType = '{1}' Parameter = '{2}'", value, targetType, parameter), this.Name);

            return value;
        }
    }
}