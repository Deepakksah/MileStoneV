namespace VideoOS.Mobile.Portable.Utilities.Arithmetic;

public class AverageCalculator<T>
{
	protected readonly IAverageCalculatorOperators<T> _operators;

	protected T _sum;

	protected int _count;

	public T Average => _operators.Divide(_sum, _count);

	public AverageCalculator(IAverageCalculatorOperators<T> operators)
	{
		_operators = operators;
		Clear();
	}

	public virtual void Clear()
	{
		_sum = default(T);
		_count = 0;
	}

	public virtual void Add(T value)
	{
		_sum = _operators.Add(_sum, value);
		_count++;
	}
}
