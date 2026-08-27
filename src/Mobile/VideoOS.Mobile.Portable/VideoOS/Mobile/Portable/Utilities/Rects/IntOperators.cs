namespace VideoOS.Mobile.Portable.Utilities.Rects;

public class IntOperators : IOperators<int>
{
	public int Add(int first, int second)
	{
		return first + second;
	}

	public int Sub(int first, int second)
	{
		return first - second;
	}

	public bool Greater(int first, int second)
	{
		return first > second;
	}

	public bool Lower(int first, int second)
	{
		return first < second;
	}
}
