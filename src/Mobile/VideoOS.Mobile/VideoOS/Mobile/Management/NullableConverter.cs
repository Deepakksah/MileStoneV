namespace VideoOS.Mobile.Management;

public class NullableConverter<T> where T : struct
{
	public delegate bool TryConvertion(string input, out T result);

	private readonly TryConvertion _convFunction;

	public NullableConverter(TryConvertion convFunction)
	{
		_convFunction = convFunction;
	}

	public T? TryParse(string input)
	{
		if (!_convFunction(input, out var result))
		{
			return null;
		}
		return result;
	}
}
