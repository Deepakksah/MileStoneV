using System;
using System.Windows.Controls;
using System.Xml;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class SettingsPanelPlugin : PropertyClass
{
	public abstract Guid Id { get; }

	public abstract string Title { get; }

	protected SettingsPanelPlugin()
	{
		new TrackPageViewData(typeof(SettingsPanelPlugin)).Send();
	}

	public abstract UserControl GenerateUserControl();

	public abstract void CloseUserControl();

	public abstract void Init();

	public abstract void Close();

	public bool LoadProperties(bool userPrivate)
	{
		XmlNode optionsConfiguration = Configuration.Instance.GetOptionsConfiguration(Id, userPrivate);
		if (optionsConfiguration != null)
		{
			Deserialize(optionsConfiguration);
			return true;
		}
		return false;
	}

	public abstract bool TrySaveChanges(out string errorMessage);

	public void SaveProperties(bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, SerializeToXmlNode());
	}

	public virtual bool ShowHelp()
	{
		return false;
	}
}
