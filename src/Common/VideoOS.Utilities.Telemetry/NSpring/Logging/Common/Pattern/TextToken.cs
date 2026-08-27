namespace NSpring.Logging.Common.Pattern;

public sealed class TextToken : Token
{
	private TextToken()
	{
	}

	public TextToken(string _text)
	{
		type = TokenType.Text;
		text = _text;
	}
}
