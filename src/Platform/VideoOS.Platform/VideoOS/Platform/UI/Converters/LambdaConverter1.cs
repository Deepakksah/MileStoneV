using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VideoOS.Platform.UI.Converters;

public sealed class LambdaConverter1<T1> : LambdaConverterBase, IValueConverter, IMultiValueConverter
{
	private readonly Func<T1, object> _convertFunction;

	public LambdaConverter1(Func<T1, object> convertFunction, [CallerMemberName] string callerMemberName = "")
		: base(convertFunction, callerMemberName)
	{
		_convertFunction = convertFunction;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!TryGetValue<T1>(value, out var v))
		{
			return Binding.DoNothing;
		}
		return _convertFunction(v);
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (!TryGetValue<T1>(values, 0, out var v))
		{
			return Binding.DoNothing;
		}
		return _convertFunction(v);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
