using System;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public interface IManagementServerComponent : IDisposable
{
	ServerInformation GetServerInformation(string userSid, out bool privateViewGroupCreated);

	string GetSystemSetting(SettingList setting);

	string GetSystemSetting(string setting);

	bool SetSystemSetting(string setting, string value);
}
