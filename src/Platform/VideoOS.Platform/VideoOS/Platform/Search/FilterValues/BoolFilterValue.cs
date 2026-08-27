using System.Globalization;

namespace VideoOS.Platform.Search.FilterValues;

public class BoolFilterValue : FilterValueBase
{
	private bool _value;

	public bool Value
	{
		get
		{
			return _value;
		}
		set
		{
			if (!object.Equals(_value, value))
			{
				_value = value;
				FireChangedEvent();
			}
		}
	}

	public override string FallbackDisplayValue()
	{
		return Value.ToString(CultureInfo.CurrentCulture);
	}

	public override string Serialize()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0}", Value);
	}

	public override bool Deserialize(string data)
	{
		if (bool.TryParse(data, out var result))
		{
			Value = result;
			return true;
		}
		return false;
	}
}
