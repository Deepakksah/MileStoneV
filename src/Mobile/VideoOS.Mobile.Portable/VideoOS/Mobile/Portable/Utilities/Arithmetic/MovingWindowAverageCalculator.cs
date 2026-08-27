using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities.Arithmetic;

public class MovingWindowAverageCalculator<T> : AverageCalculator<T>
{
	private readonly Queue<T> _values = new Queue<T>();

	private readonly int _maxCount;

	public MovingWindowAverageCalculator(IAverageCalculatorOperators<T> operators, int maxCount)
		: base(operators)
	{
		_maxCount = maxCount;
	}

	public override void Add(T value)
	{
		if (_values.Count == _maxCount)
		{
			T second = _values.Dequeue();
			_sum = _operators.Substract(_sum, second);
		}
		_values.Enqueue(value);
		_sum = _operators.Add(_sum, value);
		_count = _values.Count;
	}

	public override void Clear()
	{
		base.Clear();
		_values.Clear();
	}
}
