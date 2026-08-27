using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities.Rects;

namespace VideoOS.Mobile.Portable.Utilities;

public class ParamsHelper
{
	public delegate T Convertion<T>(string szInput);

	public delegate T Convertions<T>(Dictionary<string, string> szInputs);

	public delegate string ConvToString<T>(T obj);

	public delegate Dictionary<string, string> ConvToStrings<T>(T obj);

	public delegate void CopyMethod<T>(ref T tDestination, ref T tSource);

	public delegate void CompareFunc<T>(ref int nResult, T first, T second);

	private static readonly NumberFormatInfo Provider = new NumberFormatInfo
	{
		NumberDecimalSeparator = "."
	};

	public static void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string szKey, ref T tOutput, uint unSuccMask, Convertion<T> convFunc, uint unMaskToCheck, ref uint paramMask, bool bLog = true)
	{
		try
		{
			if ((unMaskToCheck & unSuccMask) == unSuccMask && dictInputParams.ContainsKey(szKey))
			{
				tOutput = convFunc(dictInputParams[szKey]);
				paramMask |= unSuccMask;
			}
		}
		catch (Exception ex)
		{
			if (bLog)
			{
				Logger.Instance.Log(LogContent.Error, "ParamsHelper.ConvertParameter<T> : " + szKey + " : " + typeof(T).FullName, ex.Message);
			}
		}
	}

	public static void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string[] szKeys, ref T tOutput, uint unSuccMask, Convertions<T> convFunc, uint unMaskToCheck, ref uint paramMask, bool bLog = true)
	{
		try
		{
			if ((unMaskToCheck & unSuccMask) == unSuccMask && szKeys.All((string k) => dictInputParams.ContainsKey(k)))
			{
				tOutput = convFunc(dictInputParams.Where((KeyValuePair<string, string> kvp) => Enumerable.Contains(szKeys, kvp.Key)).ToDictionary((KeyValuePair<string, string> kvp) => kvp.Key, (KeyValuePair<string, string> kvp) => kvp.Value));
				paramMask |= unSuccMask;
			}
		}
		catch (Exception ex)
		{
			if (bLog)
			{
				Logger.Instance.Log(LogContent.Error, "ParamsHelper.ConvertParameter<T> : " + szKeys.Aggregate((string i, string j) => i + " ," + j) + " : " + typeof(T).FullName, ex.Message);
			}
		}
	}

	public static void ConvertParameter<T>(Dictionary<string, string> dictInputParams, string[] requiredSzKeys, string[] optionalSzKeys, ref T tOutput, uint unSuccMask, Convertions<T> convFunc, uint unMaskToCheck, ref uint paramMask, bool bLog = true)
	{
		try
		{
			if ((unMaskToCheck & unSuccMask) == unSuccMask && requiredSzKeys.All((string k) => dictInputParams.ContainsKey(k)))
			{
				tOutput = convFunc(dictInputParams.Where((KeyValuePair<string, string> kvp) => Enumerable.Contains(requiredSzKeys, kvp.Key) || Enumerable.Contains(optionalSzKeys, kvp.Key)).ToDictionary((KeyValuePair<string, string> kvp) => kvp.Key, (KeyValuePair<string, string> kvp) => kvp.Value));
				paramMask |= unSuccMask;
			}
		}
		catch (Exception ex)
		{
			if (bLog)
			{
				Logger.Instance.Log(LogContent.Error, "ParamsHelper.ConvertParameter<T> : " + requiredSzKeys.Aggregate((string i, string j) => i + " ," + j) + " : " + typeof(T).FullName, ex.Message);
			}
		}
	}

	public static string StringConvertion(string szInput)
	{
		return szInput;
	}

	public static Guid GuidConvertion(string szInput)
	{
		return new Guid(szInput);
	}

	public static double DoubleConvertion(string szInput)
	{
		double num = Convert.ToDouble(szInput, Provider);
		if (!double.IsNaN(num))
		{
			return num;
		}
		throw new ArgumentException("Double value is NaN");
	}

	public static bool BoolConvertion(string szInput)
	{
		return szInput.Equals("Yes", StringComparison.OrdinalIgnoreCase);
	}

	public static TimeSpan TimeSpanConvertion(string szInput)
	{
		return TimeSpan.FromMilliseconds(Convert.ToDouble(szInput, Provider));
	}

	public static long LongConvertion(string szInput)
	{
		long.TryParse(szInput, out var result);
		return result;
	}

	public static DateTime TimeConvertion(string value)
	{
		if (!(value == string.Empty))
		{
			return TimeConverter.FromString(value);
		}
		return TimeConverter.Dt1970;
	}

	public static void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string szKey, T input, uint uCheckMask, uint paramMask, ConvToString<T> converter = null)
	{
		try
		{
			if ((paramMask & uCheckMask) == uCheckMask)
			{
				if (converter != null)
				{
					dictInputParams[szKey] = converter(input);
				}
				else
				{
					dictInputParams[szKey] = input.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ParamsHelper.ConvertParameterTo<T> : " + szKey + " : " + typeof(T).FullName, ex.Message);
		}
	}

	public static void ConvertParameterTo<T>(Dictionary<string, string> dictInputParams, string[] szKeys, T input, uint uCheckMask, uint paramMask, ConvToStrings<T> converter)
	{
		try
		{
			if ((paramMask & uCheckMask) != uCheckMask || converter == null)
			{
				return;
			}
			Dictionary<string, string> parameters = converter(input);
			if (parameters == null || parameters.Count <= 0)
			{
				return;
			}
			szKeys.Foreach(delegate(string key)
			{
				if (parameters.ContainsKey(key))
				{
					if (dictInputParams.ContainsKey(key))
					{
						dictInputParams[key] = parameters[key];
					}
					else
					{
						dictInputParams.Add(key, parameters[key]);
					}
				}
			});
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ParamsHelper.ConvertParameterTo<T> : " + szKeys.Aggregate((string i, string j) => i + " ," + j) + " : " + typeof(T).FullName, ex.Message);
		}
	}

	public static string BoolConvertionTo(bool value)
	{
		if (!value)
		{
			return "No";
		}
		return "Yes";
	}

	public static string DoubleConvertionTo(double value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}

	public static string TimeSpanConvertionTo(TimeSpan value)
	{
		return value.TotalMilliseconds.ToString();
	}

	public static string LongConvertionTo(long value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}

	public static string TimeConvertionTo(DateTime value)
	{
		if (!(value == TimeConverter.Dt1970))
		{
			return value.ToMilisecString();
		}
		return string.Empty;
	}

	public static void SetField<T>(ref T tToSet, ref T tField, uint paramMask, uint tCheckMask, ref uint tSetMask, CopyMethod<T> copyFunc = null)
	{
		if ((paramMask & (int)tCheckMask) == (int)tCheckMask)
		{
			if (copyFunc != null)
			{
				copyFunc(ref tToSet, ref tField);
			}
			else
			{
				tToSet = tField;
			}
			tSetMask |= tCheckMask;
		}
	}

	public static void CopyRects(ref IntRect tDest, ref IntRect tSource)
	{
		tDest = new IntRect(tSource.X, tSource.Y, tSource.Width, tSource.Height, 0);
	}

	public static void CopyFloatRects(ref FloatRectExt tDest, ref FloatRectExt tSource)
	{
		tDest = new FloatRectExt(tSource);
	}

	public static void CompareCheck<T>(ref int nResult, T first, T second, CompareFunc<T> func, uint unMaskToCheck, uint tMask)
	{
		if ((tMask & unMaskToCheck) == tMask)
		{
			func(ref nResult, first, second);
		}
	}

	public static ErrorCodes CheckParams(uint tMaskToCheck, uint paramMask)
	{
		if ((paramMask & tMaskToCheck) != tMaskToCheck)
		{
			return ErrorCodes.WrongInputParameter;
		}
		return ErrorCodes.Ok;
	}
}
