using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class Rule : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RulesRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.IDRule);
		}
		set
		{
			_datarow.IDRule = value.ToString();
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

	public string DefinitionAsXml
	{
		get
		{
			return _datarow.Definition;
		}
		set
		{
			_datarow.Definition = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return _datarow.Enabled;
		}
		set
		{
			_datarow.Enabled = value;
		}
	}

	public int Priority
	{
		get
		{
			return _datarow.Priority;
		}
		set
		{
			_datarow.Priority = value;
		}
	}

	public string DisplaySortKey => Name;

	public Rule(ConfigurationManager configurationManager, ConfigurationDataSet.RulesRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
