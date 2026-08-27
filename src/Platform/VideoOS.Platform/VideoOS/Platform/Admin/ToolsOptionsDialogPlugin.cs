using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Platform.Admin;

public abstract class ToolsOptionsDialogPlugin
{
	public abstract Guid Id { get; }

	public virtual bool OptionsEnabled => true;

	public abstract string Name { get; }

	public abstract ToolsOptionsDialogUserControl GenerateUserControl();

	public abstract void Init();

	public abstract void Close();

	public XmlNode LoadSettingsXml(bool userPrivate)
	{
		return Configuration.Instance.GetOptionsConfiguration(Id, userPrivate);
	}

	public abstract bool SaveChanges();

	public bool SaveSettingsXml(XmlNode settings, bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, settings);
		return true;
	}

	public virtual string GetLastSaveError()
	{
		return "";
	}

	public virtual bool ShowHelp()
	{
		return false;
	}

	public virtual Dictionary<Guid, string> GetConfigurationReportSections()
	{
		return null;
	}

	public virtual Dictionary<string, string> GetConfigurationReportProperties(Guid sectionId)
	{
		return null;
	}
}
