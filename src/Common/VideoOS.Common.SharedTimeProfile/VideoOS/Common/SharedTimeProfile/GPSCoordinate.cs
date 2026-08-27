using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Common.SharedTimeProfile;

public class GPSCoordinate
{
	public static class GpsParsingUtilities
	{
		public static double ConvertGpsValue(string gpsValue, int index, int length)
		{
			int result2;
			double result;
			switch (length)
			{
			case 3:
				switch (index)
				{
				case 0:
				case 1:
					if (!int.TryParse(gpsValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result2))
					{
						throw new ArgumentException("Invalid integer value: " + gpsValue);
					}
					return result2;
				case 2:
					if (!double.TryParse(gpsValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
					{
						throw new ArgumentException("Invalid double value: " + gpsValue);
					}
					return result;
				default:
					throw new ArgumentException("Index " + index.ToString(CultureInfo.InvariantCulture) + " out of range, Length=" + length.ToString(CultureInfo.InvariantCulture));
				}
			case 2:
				switch (index)
				{
				case 0:
					if (!int.TryParse(gpsValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out result2))
					{
						throw new ArgumentException("Invalid integer value: " + gpsValue);
					}
					return result2;
				case 1:
					if (!double.TryParse(gpsValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
					{
						throw new ArgumentException("Invalid double value: " + gpsValue);
					}
					return result;
				default:
					throw new ArgumentException("Index " + index.ToString(CultureInfo.InvariantCulture) + " out of range, Length=" + length.ToString(CultureInfo.InvariantCulture));
				}
			case 1:
				if (index == 0)
				{
					if (!double.TryParse(gpsValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
					{
						throw new ArgumentException("Invalid double value: " + gpsValue);
					}
					return result;
				}
				throw new ArgumentException("Index " + index.ToString(CultureInfo.InvariantCulture) + " out of range, Length=" + length.ToString(CultureInfo.InvariantCulture));
			default:
				throw new ArgumentException("Invalid number of elements in latitude or longitude: " + length.ToString(CultureInfo.InvariantCulture));
			}
		}

		public static void GpsParser(string GpsCoordinate, out double latitude, out double longitude)
		{
			latitude = 0.0;
			longitude = 0.0;
			if (string.IsNullOrEmpty(GpsCoordinate))
			{
				throw new FormatException("Empty GPS coordinate string");
			}
			GpsCoordinate += " ";
			GpsCoordinate = GpsCoordinate.ToUpperInvariant();
			int num = 0;
			for (int i = 0; i < GpsCoordinate.Length; i++)
			{
				if ((GpsCoordinate[i] < '0' || GpsCoordinate[i] > '9') && GpsCoordinate[i] != '+' && GpsCoordinate[i] != '-' && GpsCoordinate[i] != 'N' && GpsCoordinate[i] != 'S' && GpsCoordinate[i] != 'E' && GpsCoordinate[i] != 'W' && GpsCoordinate[i] != '.' && GpsCoordinate[i] != ',' && GpsCoordinate[i] != '\'' && GpsCoordinate[i] != ' ')
				{
					throw new FormatException("Invalid character in GPS coordinate string: " + GpsCoordinate[i]);
				}
				if (i > 0 && (GpsCoordinate[i] == '+' || GpsCoordinate[i] == '-' || GpsCoordinate[i] == 'N' || GpsCoordinate[i] == 'S' || GpsCoordinate[i] == 'E' || GpsCoordinate[i] == 'W') && GpsCoordinate[i - 1] != ' ' && GpsCoordinate[i - 1] != ',')
				{
					throw new FormatException("Invalid character in front of +-NSEW: " + GpsCoordinate[i - 1]);
				}
				if (GpsCoordinate[i] == '+' || GpsCoordinate[i] == '-' || GpsCoordinate[i] == 'N' || GpsCoordinate[i] == 'S' || GpsCoordinate[i] == 'E' || GpsCoordinate[i] == 'W')
				{
					num++;
				}
			}
			MatchCollection matchCollection = new Regex(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("" + "\\s*", "[+]?"), "[-]?"), "[N]?"), "[S]?"), "[E]?"), "[W]?"), "\\s?"), "\\d+"), "[.]?"), "\\d*"), "[',\\s]+"), RegexOptions.None).Matches(GpsCoordinate);
			if (matchCollection.Count < 2)
			{
				throw new FormatException("Invalid GPS syntax or invalid number of GPS coordinate values");
			}
			List<string> list = new List<string>();
			for (int j = 0; j < matchCollection.Count; j++)
			{
				list.Add(matchCollection[j].Value);
				list[j] = Regex.Replace(list[j], "\\s+", "");
			}
			bool flag = true;
			if (list[0][0] == 'N' || list[0][0] == 'S')
			{
				flag = true;
			}
			else if (list[0][0] == 'E' || list[0][0] == 'W')
			{
				flag = false;
			}
			int num2 = -1;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			for (int k = 0; k < matchCollection.Count; k++)
			{
				if (list[k][0] == '+' || list[k][0] == '-')
				{
					if (matchCollection.Count > 2)
					{
						throw new FormatException("+ and - are not allowed in front of the present entered GPS coordinate values");
					}
					if (flag3)
					{
						throw new FormatException("GPS coordinates may use only use a set of (+,-) or (N,S,E,W)");
					}
					if (!flag4)
					{
						flag4 = true;
					}
					else if (!flag5)
					{
						flag5 = true;
						num2 = k;
					}
					flag2 = true;
				}
				else if (list[k][0] >= '0' && list[k][0] <= '9')
				{
					if (matchCollection.Count <= 2)
					{
						if (flag3)
						{
							throw new FormatException("GPS coordinates may use only use a set of (+,-) or (N,S,E,W)");
						}
						if (!flag4)
						{
							flag4 = true;
						}
						else if (!flag5)
						{
							flag5 = true;
							num2 = k;
						}
						flag6 = true;
						flag2 = true;
					}
				}
				else if (list[k][0] == 'N' || list[k][0] == 'S')
				{
					if (flag2)
					{
						throw new FormatException("GPS coordinates may use only use a set of (+,-) or (N,S,E,W)");
					}
					if (flag4)
					{
						throw new FormatException("Latitude specified twice");
					}
					flag4 = true;
					if (!flag)
					{
						num2 = k;
					}
					flag3 = true;
				}
				else if (list[k][0] == 'E' || list[k][0] == 'W')
				{
					if (flag2)
					{
						throw new FormatException("GPS coordinates may use only use a set of (+,-) or (N,S,E,W)");
					}
					if (flag5)
					{
						throw new FormatException("Longitude specified twice");
					}
					flag5 = true;
					if (flag)
					{
						num2 = k;
					}
					flag3 = true;
				}
			}
			if (!flag4)
			{
				throw new FormatException("Latitude not specified");
			}
			if (!flag5)
			{
				throw new FormatException("Longitude not specified");
			}
			if (num2 < 0)
			{
				throw new FormatException("Invalid separation of latitude and longitude");
			}
			if ((num >= 2 && flag6) || num >= 3)
			{
				throw new FormatException("Invalid number of GPS directions: +,-,N,S,E and W");
			}
			bool flag7 = true;
			bool flag8 = true;
			if (flag)
			{
				if (list[0][0] == 'N' || list[0][0] == '+')
				{
					flag7 = true;
				}
				else if (list[0][0] == 'S' || list[0][0] == '-')
				{
					flag7 = false;
				}
				if (list[num2][0] == 'E' || list[num2][0] == '+')
				{
					flag8 = true;
				}
				else if (list[num2][0] == 'W' || list[num2][0] == '-')
				{
					flag8 = false;
				}
			}
			else
			{
				if (list[0][0] == 'E' || list[0][0] == '+')
				{
					flag8 = true;
				}
				else if (list[0][0] == 'W' || list[0][0] == '-')
				{
					flag8 = false;
				}
				if (list[num2][0] == 'N' || list[num2][0] == '+')
				{
					flag7 = true;
				}
				else if (list[num2][0] == 'S' || list[num2][0] == '-')
				{
					flag7 = false;
				}
			}
			for (int l = 0; l < matchCollection.Count; l++)
			{
				if (list[l][list[l].Length - 1] == ',' && l != num2 - 1)
				{
					throw new FormatException(", is only allowed as separator between the two blocks of GPS coordinates");
				}
			}
			for (int m = 0; m < matchCollection.Count; m++)
			{
				list[m] = Regex.Replace(list[m], "[+]?", "");
				list[m] = Regex.Replace(list[m], "[-]?", "");
				list[m] = Regex.Replace(list[m], "[N]?", "");
				list[m] = Regex.Replace(list[m], "[S]?", "");
				list[m] = Regex.Replace(list[m], "[E]?", "");
				list[m] = Regex.Replace(list[m], "[W]?", "");
				list[m] = Regex.Replace(list[m], "[\\,]*", "");
				list[m] = Regex.Replace(list[m], "[\\']*", "");
			}
			double num3 = 0.0;
			for (int n = 0; n < num2; n++)
			{
				double num4 = ConvertGpsValue(list[n], n, num2);
				switch (n)
				{
				case 1:
					num4 /= 60.0;
					break;
				case 2:
					num4 /= 3600.0;
					break;
				}
				num3 += num4;
			}
			double num5 = 0.0;
			for (int num6 = num2; num6 < matchCollection.Count; num6++)
			{
				double num7 = ConvertGpsValue(list[num6], num6 - num2, matchCollection.Count - num2);
				switch (num6 - num2)
				{
				case 1:
					num7 /= 60.0;
					break;
				case 2:
					num7 /= 3600.0;
					break;
				}
				num5 += num7;
			}
			if (flag)
			{
				if (flag7)
				{
					latitude = num3;
				}
				else
				{
					latitude = 0.0 - num3;
				}
				if (flag8)
				{
					longitude = num5;
				}
				else
				{
					longitude = 0.0 - num5;
				}
			}
			else
			{
				if (flag8)
				{
					longitude = num3;
				}
				else
				{
					longitude = 0.0 - num3;
				}
				if (flag7)
				{
					latitude = num5;
				}
				else
				{
					latitude = 0.0 - num5;
				}
			}
			CheckLatitudeValue(latitude);
			CheckLongitudeValue(longitude);
		}
	}

	private double latitude;

	private double longitude;

	private string displayName;

	public double Latitude
	{
		get
		{
			return latitude;
		}
		private set
		{
			CheckLatitudeValue(value);
			latitude = value;
		}
	}

	public double Longitude
	{
		get
		{
			return longitude;
		}
		private set
		{
			CheckLongitudeValue(value);
			longitude = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return displayName;
		}
		private set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			displayName = value;
		}
	}

	public GPSCoordinate(double latitude, double longitude)
	{
		Latitude = latitude;
		Longitude = longitude;
		DisplayName = string.Format(CultureInfo.InvariantCulture, "{0} {1}", latitude, longitude);
	}

	public GPSCoordinate(string gpsCoordinate)
	{
		DisplayName = gpsCoordinate;
		GpsParsingUtilities.GpsParser(gpsCoordinate, out var num, out var num2);
		Latitude = num;
		Longitude = num2;
	}

	public static bool Validate(string gpsCoordinate)
	{
		try
		{
			GpsParsingUtilities.GpsParser(gpsCoordinate, out var _, out var _);
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return false;
	}

	private static void CheckLatitudeValue(double gpsValue)
	{
		if (gpsValue > 90.0 || gpsValue < -90.0)
		{
			throw new ArgumentOutOfRangeException("gpsValue", "Latitude must be between -90 and 90");
		}
	}

	private static void CheckLongitudeValue(double gpsValue)
	{
		if (gpsValue > 180.0 || gpsValue < -180.0)
		{
			throw new ArgumentOutOfRangeException("gpsValue", "Longitude must be between -180 and 180");
		}
	}
}
