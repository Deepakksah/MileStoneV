using System;

namespace VideoOS.Mobile.Utilities;

public class ConditionalCounter<T>
{
	private int _currentCount;

	public Action<int> CountOnCondition;

	public Func<T, bool> CounterResetCondition;

	public void Increment(T t)
	{
		_currentCount++;
		if (CounterResetCondition(t))
		{
			CountOnCondition(_currentCount);
			_currentCount = 0;
		}
	}

	public void Reset()
	{
		_currentCount = 0;
	}
}
