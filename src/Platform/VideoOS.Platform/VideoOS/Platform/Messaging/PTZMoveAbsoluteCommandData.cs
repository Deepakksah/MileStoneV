namespace VideoOS.Platform.Messaging;

public struct PTZMoveAbsoluteCommandData
{
	public double Pan;

	public double Tilt;

	public double Zoom;

	public double Speed;

	public bool AllowRepeats;
}
