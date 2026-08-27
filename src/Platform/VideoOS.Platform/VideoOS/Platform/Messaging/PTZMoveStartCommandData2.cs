using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public struct PTZMoveStartCommandData2
{
	public double Pan;

	public double Tilt;

	public double Zoom;

	public double PanSpeed;

	public double TiltSpeed;

	public double ZoomSpeed;
}
