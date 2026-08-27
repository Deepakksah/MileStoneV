using System.Collections.Generic;
using System.Windows.Media;

namespace VideoOS.Platform.SDK.Platform;

public interface ISDKEnvironmentManager
{
	bool ApploggedState { set; }

	bool EnableConfigurationChangedService { get; set; }

	void SDKEnvironmentManagerInitialize();

	void Logout(ServerId serverId);

	void LogoutAll();

	void Log(bool error, string where, string text);

	void InitializeUIFramework(IDictionary<string, Color> brandColors);

	void TelemetryFlush();
}
