using System;
using System.Globalization;

namespace VideoOS.Management.Common;

public static class DeviceCoverageHelper
{
	private const double FeetPerMeter = 3.280839895;

	public const double MaximumDeviceCoverageDepth = 1000000000.0;

	public static string CoverageDirectionToDisplayText(double coverageDirection, IFormatProvider formatProvider)
	{
		return NormalizedAngleToDregrees(coverageDirection).ToString(formatProvider);
	}

	public static bool TryDisplayTextToCoverageDirection(string displayText, IFormatProvider formatProvider, out double coverageDirection)
	{
		if (!double.TryParse(displayText, NumberStyles.Any, formatProvider, out coverageDirection))
		{
			return false;
		}
		if (coverageDirection < 0.0 || coverageDirection > 360.0)
		{
			return false;
		}
		coverageDirection /= 360.0;
		return true;
	}

	public static string CoverageFieldOfViewToDisplayText(double coverageFieldOfView, IFormatProvider formatProvider)
	{
		return NormalizedAngleToDregrees(coverageFieldOfView).ToString(formatProvider);
	}

	public static bool TryDisplayTextToCoverageFieldOfView(string displayText, IFormatProvider formatProvider, out double coverageFieldOfView)
	{
		if (!double.TryParse(displayText, NumberStyles.Any, formatProvider, out coverageFieldOfView))
		{
			return false;
		}
		if (coverageFieldOfView < 0.0 || coverageFieldOfView > 360.0)
		{
			return false;
		}
		coverageFieldOfView /= 360.0;
		return true;
	}

	public static string CoverageDepthToDisplayText(double coverageDepth, bool toFeet, IFormatProvider formatProvider)
	{
		coverageDepth = Math.Max(coverageDepth, 0.0);
		coverageDepth = Math.Min(coverageDepth, 1000000000.0);
		if (toFeet)
		{
			coverageDepth = MeterToFeet(coverageDepth);
		}
		return coverageDepth.ToString(formatProvider);
	}

	public static bool TryDisplayTextToCoverageDepth(string displayText, bool fromFeet, IFormatProvider formatProvider, out double coverageDepth)
	{
		if (!double.TryParse(displayText, NumberStyles.Any, formatProvider, out coverageDepth))
		{
			return false;
		}
		if (fromFeet)
		{
			coverageDepth = FeetToMeter(coverageDepth);
		}
		if (coverageDepth < 0.0 || coverageDepth > 1000000000.0)
		{
			return false;
		}
		return true;
	}

	public static double MeterToFeet(double meter)
	{
		return meter * 3.280839895;
	}

	public static double FeetToMeter(double feet)
	{
		return feet / 3.280839895;
	}

	private static double NormalizedAngleToDregrees(double normalizedAngle)
	{
		if (normalizedAngle == 1.0 || normalizedAngle == -1.0)
		{
			return 360.0;
		}
		normalizedAngle = normalizedAngle % 1.0 * 360.0;
		if (normalizedAngle < 0.0)
		{
			normalizedAngle += 360.0;
		}
		return normalizedAngle;
	}
}
