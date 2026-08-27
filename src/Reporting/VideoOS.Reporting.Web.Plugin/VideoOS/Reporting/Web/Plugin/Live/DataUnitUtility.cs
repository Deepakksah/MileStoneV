using System;

namespace VideoOS.Reporting.Web.Plugin.Live;

public static class DataUnitUtility
{
	public static (float Value, Unit Unit) GetCalculatedUnit(float value, Unit[] units, float maxValue, uint factor)
	{
		return GetCalculatedUnit((double)value, units, (double)maxValue, factor);
	}

	public static (float Value, Unit Unit) GetCalculatedUnit(long value, Unit[] units, long maxValue, uint factor)
	{
		return GetCalculatedUnit((double)value, units, (double)maxValue, factor);
	}

	public static (float Value, Unit Unit) GetCalculatedUnit(ulong value, Unit[] units, ulong maxValue, uint factor)
	{
		return GetCalculatedUnit((double)value, units, (double)maxValue, factor);
	}

	public static (float Value, Unit Unit) GetCalculatedUnit(decimal value, Unit[] units, decimal maxValue, uint factor)
	{
		return GetCalculatedUnit((double)value, units, (double)maxValue, factor);
	}

	public static (float Value, Unit Unit) GetCalculatedUnit(double value, Unit[] units, double maxValue, uint factor)
	{
		if (units == null)
		{
			throw new ArgumentNullException("units");
		}
		int num = 0;
		while (value > maxValue && num < units.Length - 1)
		{
			value /= (double)factor;
			num++;
		}
		return (Value: (float)value, Unit: units[num]);
	}
}
