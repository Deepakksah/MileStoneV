using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;
using VideoOS.Management.Common;

namespace VideoOS.Administration.AddIn.Util;

public static class GisPointHelper
{
	public const string GisPointEmpty = "POINT EMPTY";

	public static string GisPointToDisplayText(string gisPoint)
	{
		if (!string.IsNullOrWhiteSpace(gisPoint))
		{
			try
			{
				SqlGeometry sqlGeometry = SqlGeometry.STPointFromText(new SqlChars(gisPoint.ToCharArray()), 4326);
				GpsCoordinate gpsCoordinate = new GpsCoordinate(sqlGeometry.STY.Value, sqlGeometry.STX.Value);
				return gpsCoordinate.ToString();
			}
			catch (Exception)
			{
			}
		}
		return string.Empty;
	}

	public static bool TryDisplayTextToGisPoint(string displayText, out string gisPoint)
	{
		if (string.IsNullOrWhiteSpace(displayText))
		{
			gisPoint = "POINT EMPTY";
			return true;
		}
		try
		{
			GpsCoordinate gpsCoordinate = new GpsCoordinate(displayText);
			SqlGeometry sqlGeometry = SqlGeometry.Point(gpsCoordinate.Longitude, gpsCoordinate.Latitude, 4326);
			gisPoint = new string(sqlGeometry.STAsText().Value);
			return true;
		}
		catch (Exception)
		{
		}
		gisPoint = string.Empty;
		return false;
	}
}
