using System;
using System.Text;

namespace NSpring.Logging.Common.Pattern;

public abstract class Pattern
{
	protected string source;

	protected Token[] tokens;

	public string Source => source;

	public Token[] Tokens
	{
		get
		{
			Token[] array = new Token[tokens.Length];
			Array.Copy(tokens, 0, array, 0, tokens.Length);
			return array;
		}
	}

	private Pattern()
	{
	}

	protected Pattern(string _source)
		: this(_source, _isContextPropertyEnabled: true, _isEventPropertyEnabled: true, _isEventEnabled: true, _isMessageEnabled: true, _isDataEnabled: true)
	{
	}

	protected Pattern(string _source, bool _isContextPropertyEnabled, bool _isEventPropertyEnabled, bool _isEventEnabled, bool _isMessageEnabled, bool _isDataEnabled)
	{
		if (_source == null)
		{
			throw new ArgumentNullException();
		}
		source = _source;
		tokens = new PatternParser(_isContextPropertyEnabled, _isEventPropertyEnabled, _isEventEnabled, _isMessageEnabled, _isDataEnabled).Parse(source);
	}

	protected Pattern(string _source, bool _isContextPropertyEnabled, bool _isEventPropertyEnabled, bool _isEventEnabled, bool _isMessageEnabled, bool _isDataEnabled, char[] _separators)
	{
		if (_source == null)
		{
			throw new ArgumentNullException();
		}
		source = _source;
		tokens = new PatternParser(_isContextPropertyEnabled, _isEventPropertyEnabled, _isEventEnabled, _isMessageEnabled, _isDataEnabled, _separators).Parse(source);
	}

	public abstract string Format(Event e);

	public abstract void Format(Event e, StringBuilder sb);
}
