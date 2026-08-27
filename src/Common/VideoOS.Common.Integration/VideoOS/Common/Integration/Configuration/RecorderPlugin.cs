using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class RecorderPlugin : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RecorderPluginsRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.IDRecorderPlugin);
		}
		set
		{
			_datarow.IDRecorderPlugin = value.ToString();
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

	public string DisplayName
	{
		get
		{
			string[] array = _datarow.Name.Split('.');
			return array[array.Length - 1];
		}
	}

	public string Description
	{
		get
		{
			return _datarow.Description;
		}
		set
		{
			_datarow.Description = value;
		}
	}

	public string DisplaySortKey => Name;

	public RecorderPlugin(ConfigurationManager configurationManager, ConfigurationDataSet.RecorderPluginsRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}

	public override string ToString()
	{
		return _datarow.Name;
	}
}
