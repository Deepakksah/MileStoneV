using System;
using System.Data;
using VideoOS.Common.SharedTimeProfile;

namespace VideoOS.Common.Integration.Configuration;

public class TimeProfile : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.TimeProfileRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.pkid);
		}
		set
		{
			_datarow.pkid = value.ToString();
		}
	}

	public string Name
	{
		get
		{
			return _datarow.name;
		}
		set
		{
			_datarow.name = value;
		}
	}

	public string Description
	{
		get
		{
			return _datarow.description;
		}
		set
		{
			_datarow.description = value;
		}
	}

	public string Definition
	{
		get
		{
			return _datarow.xml_data;
		}
		set
		{
			_datarow.xml_data = value;
		}
	}

	public TimeProfileType TimeProfileType
	{
		get
		{
			TimeProfileManager.ConvertTimeProfileType(_datarow.type, out var outValue);
			return outValue;
		}
		set
		{
			_datarow.type = value.ToString();
		}
	}

	public string DisplaySortKey => Name;

	public TimeProfile(ConfigurationManager configurationManager, ConfigurationDataSet.TimeProfileRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
