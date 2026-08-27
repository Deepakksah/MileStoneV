namespace VideoOS.Mobile.Portable.Utilities.Arithmetic;

public class AverageCalculatorIntOperators : IAverageCalculatorOperators<int>
{
	public int Add(int first, int second)
	{
		return first + second;
	}

	public int Substract(int first, int second)
	{
		return first - second;
	}

	public int Divide(int divident, int divisor)
	{
		return divident / divisor;
	}
}
