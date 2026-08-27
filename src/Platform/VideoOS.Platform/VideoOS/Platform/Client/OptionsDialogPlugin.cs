using System;
using System.Xml;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use SettingsPanelPlugin instead.", false)]
public abstract class OptionsDialogPlugin : PropertyClass
{
	public abstract Guid Id { get; }

	public virtual bool OptionsEnabled => true;

	public abstract string Name { get; }

	protected OptionsDialogPlugin()
	{
		new TrackPageViewData(typeof(OptionsDialogPlugin), deprecated: true).Send();
	}

	public abstract OptionsDialogUserControl GenerateUserControl();

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

	public abstract bool SaveChanges();

	public bool SaveProperties(bool userPrivate)
	{
		Configuration.Instance.SaveOptionsConfiguration(Id, userPrivate, SerializeToXmlNode());
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
}
