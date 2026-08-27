using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VideoOS.Platform.UI.Converters;

public sealed class LambdaConverter0 : LambdaConverterBase, IValueConverter, IMultiValueConverter
{
	private readonly Func<object> _convertFunction;

	public LambdaConverter0(Func<object> convertFunction, [CallerMemberName] string callerMemberName = "")
		: base(convertFunction, callerMemberName)
	{
		_convertFunction = convertFunction;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return _convertFunction();
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		return _convertFunction();
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
