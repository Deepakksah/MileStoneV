using System;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server.DataComponents;

public interface ISystemSettingsComponent : IDisposable
{
	string QuerySystemSettingByName(SettingList setting);
}
