using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace VideoOS.Platform.UI.Converters;

public sealed class LambdaConverter6<T1, T2, T3, T4, T5, T6> : LambdaConverterBase, IMultiValueConverter
{
	private readonly Func<T1, T2, T3, T4, T5, T6, object> _convertFunction;

	public LambdaConverter6(Func<T1, T2, T3, T4, T5, T6, object> convertFunction, [CallerMemberName] string callerMemberName = "")
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
		if (!TryGetValue<T3>(values, 2, out var v3))
		{
			return Binding.DoNothing;
		}
		if (!TryGetValue<T4>(values, 3, out var v4))
		{
			return Binding.DoNothing;
		}
		if (!TryGetValue<T5>(values, 4, out var v5))
		{
			return Binding.DoNothing;
		}
		if (!TryGetValue<T6>(values, 5, out var v6))
		{
			return Binding.DoNothing;
		}
		return _convertFunction(v, v2, v3, v4, v5, v6);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
