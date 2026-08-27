namespace VideoOS.Common.Drivers;

[DeviceInterface("0FB9A163-9DCF-4261-8CFC-716A1912F377")]
public interface IPTZ
{
	bool IsPTZEnabled { get; }

	void MoveJoystick(string command);

	void MoveRelative(PTZArgs ptzArguments);

	void Move(PTZArgs ptzArguments);

	void MoveHome();

	void MoveStop();

	void MoveAbsolute(PTZArgs ptzArguments);

	void CenterOnPositionInView(double positionX, double positionY, double imageWidth, double imageHeight, double zoom);

	void CenterAndZoomToRectangle(double positionLeft, double positionTop, double positionRight, double positionBottom, double imageWidth, double imageHeight);

	PTZArgs GetAbsolutePosition();
}
