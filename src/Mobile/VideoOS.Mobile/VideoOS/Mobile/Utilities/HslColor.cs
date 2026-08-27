using System.Drawing;

namespace VideoOS.Mobile.Utilities;

public class HslColor
{
	private double _hue = 1.0;

	private double _saturation = 1.0;

	private double _luminosity = 1.0;

	private const double Scale = 240.0;

	public double Hue
	{
		get
		{
			return _hue * 240.0;
		}
		set
		{
			_hue = CheckRange(value / 240.0);
		}
	}

	public double Saturation
	{
		get
		{
			return _saturation * 240.0;
		}
		set
		{
			_saturation = CheckRange(value / 240.0);
		}
	}

	public double Luminosity
	{
		get
		{
			return _luminosity * 240.0;
		}
		set
		{
			_luminosity = CheckRange(value / 240.0);
		}
	}

	private double CheckRange(double value)
	{
		if (value < 0.0)
		{
			value = 0.0;
		}
		else if (value > 1.0)
		{
			value = 1.0;
		}
		return value;
	}

	public override string ToString()
	{
		return $"H: {Hue:#0.##} S: {Saturation:#0.##} L: {Luminosity:#0.##}";
	}

	public string ToRgbString()
	{
		Color color = this;
		return $"R: {color.R:#0.##} G: {color.G:#0.##} B: {color.B:#0.##}";
	}

	public static implicit operator Color(HslColor hslColor)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		if (hslColor._luminosity != 0.0)
		{
			if (hslColor._saturation == 0.0)
			{
				num = (num2 = (num3 = hslColor._luminosity));
			}
			else
			{
				double temp = GetTemp2(hslColor);
				double temp2 = 2.0 * hslColor._luminosity - temp;
				num = GetColorComponent(temp2, temp, hslColor._hue + 1.0 / 3.0);
				num2 = GetColorComponent(temp2, temp, hslColor._hue);
				num3 = GetColorComponent(temp2, temp, hslColor._hue - 1.0 / 3.0);
			}
		}
		return Color.FromArgb((int)(255.0 * num), (int)(255.0 * num2), (int)(255.0 * num3));
	}

	private static double GetColorComponent(double temp1, double temp2, double temp3)
	{
		temp3 = MoveIntoRange(temp3);
		if (temp3 < 1.0 / 6.0)
		{
			return temp1 + (temp2 - temp1) * 6.0 * temp3;
		}
		if (temp3 < 0.5)
		{
			return temp2;
		}
		if (temp3 < 2.0 / 3.0)
		{
			return temp1 + (temp2 - temp1) * (2.0 / 3.0 - temp3) * 6.0;
		}
		return temp1;
	}

	private static double MoveIntoRange(double temp3)
	{
		if (temp3 < 0.0)
		{
			temp3 += 1.0;
		}
		else if (temp3 > 1.0)
		{
			temp3 -= 1.0;
		}
		return temp3;
	}

	private static double GetTemp2(HslColor hslColor)
	{
		if (hslColor._luminosity < 0.5)
		{
			return hslColor._luminosity * (1.0 + hslColor._saturation);
		}
		return hslColor._luminosity + hslColor._saturation - hslColor._luminosity * hslColor._saturation;
	}

	public static implicit operator HslColor(Color color)
	{
		return new HslColor
		{
			_hue = (double)color.GetHue() / 360.0,
			_luminosity = color.GetBrightness(),
			_saturation = color.GetSaturation()
		};
	}

	public void SetRgb(int red, int green, int blue)
	{
		HslColor hslColor = Color.FromArgb(red, green, blue);
		_hue = hslColor._hue;
		_saturation = hslColor._saturation;
		_luminosity = hslColor._luminosity;
	}

	public HslColor()
	{
	}

	public HslColor(Color color)
	{
		SetRgb(color.R, color.G, color.B);
	}

	public HslColor(int red, int green, int blue)
	{
		SetRgb(red, green, blue);
	}

	public HslColor(double hue, double saturation, double luminosity)
	{
		Hue = hue;
		Saturation = saturation;
		Luminosity = luminosity;
	}
}
