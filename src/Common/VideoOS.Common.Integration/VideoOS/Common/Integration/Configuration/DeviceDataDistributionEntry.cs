using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceDataDistributionEntry : IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.DeviceDataDistributionRow _datarow;

	public Guid DeviceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.device_id);
		}
	}

	public Guid RecorderId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.recorder_id);
		}
	}

	public Guid TrackId
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.track_id;
		}
	}

	public DataRow DataRow => _datarow;

	public DeviceDataDistributionEntry(ConfigurationManager configurationManager, ConfigurationDataSet.DeviceDataDistributionRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
