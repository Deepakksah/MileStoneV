using System;
using System.Globalization;

namespace VideoOS.Platform;

[Serializable]
public class PositioningInformation
{
	private string _gisPoint;

	public bool IsPointDefined { get; private set; }

	public double Latitude { get; private set; }

	public double Longitude { get; private set; }

	public double CoverageDirection { get; private set; }

	public double CoverageDepth { get; private set; }

	public double CoverageFieldOfView { get; private set; }

	public PositioningInformation()
	{
		IsPointDefined = false;
	}

	public PositioningInformation(string gisPoint, double coverageDirection, double coverageDepth, double coverageFieldOfView)
	{
		double result = 0.0;
		double result2 = 0.0;
		_gisPoint = gisPoint;
		if (!string.IsNullOrEmpty(gisPoint))
		{
			if (gisPoint.StartsWith("POINT EMPTY", StringComparison.OrdinalIgnoreCase))
			{
				IsPointDefined = false;
			}
			else
			{
				gisPoint = gisPoint.Replace(", ", ",");
				if (gisPoint.StartsWith("POINT (", StringComparison.OrdinalIgnoreCase))
				{
					gisPoint = gisPoint.Substring("POINT (".Length);
					gisPoint = gisPoint.TrimEnd(')');
				}
				if (gisPoint.StartsWith("POINT(", StringComparison.OrdinalIgnoreCase))
				{
					gisPoint = gisPoint.Substring("POINT(".Length);
					gisPoint = gisPoint.TrimEnd(')');
				}
				string[] array = gisPoint.Trim().Split(',', ' ');
				if (array.Length > 2 && string.IsNullOrWhiteSpace(array[1]))
				{
					array[1] = array[2];
				}
				if (array.Length >= 2)
				{
					IsPointDefined = double.TryParse(array[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out result2) && double.TryParse(array[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
				}
			}
		}
		Latitude = result;
		Longitude = result2;
		CoverageDirection = coverageDirection;
		CoverageDepth = coverageDepth;
		CoverageFieldOfView = coverageFieldOfView;
	}

	public PositioningInformation(double latitude, double longitude, double coverageDirection, double coverageDepth, double coverageFieldOfView)
	{
		Latitude = latitude;
		Longitude = longitude;
		CoverageDirection = coverageDirection;
		CoverageDepth = coverageDepth;
		CoverageFieldOfView = coverageFieldOfView;
		IsPointDefined = latitude != 0.0 || longitude != 0.0;
		if (IsPointDefined)
		{
			_gisPoint = string.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT ({0},{1})", longitude, latitude);
		}
		else
		{
			_gisPoint = "POINT EMPTY";
		}
	}

	public override string ToString()
	{
		return _gisPoint;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is PositioningInformation positioningInformation))
		{
			return false;
		}
		if (positioningInformation.IsPointDefined == IsPointDefined && positioningInformation.Latitude == Latitude && positioningInformation.Longitude == Longitude && positioningInformation.CoverageDirection == CoverageDirection && positioningInformation.CoverageFieldOfView == CoverageFieldOfView)
		{
			return positioningInformation.CoverageDepth == CoverageDepth;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (IsPointDefined ? 1 : 0) + Latitude.GetHashCode() + Longitude.GetHashCode() + CoverageDirection.GetHashCode() + CoverageFieldOfView.GetHashCode() + CoverageDepth.GetHashCode();
	}

	public static bool operator ==(PositioningInformation position1, PositioningInformation position2)
	{
		return position1?.Equals(position2) ?? ((object)position2 == null);
	}

	public static bool operator !=(PositioningInformation position1, PositioningInformation position2)
	{
		return !(position1 == position2);
	}
}
