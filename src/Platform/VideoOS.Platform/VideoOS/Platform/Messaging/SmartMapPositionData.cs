using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SmartMapPositionData
{
	public double UpperLeftLatitude { get; set; }

	public double UpperLeftLongitude { get; set; }

	public double LowerRightLatitude { get; set; }

	public double LowerRightLongitude { get; set; }

	public double CenterLatitude { get; set; }

	public double CenterLongitude { get; set; }

	public double ZoomLevel { get; set; }

	public FQID Window { get; set; }

	public int Index { get; set; }
}
