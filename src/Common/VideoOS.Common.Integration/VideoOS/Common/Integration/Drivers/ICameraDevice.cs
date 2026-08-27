namespace VideoOS.Common.Integration.Drivers;

public interface ICameraDevice : IMediaStreamDevice
{
	void CustomCommand(string command);

	string CustomCommandWithResponse(string command, string parameter);
}
