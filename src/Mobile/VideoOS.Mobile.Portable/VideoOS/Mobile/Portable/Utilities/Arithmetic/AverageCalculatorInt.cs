namespace VideoOS.Mobile.Portable.Utilities.Arithmetic;

public class AverageCalculatorInt : AverageCalculator<int>
{
	public AverageCalculatorInt()
		: base((IAverageCalculatorOperators<int>)new AverageCalculatorIntOperators())
	{
	}
}
