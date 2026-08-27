namespace VideoOS.Mobile.Portable.Utilities.Arithmetic;

public class MovingWindowAverageCalculatorInt : MovingWindowAverageCalculator<int>
{
	public MovingWindowAverageCalculatorInt(int maxCount)
		: base((IAverageCalculatorOperators<int>)new AverageCalculatorIntOperators(), maxCount)
	{
	}
}
