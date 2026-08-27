using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class ReplacementLimit : IReplacementLimit
{
	public int Minimum { get; set; }

	public int Maximum { get; set; }

	public int Percentage { get; set; }

	public ReplacementLimit()
	{
		Minimum = 0;
		Maximum = 0;
		Percentage = 0;
	}

	public ReplacementLimit(int minimum, int maximum, int percentage)
	{
		Minimum = minimum;
		Maximum = maximum;
		Percentage = percentage;
	}

	public int CalculateTotalReplacementLimit(int totalCameraCount)
	{
		if (Percentage == 100)
		{
			return int.MaxValue;
		}
		int val = Convert.ToInt32(Math.Floor((double)Percentage / 100.0 * (double)totalCameraCount));
		return Math.Max(Minimum, Math.Min(val, Maximum));
	}
}
