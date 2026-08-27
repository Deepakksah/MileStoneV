namespace VideoOS.Platform.SDK.Interface;

public interface IPositionInfo
{
	double Pan { get; set; }

	double Tilt { get; set; }

	double Zoom { get; set; }
}
