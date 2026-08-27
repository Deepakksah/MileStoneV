using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration;

public class SettingsCollection : KeyedCollection<string, SettingEntry>
{
	public ICollection<string> Keys
	{
		get
		{
			if (base.Dictionary == null)
			{
				return null;
			}
			return base.Dictionary.Keys;
		}
	}

	protected override string GetKeyForItem(SettingEntry item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Name;
	}

	public bool TryGetValue(string name, out SettingEntry settingEntry)
	{
		if (base.Dictionary == null)
		{
			settingEntry = null;
			return false;
		}
		return base.Dictionary.TryGetValue(name, out settingEntry);
	}
}
