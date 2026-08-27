using System.Globalization;

namespace VideoOS.Platform.Search.FilterValues;

public class DoubleFilterValue : FilterValueBase
{
	private double _value;

	public double Value
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
		return string.Format(CultureInfo.CurrentCulture, "{0:F3}", Value);
	}

	public override string Serialize()
	{
		return Value.ToString(CultureInfo.InvariantCulture);
	}

	public override bool Deserialize(string data)
	{
		if (double.TryParse(data, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
		{
			Value = result;
			return true;
		}
		return false;
	}
}
