using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Config;

public abstract class ItemConfigBase
{
	private readonly string _keyPrefix;

	private static readonly char[] CommaSeparatorArray = new char[1] { ',' };

	protected Dictionary<string, string> Properties { get; private set; }

	protected ItemConfigBase(Dictionary<string, string> properties, string keyPrefix)
	{
		Properties = properties;
		_keyPrefix = keyPrefix;
	}

	protected virtual string GetProperty(string key, string defaultValue)
	{
		if (!Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			return defaultValue;
		}
		return value;
	}

	protected virtual bool GetProperty(string key, bool defaultValue)
	{
		if (!Properties.TryGetValue(_keyPrefix + key, out var value) || !bool.TryParse(value, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	protected virtual int GetProperty(string key, int defaultValue)
	{
		if (!Properties.TryGetValue(_keyPrefix + key, out var value) || !int.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	protected virtual float GetProperty(string key, float defaultValue)
	{
		if (!Properties.TryGetValue(_keyPrefix + key, out var value) || !float.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	protected virtual Size GetProperty(string key, Size defaultValue)
	{
		Size result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			string[] array = value.Split(',');
			if (array.Length == 2 && int.TryParse(array[0], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result2) && int.TryParse(array[1], NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result3))
			{
				result = new Size(result2, result3);
			}
		}
		return result;
	}

	protected virtual RectangleF GetProperty(string key, RectangleF defaultValue)
	{
		RectangleF result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			string[] array = value.Split(',');
			if (array.Length == 4 && float.TryParse(array[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result2) && float.TryParse(array[1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result3) && float.TryParse(array[2], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result4) && float.TryParse(array[3], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result5))
			{
				result = new RectangleF(result2, result3, result4, result5);
			}
		}
		return result;
	}

	protected virtual PointF GetProperty(string key, PointF defaultValue)
	{
		PointF result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			string[] array = value.Split(',');
			if (array.Length == 2 && float.TryParse(array[0], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result2) && float.TryParse(array[1], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var result3))
			{
				result = new PointF(result2, result3);
			}
		}
		return result;
	}

	protected virtual FQID GetProperty(string key, FQID defaultValue)
	{
		FQID result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			try
			{
				result = new FQID(value);
			}
			catch
			{
			}
		}
		return result;
	}

	protected virtual byte[] GetProperty(string key, byte[] defaultValue)
	{
		byte[] result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			try
			{
				result = Convert.FromBase64String(value);
			}
			catch (FormatException)
			{
			}
		}
		return result;
	}

	protected virtual string[] GetProperty(string key, string[] defaultValue)
	{
		string[] result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			result = value.Split(CommaSeparatorArray, StringSplitOptions.RemoveEmptyEntries);
		}
		return result;
	}

	protected virtual Guid[] GetProperty(string key, Guid[] defaultValue)
	{
		Guid[] result = defaultValue;
		if (Properties.TryGetValue(_keyPrefix + key, out var value))
		{
			result = new List<string>(value.Split(',')).ConvertAll((string x) => (!Guid.TryParse(x, out var id)) ? Guid.Empty : id).ToArray();
		}
		return result;
	}

	protected virtual void SetProperty(string key, string value)
	{
		Properties[_keyPrefix + key] = value;
	}

	protected virtual void SetProperty(string key, bool value)
	{
		Properties[_keyPrefix + key] = value.ToString(NumberFormatInfo.InvariantInfo);
	}

	protected virtual void SetProperty(string key, int value)
	{
		Properties[_keyPrefix + key] = value.ToString(NumberFormatInfo.InvariantInfo);
	}

	protected virtual void SetProperty(string key, float value)
	{
		Properties[_keyPrefix + key] = value.ToString("0.###", NumberFormatInfo.InvariantInfo);
	}

	protected virtual void SetProperty(string key, Size value)
	{
		Properties[_keyPrefix + key] = string.Format(NumberFormatInfo.InvariantInfo, "{0},{1}", value.Width, value.Height);
	}

	protected virtual void SetProperty(string key, RectangleF value)
	{
		Properties[_keyPrefix + key] = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.###},{1:0.###},{2:0.###},{3:0.###}", value.X, value.Y, value.Width, value.Height);
	}

	protected virtual void SetProperty(string key, PointF value)
	{
		Properties[_keyPrefix + key] = string.Format(NumberFormatInfo.InvariantInfo, "{0:0.###},{1:0.###}", value.X, value.Y);
	}

	protected virtual void SetProperty(string key, FQID value)
	{
		Properties[_keyPrefix + key] = ((value != null) ? value.ToXmlNode().OuterXml : string.Empty);
	}

	protected virtual void SetProperty(string key, byte[] value)
	{
		Properties[_keyPrefix + key] = ((value != null) ? Convert.ToBase64String(value) : string.Empty);
	}

	protected virtual void SetProperty(string key, string[] value)
	{
		Properties[_keyPrefix + key] = ((value != null) ? string.Join(",", value) : string.Empty);
	}

	protected virtual void SetProperty(string key, Guid[] value)
	{
		Properties[_keyPrefix + key] = ((value != null) ? string.Join(",", value) : string.Empty);
	}
}
