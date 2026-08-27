using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ClientProfile;

namespace VideoOS.Management.Server;

public class ClientProfileSpecificationCacheManager : IClientProfileSpecificationCacheManager
{
	public string[] EncryptedClientProfileSettings { get; private set; }

	public ClientProfileSpecificationCacheManager()
	{
		List<string> list = new List<string>();
		foreach (ClientProfileSectionSpecification section in new ClientProfileSpecification().Sections)
		{
			foreach (ClientProfileSettingSpecification setting in section.Settings)
			{
				if (setting.Control != null && (setting.Control.ID == "EncryptionPassword" || setting.Control.ID == "EncryptionKey"))
				{
					list.Add(setting.ID);
				}
			}
		}
		EncryptedClientProfileSettings = list.ToArray();
	}
}
