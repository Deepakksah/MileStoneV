using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceRecorderPlugin : IConfigurationItemRow
{
	private ConfigurationManager configurationManager;

	private ConfigurationDataSet.RecorderPluginDeviceRow datarow;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return datarow;
		}
	}

	public int ProcessOrder
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.ProcessOrder;
		}
		[DebuggerStepThrough]
		set
		{
			datarow.ProcessOrder = value;
		}
	}

	public Guid IDDevice
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(datarow.IDDevice);
		}
		[DebuggerStepThrough]
		set
		{
			datarow.IDDevice = value.ToString();
		}
	}

	public Guid IDRecorderPlugin
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(datarow.IDRecorderPlugin);
		}
		[DebuggerStepThrough]
		set
		{
			datarow.IDRecorderPlugin = value.ToString();
		}
	}

	public bool Enabled
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.Enabled;
		}
		[DebuggerStepThrough]
		set
		{
			datarow.Enabled = value;
		}
	}

	public string Settings
	{
		[DebuggerStepThrough]
		get
		{
			return datarow.Settings;
		}
		[DebuggerStepThrough]
		set
		{
			datarow.Settings = value;
		}
	}

	public RecorderPlugin RecorderPlugin => configurationManager.RecorderPlugins[new Guid(datarow.IDRecorderPlugin)];

	public DeviceRecorderPlugin(ConfigurationManager configurationManager, ConfigurationDataSet.RecorderPluginDeviceRow row)
	{
		this.configurationManager = configurationManager;
		datarow = row;
	}
}
