using System;

namespace VideoOS.Utilities.Workflow;

public class DecisionItem<T> : IDecisionItem
{
	private bool _hasSavedValue;

	private bool _hasNewValue;

	private T _savedValue;

	private T _newValue;

	private Func<T> _calculate;

	private Func<T, T, bool> _compare;

	public DecisionItem(Func<T> calculate)
		: this(calculate, (Func<T, T, bool>)((T oldValue, T newValue) => oldValue.Equals(newValue)))
	{
	}

	public DecisionItem(Func<T> calculate, Func<T, T, bool> compare)
	{
		_calculate = calculate;
		_compare = compare;
	}

	public void CalculateNewValue()
	{
		_newValue = _calculate();
		_hasNewValue = true;
	}

	public void SaveNewValue()
	{
		ValidateNewValue();
		_hasNewValue = false;
		_savedValue = _newValue;
		_hasSavedValue = true;
	}

	public bool IsDifferent()
	{
		ValidateNewValue();
		if (!_hasSavedValue)
		{
			return true;
		}
		return !_compare(_savedValue, _newValue);
	}

	public object GetNewestValue()
	{
		if (_hasNewValue)
		{
			return _newValue;
		}
		if (_hasSavedValue)
		{
			return _savedValue;
		}
		return null;
	}

	private void ValidateNewValue()
	{
		if (!_hasNewValue)
		{
			throw new InvalidOperationException("Calculate new value first!");
		}
	}
}
