using System;
using System.Globalization;

namespace VideoOS.Platform.Search.FilterValues;

public class DoubleRangeFilterValue : FilterValueBase
{
	private double _high;

	private double _low;

	private const char _separator = '\u001b';

	public double High
	{
		get
		{
			return _high;
		}
		set
		{
			if (!object.Equals(_high, value))
			{
				_high = value;
				FireChangedEvent();
			}
		}
	}

	public double Low
	{
		get
		{
			return _low;
		}
		set
		{
			if (!object.Equals(_low, value))
			{
				_low = value;
				FireChangedEvent();
			}
		}
	}

	public override string FallbackDisplayValue()
	{
		return string.Format(CultureInfo.CurrentCulture, "{0:F3} - {1:F3}", Low, High);
	}

	public override string Serialize()
	{
		return Low.ToString(CultureInfo.InvariantCulture) + "\u001b" + High.ToString(CultureInfo.InvariantCulture);
	}

	public override bool Deserialize(string data)
	{
		if (string.IsNullOrWhiteSpace(data))
		{
			return false;
		}
		string[] array = data.Split(new char[1] { '\u001b' }, StringSplitOptions.None);
		if (array.Length == 2 && double.TryParse(array[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var result) && double.TryParse(array[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var result2))
		{
			_low = result;
			_high = result2;
			FireChangedEvent();
			return true;
		}
		return false;
	}
}
