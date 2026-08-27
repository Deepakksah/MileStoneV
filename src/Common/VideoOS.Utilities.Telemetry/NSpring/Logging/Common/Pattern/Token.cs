using System.Text;

namespace NSpring.Logging.Common.Pattern;

public abstract class Token
{
	protected string text;

	protected TokenType type;

	public TokenType Type => type;

	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
		}
	}

	public virtual void Format(Event e, StringBuilder sb)
	{
		sb.Append(text);
	}

	public virtual string Format(Event e)
	{
		return text;
	}
}
