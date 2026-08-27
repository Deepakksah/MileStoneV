using System;
using System.Data;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceGroupMember : IConfigurationItemRow
{
	private ConfigurationManager configurationManager;

	private ConfigurationDataSet.DeviceGroupMembersRow row;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return row;
		}
	}

	public Guid DeviceGroupId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(row.IDDeviceGroup);
		}
	}

	public Guid DeviceId
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(row.IDDevice);
		}
	}

	public DeviceGroupMemberKey Key{
		[DebuggerStepThrough]
		get
		{
			return new DeviceGroupMemberKey(DeviceGroupId, DeviceId);
		}
	}

	public DeviceGroupMember(ConfigurationManager configurationManager, ConfigurationDataSet.DeviceGroupMembersRow row)
	{
		this.configurationManager = configurationManager;
		this.row = row;
	}
}
