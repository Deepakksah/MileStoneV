using System;

namespace VideoOS.UI.Common.WPF.Utils.Validation;

public class ValidationEventArgs : EventArgs
{
	private readonly string _propertyName;

	public string PropertyName => _propertyName;

	protected ValidationEventArgs(string propertyName)
	{
		_propertyName = propertyName;
	}
}
public class ValidationEventArgs<T> : ValidationEventArgs
{
	private readonly T _originalValue;

	private readonly T _proposedValue;

	private T _effectiveValue;

	public T OriginalValue => _originalValue;

	public T ProposedValue => _proposedValue;

	public T EffectiveValue
	{
		get
		{
			return _effectiveValue;
		}
		set
		{
			_effectiveValue = value;
		}
	}

	public ValidationEventArgs(T originalValue, T proposedValue, string propertyName)
		: base(propertyName)
	{
		_originalValue = originalValue;
		_proposedValue = proposedValue;
		_effectiveValue = proposedValue;
	}
}
