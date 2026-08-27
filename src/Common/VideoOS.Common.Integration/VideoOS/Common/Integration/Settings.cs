namespace VideoOS.Common.Integration;

public class Settings : SectionSettings
{
	public Settings()
	{
	}

	public Settings(string xml)
	{
		LoadXml(xml, SettingsLoadMethod.Reload);
	}

	public void Set(string name, string value)
	{
		Set("", new SettingEntry("", name, value), allowCreate: true);
	}

	public string Get(string name)
	{
		return Get("", name);
	}

	public string[] GetNames()
	{
		return GetSectionSettingNames("");
	}

	public override string SaveAsXml()
	{
		return base.SaveSectionAsXml("");
	}
}
