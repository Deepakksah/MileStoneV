using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class HostingServer : IConfigurationItemRow
{
	private ConfigurationManager configurationManager;

	public ConfigurationDataSet.HostingServerRow DataRow { get; private set; }

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

	public string InternalAddress
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.internal_address;
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

	public string HostingType
	{
		[DebuggerStepThrough]
		get
		{
			return DataRow.type;
		}
	}

	public HostingServer(ConfigurationManager manager, ConfigurationDataSet.HostingServerRow row)
	{
		configurationManager = manager;
		DataRow = row;
	}
}
