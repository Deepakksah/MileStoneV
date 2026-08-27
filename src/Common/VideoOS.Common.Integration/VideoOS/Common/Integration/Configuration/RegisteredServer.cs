using System;
using System.Data;
using System.Diagnostics;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

public class RegisteredServer : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RegisteredServersRow _datarow;

	public DataRow DataRow => _datarow;

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.IDServer);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDServer = value.ToString();
		}
	}

	public Guid IDServer
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.IDServer);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDServer = value.ToString();
		}
	}

	public Guid Type
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.Type);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Type = value.ToString();
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Name;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Name = value;
		}
	}

	public string Description
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Description;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Description = value;
		}
	}

	public ServerAddress[] Addresses
	{
		[DebuggerStepThrough]
		get
		{
			return ServerAddresses.GetFromXml(_datarow.Addresses).ToArray();
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Addresses = ServerAddresses.GetAsXml(value);
		}
	}

	public string EndpointDefinitions
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.EndpointDefinitions;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Addresses = value;
		}
	}

	public bool Trusted
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Trusted;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Trusted = value;
		}
	}

	public string CustomData
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.CustomData;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.CustomData = value;
		}
	}

	public bool Enabled
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Enabled;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Enabled = value;
		}
	}

	public string DisplaySortKey => Name;

	public RegisteredServer(ConfigurationDataSet.RegisteredServersRow row)
	{
		_configurationManager = null;
		_datarow = row;
	}

	public RegisteredServer(ConfigurationManager manager, ConfigurationDataSet.RegisteredServersRow row)
	{
		_configurationManager = manager;
		_datarow = row;
	}
}
