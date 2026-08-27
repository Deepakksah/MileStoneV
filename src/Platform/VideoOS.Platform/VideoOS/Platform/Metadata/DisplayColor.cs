using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Platform.Metadata;

public class DisplayColor : IEquatable<DisplayColor>
{
	private const byte OpaqueAlpha = byte.MaxValue;

	private static readonly Regex ArgbStringRegex = new Regex("^#([0-9A-Fa-f]{8})$");

	private readonly byte _a;

	private readonly byte _r;

	private readonly byte _g;

	private readonly byte _b;

	public byte A => _a;

	public byte R => _r;

	public byte G => _g;

	public byte B => _b;

	public string ArgbString => "#" + A.ToString("X2", MetadataXml.Culture) + R.ToString("X2", MetadataXml.Culture) + G.ToString("X2", MetadataXml.Culture) + B.ToString("X2", MetadataXml.Culture);

	public int Argb => (A << 24) | (R << 16) | (G << 8) | B;

	public DisplayColor(byte r, byte g, byte b)
	{
		_a = byte.MaxValue;
		_r = r;
		_g = g;
		_b = b;
	}

	public DisplayColor(byte a, byte r, byte g, byte b)
	{
		_a = a;
		_r = r;
		_g = g;
		_b = b;
	}

	public DisplayColor(int argb)
	{
		_a = (byte)((argb >> 24) & 0xFF);
		_r = (byte)((argb >> 16) & 0xFF);
		_g = (byte)((argb >> 8) & 0xFF);
		_b = (byte)(argb & 0xFF);
	}

	public static bool TryParseArgbString(string argbValue, out DisplayColor color)
	{
		if (argbValue == null)
		{
			throw new ArgumentNullException("argbValue");
		}
		Match match = ArgbStringRegex.Match(argbValue.Trim());
		if (!match.Success)
		{
			color = null;
			return false;
		}
		if (int.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, MetadataXml.Culture, out var result))
		{
			color = new DisplayColor(result);
			return true;
		}
		color = null;
		return false;
	}

	public static DisplayColor ParseArgbString(string argbValue)
	{
		if (TryParseArgbString(argbValue, out var color))
		{
			return color;
		}
		throw new FormatException(string.Format(CultureInfo.InvariantCulture, "ARGB string '{0}' is not in a correct format", argbValue));
	}

	public bool Equals(DisplayColor other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (_a == other._a && _r == other._r && _g == other._g)
		{
			return _b == other._b;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((DisplayColor)obj);
	}

	public override int GetHashCode()
	{
		return (((((_a.GetHashCode() * 397) ^ _r.GetHashCode()) * 397) ^ _g.GetHashCode()) * 397) ^ _b.GetHashCode();
	}
}
