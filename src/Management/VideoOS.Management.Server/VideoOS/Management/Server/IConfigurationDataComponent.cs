using System;

namespace VideoOS.Management.Server;

public interface IConfigurationDataComponent : IDisposable
{
	IdNameDescription FindHomeSiteInfo();

	AudioMessageIdNameDescription[] FindAudioMessages();

	bool FindPrivacyMaskUsed();

	string GetClientSettingsXml();
}
