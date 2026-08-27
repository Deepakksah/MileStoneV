using System.Diagnostics;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class SettingDefinition
{
	private MainDefinition mainDefinition;

	private settingsType settings;

	private settingType setting;

	public settingType Base
	{
		[DebuggerStepThrough]
		get
		{
			return setting;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return setting.name;
		}
	}

	public Hwid Hwid => Hwid.ParseHwid(settings.hwid);

	public string Value
	{
		[DebuggerStepThrough]
		get
		{
			return setting.value;
		}
		[DebuggerStepThrough]
		set
		{
			setting.value = value;
		}
	}

	public SettingDefinition(MainDefinition mainDefinition, settingsType settings, settingType setting)
	{
		this.mainDefinition = mainDefinition;
		this.settings = settings;
		this.setting = setting;
	}
}
