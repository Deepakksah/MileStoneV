namespace VideoOS.Mobile.Portable.Utilities.Rects;

public class FloatOperators : IOperators<double>
{
	public double Add(double first, double second)
	{
		return first + second;
	}

	public double Sub(double first, double second)
	{
		return first - second;
	}

	public bool Greater(double first, double second)
	{
		return first > second;
	}

	public bool Lower(double first, double second)
	{
		return first < second;
	}
}
