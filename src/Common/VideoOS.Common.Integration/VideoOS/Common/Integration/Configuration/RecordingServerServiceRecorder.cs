using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public sealed class RecordingServerServiceRecorder : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RecordingServerServiceRecorderRow _datarow;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
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

	public Guid ServiceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.recording_server_service_id);
		}
	}

	public RecordingServerServiceType ServiceType
	{
		[DebuggerStepThrough]
		get
		{
			return (RecordingServerServiceType)_datarow.type;
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.last_modified;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.last_modified = value;
		}
	}

	public bool Active
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.active;
		}
	}

	Guid IConfigurationItem.ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.recording_server_service_id);
		}
	}

	public RecordingServerServiceRecorder(ConfigurationManager configurationManager, ConfigurationDataSet.RecordingServerServiceRecorderRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
