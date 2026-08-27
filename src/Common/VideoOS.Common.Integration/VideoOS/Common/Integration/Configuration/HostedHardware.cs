using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class HostedHardware : IConfigurationItemRow
{
	private ConfigurationManager configurationManager;

	public ConfigurationDataSet.HostedHardwareRow DataRow { get; private set; }

	DataRow IConfigurationItemRow.DataRow => DataRow;

	public int Id
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.pkid;
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.name;
		}
	}

	public int HostingServerId
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.hosting_server_id;
		}
	}

	public string HostingInfo
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.hosting_info;
		}
	}

	public string ManufacturerId
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.manufacturer_id;
		}
	}

	public string UserName
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.user_name;
		}
	}

	public string Password
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.password;
		}
	}

	public string CustomData
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.custom_data;
		}
	}

	public HostingServer HostingServer => configurationManager.HostingServers.GetHostingServerById(HostingServerId);

	public HostedHardware(ConfigurationManager manager, ConfigurationDataSet.HostedHardwareRow row)
	{
		configurationManager = manager;
		DataRow = row;
	}
}
