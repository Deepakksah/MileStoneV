using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Mobile.Portable.Utilities.Enums;
using VideoOS.Mobile.Portable.Utilities.Rects;

namespace VideoOS.Mobile.Portable.Utilities;

public class Math
{
	public const float BytesInKb = 1024f;

	public const float BytesInMb = 1048576f;

	public const float BytesInGb = 1.0737418E+09f;

	public static int Compare(double first, double second, double precision = 0.001)
	{
		if (System.Math.Abs(first - second) < precision)
		{
			return 0;
		}
		if (first > second)
		{
			return 1;
		}
		return -1;
	}

	public static bool AreEqual(double doubleA, double doubleB)
	{
		return Compare(doubleA, doubleB) == 0;
	}

	public static bool IsLessOrEqual(double doubleA, double doubleB)
	{
		return Compare(doubleA, doubleB) <= 0;
	}

	public static bool IsGreaterOrEqual(double doubleA, double doubleB)
	{
		return Compare(doubleA, doubleB) >= 0;
	}

	public static bool IsGreater(double doubleA, double doubleB)
	{
		return Compare(doubleA, doubleB) > 0;
	}

	public static bool IsLess(double doubleA, double doubleB)
	{
		return Compare(doubleA, doubleB) < 0;
	}

	public static void CompareE(ref int nResult, uint nFirst, uint nSecond)
	{
		nResult = nFirst.CompareTo(nSecond);
	}

	public static void CompareE(ref int nResult, int nFirst, int nSecond)
	{
		nResult = nFirst.CompareTo(nSecond);
	}

	public static void CompareE(ref int nResult, bool first, bool second)
	{
		nResult = first.CompareTo(second);
	}

	public static void CompareE(ref int nResult, double first, double second, double accuracy)
	{
		nResult = Compare(first, second, accuracy);
	}

	public static void CompareE(ref int nResult, string first, string second)
	{
		nResult = string.Compare(first, second, StringComparison.CurrentCultureIgnoreCase);
	}

	public static void CompareE(ref int nResult, Guid first, Guid second)
	{
		nResult = first.CompareTo(second);
	}

	public static void CompareE(ref int nResult, DateTime first, DateTime second)
	{
		nResult = first.CompareTo(second);
	}

	public static void CompareE(ref int nResult, TimeSpan first, TimeSpan second)
	{
		nResult = first.CompareTo(second);
	}

	public static void CompareE(ref int nResult, IntRect first, IntRect second)
	{
		nResult = 1;
		CompareE(ref nResult, first.Width, second.Width);
		if (nResult != 0)
		{
			return;
		}
		CompareE(ref nResult, first.Height, second.Height);
		if (nResult == 0)
		{
			CompareE(ref nResult, first.X, second.X);
			if (nResult == 0)
			{
				CompareE(ref nResult, first.Y, second.Y);
			}
		}
	}

	public static void CompareE(ref int nResult, FloatRect first, FloatRect second, double accuracy)
	{
		nResult = 1;
		CompareE(ref nResult, first.Left, second.Left, accuracy);
		if (nResult != 0)
		{
			return;
		}
		CompareE(ref nResult, first.Top, second.Top, accuracy);
		if (nResult == 0)
		{
			CompareE(ref nResult, first.Right, second.Right, accuracy);
			if (nResult == 0)
			{
				CompareE(ref nResult, first.Bottom, second.Bottom, accuracy);
			}
		}
	}

	public static void CompareE(ref int nResult, IEnumerable<Guid> first, IEnumerable<Guid> second)
	{
		if (first == null && second == null)
		{
			nResult = 0;
			return;
		}
		if (first == null && second != null)
		{
			nResult = -1;
			return;
		}
		if (first != null && second == null)
		{
			nResult = 1;
			return;
		}
		CompareE(ref nResult, first.Count(), second.Count());
		if (nResult != 0)
		{
			return;
		}
		foreach (var item in first.Zip(second, (Guid f, Guid s) => (f: f, s: s)))
		{
			CompareE(ref nResult, item.f, item.s);
			if (nResult != 0)
			{
				break;
			}
		}
	}

	public static T Min<T>(T first, T second)
	{
		if (Comparer<T>.Default.Compare(first, second) > 0)
		{
			return second;
		}
		return first;
	}

	public static T Max<T>(T first, T second)
	{
		if (Comparer<T>.Default.Compare(first, second) <= 0)
		{
			return second;
		}
		return first;
	}

	public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
	{
		if (value.CompareTo(min) >= 0)
		{
			if (value.CompareTo(max) <= 0)
			{
				return value;
			}
			return max;
		}
		return min;
	}

	public static double PutInRange(double start, double end, double value)
	{
		double num = end - start;
		long num2 = (long)((value - start) / num);
		if (value < start)
		{
			num2--;
		}
		return value - (double)num2 * num;
	}

	public static Tuple<float, DataMeasurementUnit> GetByteSizeInAppropriateMeasurementUnit(long sizeBytes)
	{
		float num = 0f;
		DataMeasurementUnit dataMeasurementUnit = DataMeasurementUnit.Byte;
		if ((float)sizeBytes < 1024f)
		{
			num = sizeBytes;
			dataMeasurementUnit = DataMeasurementUnit.Byte;
		}
		else if ((float)sizeBytes < 1048576f)
		{
			num = (float)sizeBytes / 1024f;
			dataMeasurementUnit = DataMeasurementUnit.Kilobyte;
		}
		else if ((float)sizeBytes < 1.0737418E+09f)
		{
			num = (float)sizeBytes / 1048576f;
			dataMeasurementUnit = DataMeasurementUnit.Megabyte;
		}
		else
		{
			num = (float)sizeBytes / 1.0737418E+09f;
			dataMeasurementUnit = DataMeasurementUnit.Gigabyte;
		}
		return Tuple.Create(num, dataMeasurementUnit);
	}

	public static string GetByteSizeInAppropriateMeasureUnit(long sizeBytes)
	{
		Tuple<float, DataMeasurementUnit> byteSizeInAppropriateMeasurementUnit = GetByteSizeInAppropriateMeasurementUnit(sizeBytes);
		return string.Format("{0} {1}", byteSizeInAppropriateMeasurementUnit.Item1.ToString("F", CultureInfo.InvariantCulture), EnumsHelper.GetDescription(byteSizeInAppropriateMeasurementUnit.Item2));
	}
}
