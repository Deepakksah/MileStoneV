using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class IPHardwareBlock : IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.HardwareBlockRow _datarow;

	public int Id
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.pkid;
		}
	}

	public Guid HardwareId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.hardware_id);
		}
	}

	public DateTime Expires
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.expires;
		}
	}

	public DataRow DataRow => _datarow;

	public IPHardwareBlock(ConfigurationManager configurationManager, ConfigurationDataSet.HardwareBlockRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
