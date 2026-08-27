using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public struct PTZMoveStartCommandData
{
	public double Pan;

	public double Tilt;

	public double Zoom;

	public double Speed;
}
