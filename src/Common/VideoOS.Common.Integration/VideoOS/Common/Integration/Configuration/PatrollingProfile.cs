using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class PatrollingProfile : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.PatrollingProfilesRow _dataRow;

	public DataRow DataRow => _dataRow;

	public Guid ID
	{
		get
		{
			return new Guid(_dataRow.IDPatrollingProfile);
		}
		set
		{
			_dataRow.IDPatrollingProfile = value.ToString();
		}
	}

	public Guid IDDevice
	{
		get
		{
			return new Guid(_dataRow.IDDevice);
		}
		set
		{
			_dataRow.IDDevice = value.ToString();
		}
	}

	public string Name
	{
		get
		{
			return _dataRow.Name;
		}
		set
		{
			_dataRow.Name = value;
		}
	}

	public string Description
	{
		get
		{
			return _dataRow.Description;
		}
		set
		{
			_dataRow.Description = value;
		}
	}

	public string DefinitionAsXml
	{
		get
		{
			return _dataRow.Definition;
		}
		set
		{
			_dataRow.Definition = value;
		}
	}

	public string DisplaySortKey => Name;

	public PatrollingProfile(ConfigurationManager configurationManager, ConfigurationDataSet.PatrollingProfilesRow row)
	{
		_configurationManager = configurationManager;
		_dataRow = row;
	}
}
