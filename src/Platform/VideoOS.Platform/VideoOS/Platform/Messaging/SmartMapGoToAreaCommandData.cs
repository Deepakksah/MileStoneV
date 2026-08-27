using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SmartMapGoToAreaCommandData
{
	public double UpperLeftLatitude { get; set; }

	public double UpperLeftLongitude { get; set; }

	public double LowerRightLatitude { get; set; }

	public double LowerRightLongitude { get; set; }

	public FQID Window { get; set; }

	public int Index { get; set; }
}
