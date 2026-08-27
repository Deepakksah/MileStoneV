using System.Xml;

namespace VideoOS.Common.Interfaces;

public interface IFileManager
{
	XmlDocument LoadApplicationSettings();

	bool SaveApplicationSettings(XmlDocument applicationSettingsXmlDocument);

	XmlDocument LoadUserSettings30();

	bool SaveUserSettings30(XmlDocument userSettingsXmlDocument);

	XmlDocument LoadMigrateLegacyMapSettings();

	bool SaveMigrateLegacyMapSettings(XmlDocument addLegacyMapSettingsXmlDocument);

	XmlDocument LoadPositioning(string formName, string settingsFolder);

	bool SavePositioning(string formName, string settingsFolder, XmlDocument settingsXmlDocument);

	void ClearAllPositionings(string settingsFolder);

	void ResetPositioning(string formName, string settingsFolder);
}
