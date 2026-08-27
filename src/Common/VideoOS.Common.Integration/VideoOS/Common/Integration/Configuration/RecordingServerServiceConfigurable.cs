using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public sealed class RecordingServerServiceConfigurable : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RecordingServerServiceConfigurableRow _datarow;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.LastModified;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.LastModified = value;
		}
	}

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.PKID);
		}
	}

	public string MulticastServerAddress
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.MulticastServerAddress;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.MulticastServerAddress = value;
		}
	}

	public bool ShutdownOnStorageFailure
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.ShutdownOnStorageFailure;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.ShutdownOnStorageFailure = value;
		}
	}

	public bool PublicAccessEnabled
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.PublicAccessEnabled;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.PublicAccessEnabled = value;
		}
	}

	public string PublicWebserverHostName
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.PublicWebServerHostname;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.PublicWebServerHostname = value;
		}
	}

	public int PublicWebserverPort
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.PublicWebServerPort;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.PublicWebServerPort = value;
		}
	}

	public RecordingServerServiceConfigurable(ConfigurationManager configurationManager, ConfigurationDataSet.RecordingServerServiceConfigurableRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}
}
