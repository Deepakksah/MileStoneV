using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Converters;

public abstract class LambdaConverterBase
{
	private readonly Delegate _convertDelegate;

	private readonly string _baseCallerName;

	public LambdaConverterBase(Delegate convertDelegate, string baseCallerName)
	{
		_convertDelegate = convertDelegate;
		_baseCallerName = baseCallerName;
	}

	protected bool TryGetValue<T>(object value, out T v)
	{
		if (value == DependencyProperty.UnsetValue)
		{
			v = default(T);
			return false;
		}
		if (value is T val)
		{
			v = val;
			return true;
		}
		if (value == null && !typeof(T).IsValueType)
		{
			v = default(T);
			return true;
		}
		if (value == null && Nullable.GetUnderlyingType(typeof(T)) != null)
		{
			v = default(T);
			return true;
		}
		throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0}: Invalid converter input parameter type. Expected: {1}, Actual: {2}", GetConverterName(), typeof(T).FullName, value?.GetType().FullName ?? "null")));
	}

	protected bool TryGetValue<T>(object[] values, int index, out T v)
	{
		if (index >= values.Length)
		{
			throw new ArgumentException(FormattableString.Invariant($"{GetConverterName()}: Missing converter input parameter at index {index}. Expected input parameter of type: {typeof(T).FullName}"));
		}
		object obj = values[index];
		if (obj == DependencyProperty.UnsetValue)
		{
			v = default(T);
			return false;
		}
		if (obj is T val)
		{
			v = val;
			return true;
		}
		if (obj == null && !typeof(T).IsValueType)
		{
			v = default(T);
			return true;
		}
		if (obj == null && Nullable.GetUnderlyingType(typeof(T)) != null)
		{
			v = default(T);
			return true;
		}
		throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0}: Invalid converter input parameter type at index {1}. Expected: {2}, Actual: {3}", GetConverterName(), index, typeof(T).FullName, obj?.GetType().FullName ?? "null")));
	}

	private string GetConverterName()
	{
		return _convertDelegate.Method.DeclaringType?.ToString() + "." + _baseCallerName;
	}
}
