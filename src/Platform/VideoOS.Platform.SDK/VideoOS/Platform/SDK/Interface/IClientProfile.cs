using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IClientProfile
{
	Guid Id { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	DateTime LastModified { get; set; }

	DateTime SettingsLastModified { get; set; }

	IClientProfileSetting[] ClientProfileSettings { get; set; }
}
