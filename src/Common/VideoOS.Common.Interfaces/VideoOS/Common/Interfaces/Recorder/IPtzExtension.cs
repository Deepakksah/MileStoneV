namespace VideoOS.Common.Interfaces.Recorder;

public interface IPtzExtension
{
	void Initialize(string serverAddress, int serverPort);

	void Uninitialize();

	void SetPtzExtensionCallback(IPtzExtensionCallback ptzExtensionCallback);
}
