using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SmartMapGoToPositionCommandData
{
	public double Latitude { get; set; }

	public double Longitude { get; set; }

	public double ZoomLevel { get; set; }

	public FQID Window { get; set; }

	public int Index { get; set; }
}
