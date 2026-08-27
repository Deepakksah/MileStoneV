using System;
using System.Text;

namespace NSpring.Logging.Common.Pattern;

public sealed class MessageToken : Token
{
	private const string BLANK_STRING = "";

	private const string ELLIPSIS = "...";

	private const char SPACE_CHAR = ' ';

	private Alignment alignment;

	private Case __case;

	private int minimumWidth;

	private int maximumWidth;

	public Alignment Alignment => alignment;

	public Case Case => __case;

	public int MinimumWidth => minimumWidth;

	public int MaximumWidth => maximumWidth;

	private MessageToken()
	{
	}

	public MessageToken(string _text, int _minimumWidth, int _maximumWidth, Alignment _alignment, Case _case)
	{
		type = TokenType.Message;
		text = _text;
		minimumWidth = Math.Max(_minimumWidth, 0);
		maximumWidth = Math.Max(minimumWidth, _maximumWidth);
		alignment = _alignment;
		__case = _case;
	}

	public override string Format(Event e)
	{
		string text = e.Message;
		if (text == null)
		{
			text = ((minimumWidth != 0) ? "".PadLeft(minimumWidth, ' ') : "");
		}
		else if (text.Length < minimumWidth)
		{
			switch (alignment)
			{
			case Alignment.Center:
			{
				int num = (minimumWidth - text.Length) / 2;
				if (num > 0)
				{
					text = text.PadLeft(num, ' ');
				}
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
			case Alignment.Left:
				text = text.PadRight(minimumWidth, ' ');
				break;
			case Alignment.Right:
				text = text.PadLeft(minimumWidth, ' ');
				break;
			case Alignment.Default:
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
		}
		else if (text.Length > maximumWidth)
		{
			text = ((maximumWidth <= 7) ? text.Substring(0, maximumWidth) : (text.Substring(0, maximumWidth - 3) + "..."));
		}
		switch (__case)
		{
		case Case.Lower:
			text = text.ToLower();
			break;
		case Case.Upper:
			text = text.ToUpper();
			break;
		}
		return text;
	}

	public override void Format(Event e, StringBuilder sb)
	{
		string text = e.Message;
		if (text == null)
		{
			if (minimumWidth != 0)
			{
				sb.Append("".PadLeft(minimumWidth, ' '));
			}
			return;
		}
		if (text.Length < minimumWidth)
		{
			switch (alignment)
			{
			case Alignment.Center:
			{
				int num = (minimumWidth - text.Length) / 2;
				if (num > 0)
				{
					text = text.PadLeft(num, ' ');
				}
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
			case Alignment.Left:
				text = text.PadRight(minimumWidth, ' ');
				break;
			case Alignment.Right:
				text = text.PadLeft(minimumWidth, ' ');
				break;
			case Alignment.Default:
				text = text.PadRight(minimumWidth, ' ');
				break;
			}
		}
		else if (text.Length > maximumWidth)
		{
			text = ((maximumWidth <= 7) ? text.Substring(0, maximumWidth) : (text.Substring(0, maximumWidth - 3) + "..."));
		}
		switch (__case)
		{
		case Case.Lower:
			sb.Append(text.ToLower());
			break;
		case Case.Upper:
			sb.Append(text.ToUpper());
			break;
		case Case.Default:
			sb.Append(text);
			break;
		}
	}
}
