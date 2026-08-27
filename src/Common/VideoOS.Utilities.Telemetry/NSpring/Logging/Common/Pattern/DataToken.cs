namespace NSpring.Logging.Common.Pattern;

public sealed class DataToken : Token
{
	private DataToken()
	{
	}

	public DataToken(string _text)
	{
		type = TokenType.Data;
		text = _text;
	}
}
