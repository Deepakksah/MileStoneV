namespace VideoOS.Installer.RecorderPlugins;

public interface IPluginUtilities
{
	bool FailoverInstallation { get; }

	bool IsCustom { get; }

	bool IsTypical { get; }

	string LookupRegistryKey { get; }

	string LookupUser { get; }

	string GetValueFromRegistry(string keyName, string parameter);

	void DeleteValueFromRegistry(string keyName, string parameter);

	PluginUtilities.MilestoneServerTypes InstalledMilestoneServices();

	bool IsRecorderInstalledInHWMM();
}
