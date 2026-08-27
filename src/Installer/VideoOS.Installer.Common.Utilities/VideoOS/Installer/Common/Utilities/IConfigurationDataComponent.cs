using System;

namespace VideoOS.Installer.Common.Utilities;

public interface IConfigurationDataComponent : IDisposable
{
	int QueryRecordingServerCountPre2020R1();

	(bool HasAnyDuplicateDevices, bool HasImportantDuplicateDevices, bool HasDuplicateRecordingDevices) QueryDuplicateDevicesInfo();
}
