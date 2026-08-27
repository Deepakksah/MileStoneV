using System;

namespace VideoOS.Utilities.Colors;

public static class ColorConverter
{
	public struct RGB(byte r, byte g, byte b)
	{
		public byte R { get; } = r;

		public byte G { get; } = g;

		public byte B { get; } = b;

		public bool Equals(RGB rgb)
		{
			if (R == rgb.R && G == rgb.G)
			{
				return B == rgb.B;
			}
			return false;
		}

		public HSL ToHSL()
		{
			double num = (float)(int)R / 255f;
			double num2 = (float)(int)G / 255f;
			double num3 = (float)(int)B / 255f;
			double num4 = System.Math.Min(System.Math.Min(num, num2), num3);
			double num5 = System.Math.Max(System.Math.Max(num, num2), num3);
			double num6 = num5 - num4;
			double num7 = (num5 + num4) / 2.0;
			double h;
			double s;
			if (num6 == 0.0)
			{
				h = 0.0;
				s = 0.0;
			}
			else
			{
				s = ((num7 <= 0.5) ? (num6 / (num5 + num4)) : (num6 / (2.0 - num5 - num4)));
				double num8 = ((num == num5) ? ((num2 - num3) / 6.0 / num6) : ((num2 != num5) ? (0.6666666865348816 + (num - num2) / 6.0 / num6) : (0.3333333432674408 + (num3 - num) / 6.0 / num6)));
				if (num8 < 0.0)
				{
					num8 += 1.0;
				}
				if (num8 > 1.0)
				{
					num8 -= 1.0;
				}
				h = (int)(num8 * 360.0);
			}
			return new HSL(h, s, num7);
		}
	}

	public struct HSL
	{
		public double H { get; }

		public double S { get; }

		public double L { get; }

		public HSL(double h, double s, double l)
		{
			if (h < 0.0 || h > 365.0)
			{
				throw new ArgumentException("H must be a degree betwen 0 and 360");
			}
			if (s < 0.0 || s > 100.0 || l < 0.0 || l > 100.0)
			{
				throw new ArgumentException("S and L must be a percentage between 0 and 100");
			}
			H = h;
			S = s;
			L = l;
		}

		public HSL(string hslString)
		{
			if (string.IsNullOrEmpty(hslString))
			{
				throw new ArgumentNullException("hslString");
			}
			double[] array = StringArrayToHsl(hslString);
			H = array[0];
			S = array[1] / 100.0;
			L = array[2] / 100.0;
		}

		public bool Equals(HSL hsl)
		{
			if (H == hsl.H && S == hsl.S)
			{
				return L == hsl.L;
			}
			return false;
		}

		public RGB ToRGB()
		{
			int num;
			int num2;
			int num3;
			if (S == 0.0)
			{
				num = (num2 = (num3 = (int)(L * 255.0)));
			}
			else
			{
				double num4 = H / 360.0;
				double num5 = ((L < 0.5) ? (L * (1.0 + S)) : (L + S - L * S));
				double v = 2.0 * L - num5;
				num = (int)(255.0 * HueToRGB(v, num5, num4 + 1.0 / 3.0));
				num2 = (int)(255.0 * HueToRGB(v, num5, num4));
				num3 = (int)(255.0 * HueToRGB(v, num5, num4 - 1.0 / 3.0));
			}
			return new RGB((byte)num, (byte)num2, (byte)num3);
		}
	}

	private static double[] StringArrayToHsl(string baseHslColor)
	{
		string[] array = baseHslColor.Split(',');
		if (array.Length != 3)
		{
			throw new ArgumentException("HSL string must be three comma-delimited integers");
		}
		double.TryParse(array[0], out var result);
		double.TryParse(array[1], out var result2);
		double.TryParse(array[2], out var result3);
		return new double[3] { result, result2, result3 };
	}

	private static double HueToRGB(double v1, double v2, double vH)
	{
		if (vH < 0.0)
		{
			vH += 1.0;
		}
		else if (vH > 1.0)
		{
			vH -= 1.0;
		}
		if (6.0 * vH < 1.0)
		{
			return v1 + (v2 - v1) * 6.0 * vH;
		}
		if (2.0 * vH < 1.0)
		{
			return v2;
		}
		if (3.0 * vH < 2.0)
		{
			return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
		}
		return v1;
	}
}
