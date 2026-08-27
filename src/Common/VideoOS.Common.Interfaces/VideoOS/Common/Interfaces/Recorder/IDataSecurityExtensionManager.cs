using System;

namespace VideoOS.Common.Interfaces.Recorder;

public interface IDataSecurityExtensionManager
{
	void Initialize(string serverAddress, int serverPort);

	void Uninitialize();

	IDataSecurityExtension GetDataSecurityExtension(Guid deviceId);
}
