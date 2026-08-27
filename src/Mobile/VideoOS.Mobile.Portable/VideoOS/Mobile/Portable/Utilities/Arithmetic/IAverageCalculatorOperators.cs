namespace VideoOS.Mobile.Portable.Utilities.Arithmetic;

public interface IAverageCalculatorOperators<T>
{
	T Add(T first, T second);

	T Substract(T first, T second);

	T Divide(T divident, int divisor);
}
