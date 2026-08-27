using System;

namespace VideoOS.Management.Server;

public interface ICustomSettingComponent : IDisposable
{
	CustomSetting GetCustomSetting(string userSid, Guid customSettingId);

	void UpdateCustomSetting(string userSid, Guid customSettingId, int dataVersion, string settingDataXml, string modifiedUser, bool forceOverride);
}
