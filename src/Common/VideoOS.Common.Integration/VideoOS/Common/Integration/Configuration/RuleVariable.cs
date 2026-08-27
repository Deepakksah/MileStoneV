using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class RuleVariable : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RuleVariablesRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		get
		{
			return new Guid(_datarow.IDVariable);
		}
		set
		{
			_datarow.IDVariable = value.ToString();
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

	public string Value
	{
		get
		{
			return _datarow.Value;
		}
		set
		{
			_datarow.Value = value;
		}
	}

	public bool Reset
	{
		get
		{
			return _datarow.Reset;
		}
		set
		{
			_datarow.Reset = value;
		}
	}

	public string DisplaySortKey => Name;

	public RuleVariable(ConfigurationManager configurationManager, ConfigurationDataSet.RuleVariablesRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
