using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class IPHardwareDefinition : IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.HardwareDefinitionRow _datarow;

	DataRow IConfigurationItemRow.DataRow => DataRow;

	public ConfigurationDataSet.HardwareDefinitionRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public string C18NDigest
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.C18NDigest;
		}
	}

	public string Definition
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Definition;
		}
	}

	public string DisplaySortKey
	{
		[DebuggerStepThrough]
		get
		{
			return C18NDigest;
		}
	}

	public IPHardwareDefinition(ConfigurationManager manager, ConfigurationDataSet.HardwareDefinitionRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}
}
