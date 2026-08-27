namespace VideoOS.Common.Integration.Drivers;

public interface IPtz
{
	bool IsPtzEnabled { get; }

	void MoveJoystick(string command);

	void MoveRelative(PtzArgs ptzArguments);

	void Move(PtzArgs ptzArguments);

	void MoveHome();

	void MoveStop();

	void MoveAbsolute(PtzArgs ptzArguments);

	void CenterOnPositionInView(double positionX, double positionY, double imageWidth, double imageHeight, double actualWidth, double actualHeight, double zoom);

	void CenterAndZoomToRectangle(double positionLeft, double positionTop, double positionRight, double positionBottom, double imageWidth, double imageHeight, double actualWidth, double actualHeight);

	PtzArgs GetAbsolutePosition();
}
