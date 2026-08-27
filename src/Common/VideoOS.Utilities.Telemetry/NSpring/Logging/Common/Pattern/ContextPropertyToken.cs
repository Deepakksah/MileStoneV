using System;
using System.Text;

namespace NSpring.Logging.Common.Pattern;

public sealed class ContextPropertyToken : Token
{
	private const string BLANK_STRING = "";

	private const char SPACE = ' ';

	private Alignment alignment;

	private Case __case;

	private int minimumWidth;

	private int maximumWidth;

	private ContextPropertyType propertyType;

	private string propertyName;

	public ContextPropertyType PropertyType => propertyType;

	public string PropertyName => propertyName;

	public Alignment Alignment => alignment;

	public Case Case => __case;

	public int MinimumWidth => minimumWidth;

	public int MaximumWidth => maximumWidth;

	private ContextPropertyToken()
	{
	}

	public ContextPropertyToken(ContextPropertyType _propertyType, string _propertyName, string _text, int _minimumWidth, int _maximumWidth, Alignment _alignment, Case _case)
	{
		type = TokenType.ContextProperty;
		propertyType = _propertyType;
		propertyName = _propertyName;
		text = _text;
		minimumWidth = Math.Max(_minimumWidth, 0);
		maximumWidth = Math.Max(minimumWidth, _maximumWidth);
		alignment = _alignment;
		__case = _case;
	}

	public override string Format(Event _event)
	{
		Context context = _event.Context;
		string text = null;
		if (context != null)
		{
			switch (propertyType)
			{
			case ContextPropertyType.ApplicationName:
				text = context.ApplicationName;
				break;
			case ContextPropertyType.SystemName:
				text = context.SystemName;
				break;
			case ContextPropertyType.UserName:
				text = context.UserName;
				break;
			case ContextPropertyType.ThreadName:
				text = context.ThreadName;
				break;
			case ContextPropertyType.ThreadId:
				text = context.ThreadId;
				break;
			case ContextPropertyType.ThreadPriority:
				text = context.ThreadPriority.ToString();
				break;
			case ContextPropertyType.Property:
			{
				object property = context.GetProperty(propertyName);
				if (property != null)
				{
					text = property.ToString();
				}
				break;
			}
			}
		}
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
					text = text.PadLeft(text.Length + num, ' ');
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
			text = text.Substring(0, maximumWidth);
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

	public override void Format(Event _event, StringBuilder sb)
	{
		Context context = _event.Context;
		string text = null;
		if (context != null)
		{
			switch (propertyType)
			{
			case ContextPropertyType.ApplicationName:
				text = context.ApplicationName;
				break;
			case ContextPropertyType.SystemName:
				text = context.SystemName;
				break;
			case ContextPropertyType.UserName:
				text = context.UserName;
				break;
			case ContextPropertyType.ThreadName:
				text = context.ThreadName;
				break;
			case ContextPropertyType.ThreadId:
				text = context.ThreadId;
				break;
			case ContextPropertyType.ThreadPriority:
				text = context.ThreadPriority.ToString();
				break;
			case ContextPropertyType.Property:
			{
				object property = context.GetProperty(propertyName);
				if (property != null)
				{
					text = property.ToString();
				}
				break;
			}
			}
		}
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
					text = text.PadLeft(text.Length + num, ' ');
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
			text = text.Substring(0, maximumWidth);
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
