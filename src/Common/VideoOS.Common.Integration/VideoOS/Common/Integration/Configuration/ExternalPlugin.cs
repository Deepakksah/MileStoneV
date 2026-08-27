using System;

namespace VideoOS.Common.Integration.Configuration;

public class ExternalPlugin
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.ExternalPluginsRow _datarow;

	public ConfigurationDataSet.ExternalPluginsRow DataRow => _datarow;

	public Guid PluginID
	{
		get
		{
			return new Guid(_datarow.PluginID);
		}
		set
		{
			_datarow.PluginID = value.ToString();
		}
	}

	public string PluginType
	{
		get
		{
			return _datarow.PluginType;
		}
		set
		{
			_datarow.PluginType = value;
		}
	}

	public string Data
	{
		get
		{
			return _datarow.Data;
		}
		set
		{
			_datarow.Data = value;
		}
	}

	public ExternalPlugin(ConfigurationManager configurationManager, ConfigurationDataSet.ExternalPluginsRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
