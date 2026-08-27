namespace NSpring.Logging.Common.Pattern;

public sealed class EventToken : Token
{
	private EventToken()
	{
	}

	public EventToken(string _text)
	{
		type = TokenType.Event;
		text = _text;
	}
}
