using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VideoOS.Platform.UI.Converters;

public sealed class LambdaConverter2<T1, T2> : LambdaConverterBase, IMultiValueConverter
{
	private readonly Func<T1, T2, object> _convertFunction;

	public LambdaConverter2(Func<T1, T2, object> convertFunction, [CallerMemberName] string callerMemberName = "")
		: base(convertFunction, callerMemberName)
	{
		_convertFunction = convertFunction;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (!TryGetValue<T1>(values, 0, out var v))
		{
			return Binding.DoNothing;
		}
		if (!TryGetValue<T2>(values, 1, out var v2))
		{
			return Binding.DoNothing;
		}
		return _convertFunction(v, v2);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
