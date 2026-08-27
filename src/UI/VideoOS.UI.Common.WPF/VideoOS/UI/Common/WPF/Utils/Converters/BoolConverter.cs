using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class BoolConverter<T> : IValueConverter, IMultiValueConverter
{
	public T TrueValue { get; set; }

	public T FalseValue { get; set; }

	public bool IsOrOperation { get; set; }

	public BoolConverter(T trueValue, T falseValue, bool isOrOperation)
	{
		TrueValue = trueValue;
		FalseValue = falseValue;
		IsOrOperation = isOrOperation;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return System.Convert.ToBoolean(value, CultureInfo.InvariantCulture) ? TrueValue : FalseValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return TrueValue.Equals(value);
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		foreach (object obj in values)
		{
			bool flag = false;
			if (obj != DependencyProperty.UnsetValue)
			{
				flag = System.Convert.ToBoolean(obj, CultureInfo.InvariantCulture);
			}
			if (IsOrOperation && flag)
			{
				return TrueValue;
			}
			if (!IsOrOperation && !flag)
			{
				return FalseValue;
			}
		}
		return IsOrOperation ? FalseValue : TrueValue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException("Bool converter can only bind to target in multi-value mode.");
	}
}
