using System;

namespace VideoOS.Common.Integration.Configuration;

public class NotificationType
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.NotificationTypesRow _datarow;

	public ConfigurationDataSet.NotificationTypesRow DataRow => _datarow;

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

	public string Name
	{
		get
		{
			return _datarow.Name;
		}
		set
		{
			_datarow.Name = value;
		}
	}

	public string PageName
	{
		get
		{
			return _datarow.PageName;
		}
		set
		{
			_datarow.PageName = value;
		}
	}

	public string DisplaySortKey => Name;

	public NotificationType(ConfigurationManager manager, ConfigurationDataSet.NotificationTypesRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}

	public override string ToString()
	{
		return Name;
	}
}
