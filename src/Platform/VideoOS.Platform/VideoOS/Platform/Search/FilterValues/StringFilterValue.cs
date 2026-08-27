using System;
using System.Globalization;

namespace VideoOS.Platform.Search.FilterValues;

public class StringFilterValue : FilterValueBase
{
	private string _text = string.Empty;

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!object.Equals(_text, value))
			{
				_text = value;
				FireChangedEvent();
			}
		}
	}

	public override string FallbackDisplayValue()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0}", Text);
	}

	public override string Serialize()
	{
		return Text;
	}

	public override bool Deserialize(string data)
	{
		if (data == null)
		{
			return false;
		}
		Text = data;
		return true;
	}
}
