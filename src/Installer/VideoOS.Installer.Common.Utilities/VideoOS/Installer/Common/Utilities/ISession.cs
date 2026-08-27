using Microsoft.Deployment.WindowsInstaller;

namespace VideoOS.Installer.Common.Utilities;

public interface ISession
{
	string this[string property] { get; set; }

	void SetMode(InstallRunMode mode, bool value);

	bool GetMode(InstallRunMode mode);

	void SetPersistedProperty(string name, string value);

	string GetPersistedProperty(string name);

	void RemovePersistedProperty(string name);

	void Log(string msg);

	void Log(string format, params object[] args);
}
