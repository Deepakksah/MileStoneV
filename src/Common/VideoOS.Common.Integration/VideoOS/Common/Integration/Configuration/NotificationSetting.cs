using System;
using System.ComponentModel;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class NotificationSetting : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.NotificationSettingsRow _datarow;

	private Settings _settings;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.IDNotificationSetting);
		}
		set
		{
			_datarow.IDNotificationSetting = value.ToString();
		}
	}

	public string SettingsAsXml
	{
		get
		{
			return Settings.SaveAsXml();
		}
		set
		{
			_datarow.Settings = value;
			if (_settings != null)
			{
				_settings.SettingsChanged -= SettingsChanged;
				_settings.LoadXml(value, SettingsLoadMethod.Update);
				_settings.SettingsChanged += SettingsChanged;
			}
		}
	}

	[Browsable(false)]
	public Guid IDNotificationSetting
	{
		get
		{
			return new Guid(_datarow.IDNotificationSetting);
		}
		set
		{
			_datarow.IDNotificationSetting = value.ToString();
		}
	}

	public Settings Settings
	{
		get
		{
			if (_settings == null)
			{
				_settings = new Settings();
				if (!_datarow.IsSettingsNull())
				{
					_settings.LoadXml(_datarow.Settings, SettingsLoadMethod.Reload);
				}
				_settings.SettingsChanged += SettingsChanged;
			}
			else if (!_datarow.IsSettingsNull())
			{
				_settings.SettingsChanged -= SettingsChanged;
				_settings.LoadXml(_datarow.Settings, SettingsLoadMethod.Update);
				_settings.SettingsChanged += SettingsChanged;
			}
			return _settings;
		}
	}

	[Browsable(false)]
	public Guid IDNotificationType
	{
		get
		{
			return new Guid(_datarow.IDNotificationType);
		}
		set
		{
			_datarow.IDNotificationType = value.ToString();
		}
	}

	[Browsable(false)]
	public Guid IDNotificationProfile
	{
		get
		{
			return new Guid(_datarow.IDNotificationProfile);
		}
		set
		{
			_datarow.IDNotificationProfile = value.ToString();
		}
	}

	[Browsable(false)]
	public string DisplaySortKey => Settings.Get("Subject").ToString();

	private void SettingsChanged(object sender, SettingsEventArgs eventarg)
	{
		_datarow.Settings = _settings.SaveAsXml();
	}

	public NotificationSetting(ConfigurationDataSet.NotificationSettingsRow row)
	{
		_configurationManager = null;
		_datarow = row;
	}

	public NotificationSetting(ConfigurationManager manager, ConfigurationDataSet.NotificationSettingsRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}
}
