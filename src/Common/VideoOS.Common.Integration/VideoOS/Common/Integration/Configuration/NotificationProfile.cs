using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class NotificationProfile : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.NotificationProfilesRow _datarow;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.IDNotificationProfile);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDNotificationProfile = value.ToString();
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Name;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Name = value;
		}
	}

	public string Description
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Description;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Description = value;
		}
	}

	public string DisplaySortKey
	{
		[DebuggerStepThrough]
		get
		{
			return Name;
		}
	}

	public NotificationProfile(ConfigurationManager manager, ConfigurationDataSet.NotificationProfilesRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}
}
