using System;

namespace VideoOS.Management.Server.ConfigurationApi;

public class ValueInvalidFormatException : Exception
{
	private int _position = -1;

	public int Position => _position;

	public ValueInvalidFormatException()
	{
	}

	public ValueInvalidFormatException(string message, int position)
		: base(message)
	{
		_position = position;
	}
}
