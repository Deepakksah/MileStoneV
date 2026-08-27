using System;

namespace VideoOS.Platform.UI.Extensions;

public static class DoubleExtensionMethods
{
	private const double Epsilon = 1E-10;

	public static bool IsEqualTo(this double x, double y, double epsilon = 1E-10)
	{
		return Math.Abs(x - y) < epsilon;
	}

	public static bool IsNotEqualTo(this double x, double y, double epsilon = 1E-10)
	{
		return !x.IsEqualTo(y, epsilon);
	}

	public static bool IsLessThanOrEqualTo(this double x, double y, double epsilon = 1E-10)
	{
		if (!x.IsEqualTo(y, epsilon))
		{
			return x < y;
		}
		return true;
	}

	public static bool IsLessThan(this double x, double y, double epsilon = 1E-10)
	{
		if (x.IsNotEqualTo(y, epsilon))
		{
			return x < y;
		}
		return false;
	}

	public static bool IsGreaterThanOrEqualTo(this double x, double y, double epsilon = 1E-10)
	{
		if (!x.IsEqualTo(y, epsilon))
		{
			return x > y;
		}
		return true;
	}

	public static bool IsGreaterThan(this double x, double y, double epsilon = 1E-10)
	{
		if (x.IsNotEqualTo(y, epsilon))
		{
			return x > y;
		}
		return false;
	}
}
