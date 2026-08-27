using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Common.Integration.Configuration;

public class SystemSettings
{
	private ConfigurationManager _manager;

	private ConfigurationDataSet.SystemSettingsDataTable _table;

	private Dictionary<string, SystemSetting> _settings;

	public SystemSetting this[SettingList settingName]
	{
		get
		{
			string name = Enum.GetName(typeof(SettingList), settingName);
			if (!TryGetSetting(name, out var setting))
			{
				throw new ArgumentOutOfRangeException("settingName", string.Format(CultureInfo.InvariantCulture, "The setting '{0}' does not exist", name));
			}
			return setting;
		}
	}

	public SystemSetting this[string name]
	{
		get
		{
			if (!TryGetSetting(name, out var setting))
			{
				throw new ArgumentOutOfRangeException("name", string.Format(CultureInfo.InvariantCulture, "The setting '{0}' does not exist", name));
			}
			return setting;
		}
	}

	public SystemSettings(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_manager = manager;
		_table = _manager.Dataset.SystemSettings;
		_settings = new Dictionary<string, SystemSetting>(StringComparer.OrdinalIgnoreCase);
		_manager.EnsureTableDataLoaded(_table);
		foreach (ConfigurationDataSet.SystemSettingsRow item in _table)
		{
			Add(item);
		}
	}

	public bool HasSetting(SettingList setting)
	{
		return HasSetting(Enum.GetName(typeof(SettingList), setting));
	}

	public bool HasSetting(string settingName)
	{
		if (settingName == null)
		{
			throw new ArgumentNullException("settingName");
		}
		settingName = settingName.Trim();
		return _settings.ContainsKey(settingName);
	}

	public bool TryGetSetting(SettingList settingName, out SystemSetting setting)
	{
		string name = Enum.GetName(typeof(SettingList), settingName);
		return TryGetSetting(name, out setting);
	}

	public bool TryGetSetting(string name, out SystemSetting setting)
	{
		return _settings.TryGetValue(name, out setting);
	}

	private SystemSetting Add(ConfigurationDataSet.SystemSettingsRow row)
	{
		SystemSetting systemSetting = new SystemSetting(_manager, row);
		_settings.Add(systemSetting.Name, systemSetting);
		return systemSetting;
	}
}
