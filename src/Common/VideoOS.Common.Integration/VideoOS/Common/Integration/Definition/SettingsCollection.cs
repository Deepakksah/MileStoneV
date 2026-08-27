using System;
using System.Collections.ObjectModel;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration.Definition;

public class SettingsCollection : KeyedCollection<string, SettingDefinition>
{
	private MainDefinition mainDefinition;

	private settingsType settings;

	public SettingsCollection(MainDefinition mainDefinition, settingsType settings)
	{
		this.mainDefinition = mainDefinition;
		this.settings = settings;
		if (this.settings != null && this.settings.setting != null)
		{
			settingType[] setting = this.settings.setting;
			foreach (settingType setting2 in setting)
			{
				Add(new SettingDefinition(this.mainDefinition, this.settings, setting2));
			}
		}
	}

	protected override string GetKeyForItem(SettingDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Name;
	}

	public bool TryGetValue(string name, out SettingDefinition setting)
	{
		if (base.Dictionary == null)
		{
			setting = null;
			return false;
		}
		return base.Dictionary.TryGetValue(name, out setting);
	}
}
