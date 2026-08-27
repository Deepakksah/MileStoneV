using System.Text;

namespace VideoOS.Platform.JsonHandling;

public class JsonBuilder
{
	private StringBuilder _sb = new StringBuilder();

	private bool _first = true;

	private bool _array;

	private bool _hasEnded;

	public JsonBuilder()
	{
		_array = false;
		_hasEnded = true;
	}

	public JsonBuilder(bool array)
	{
		_array = array;
		if (array)
		{
			_sb.Append(" [ ");
		}
		else
		{
			_sb.Append("{ ");
		}
	}

	public void Clear()
	{
		_first = true;
		_sb = new StringBuilder();
		_sb.Append(_array ? " [ " : "{ ");
	}

	public void AppendElement(string elementWithBrackets)
	{
		if (!string.IsNullOrEmpty(elementWithBrackets))
		{
			FirstCheck();
			_sb.Append(elementWithBrackets);
		}
	}

	public void AppendElement(string key, string elementValue)
	{
		AppendKeyValue(key, elementValue);
	}

	public void AppendKeyString(string key, string value)
	{
		string text = JsonUtilities.JsonEscape(key);
		FirstCheck();
		if (value == null)
		{
			_sb.Append(" \"" + text + "\" : null");
			return;
		}
		_sb.Append(" \"" + text + "\" : \"" + value + "\"");
	}

	public void AppendString(string value)
	{
		FirstCheck();
		if (value == null)
		{
			_sb.Append("null");
		}
		else
		{
			_sb.Append("\"" + value + "\"");
		}
	}

	public void AppendKeyStringEscaped(string key, string value)
	{
		string text = JsonUtilities.JsonEscape(key);
		FirstCheck();
		if (value == null)
		{
			_sb.Append(" \"" + text + "\" : null");
			return;
		}
		_sb.Append(" \"" + text + "\" : \"" + JsonUtilities.JsonEscape(value) + "\"");
	}

	public void AppendKeyValue(string key, string value)
	{
		string text = JsonUtilities.JsonEscape(key);
		FirstCheck();
		if (string.IsNullOrEmpty(value))
		{
			_sb.Append(" \"" + text + "\" : null");
		}
		else
		{
			_sb.Append(" \"" + text + "\" : " + value);
		}
	}

	public string ToJson()
	{
		if (!_hasEnded)
		{
			if (_array)
			{
				_sb.Append(" ]");
			}
			else
			{
				_sb.Append(" }");
			}
			_hasEnded = true;
		}
		return _sb.ToString();
	}

	public override string ToString()
	{
		return _sb.ToString();
	}

	public void AppendElementString(string elementValue)
	{
		FirstCheck();
		_sb.Append("\"" + JsonUtilities.JsonEscape(elementValue) + "\"");
	}

	public void AppendElementInt(int elementValue)
	{
		FirstCheck();
		_sb.Append(elementValue);
	}

	private void FirstCheck()
	{
		if (!_first)
		{
			_sb.Append(", ");
		}
		_first = false;
	}
}
