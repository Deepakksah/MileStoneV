using System;
using System.Data;
using System.Diagnostics;
using VideoOS.Common.Integration.Properties;
using VideoOS.Utilities;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceGroup : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager configurationManager;

	private ConfigurationDataSet.DeviceGroupsRow row;

	private DeviceGroup parent;

	private DeviceGroupMembers members;

	private DeviceGroups deviceGroups;

	public DataRow DataRow => row;

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(row.IDDeviceGroup);
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			string text = row.Name;
			if (BuiltIn)
			{
				text = BuiltInGroupResources.ResourceManager.GetString(text);
			}
			return text;
		}
		[DebuggerStepThrough]
		set
		{
			row.Name = Character.RemoveInvalidXmlChars(value);
		}
	}

	public string Description
	{
		[DebuggerStepThrough]
		get
		{
			string text = row.Description;
			if (BuiltIn)
			{
				text = BuiltInGroupResources.ResourceManager.GetString(text);
			}
			return text;
		}
		[DebuggerStepThrough]
		set
		{
			row.Description = Character.RemoveInvalidXmlChars(value);
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			return row.LastModified;
		}
		[DebuggerStepThrough]
		set
		{
			row.LastModified = value;
		}
	}

	public string DeviceType
	{
		[DebuggerStepThrough]
		get
		{
			return row.DeviceType.Trim();
		}
	}

	public bool BuiltIn
	{
		[DebuggerStepThrough]
		get
		{
			return row.BuiltIn;
		}
	}

	public bool IsUserControlled
	{
		[DebuggerStepThrough]
		get
		{
			return row.IsUserControlled;
		}
	}

	public DeviceGroup Parent
	{
		get
		{
			if (row.IsIDParentDeviceGroupNull())
			{
				return null;
			}
			if (parent == null)
			{
				parent = configurationManager.DeviceGroupItems.GetDeviceGroupByDeviceGroupId(new Guid(row.IDParentDeviceGroup));
			}
			return parent;
		}
	}

	public DeviceGroupMembers Devices
	{
		get
		{
			if (members == null)
			{
				members = new DeviceGroupMembers(configurationManager, this);
			}
			return members;
		}
	}

	public bool IsDevicesLoaded => members != null;

	public DeviceGroups DeviceGroups
	{
		get
		{
			if (deviceGroups == null)
			{
				deviceGroups = new DeviceGroups(configurationManager, this);
			}
			return deviceGroups;
		}
	}

	public string DisplaySortKey
	{
		[DebuggerStepThrough]
		get
		{
			return Name;
		}
	}

	public DeviceGroup(ConfigurationManager configurationManager, ConfigurationDataSet.DeviceGroupsRow row)
	{
		this.configurationManager = configurationManager;
		this.row = row;
	}

	public override string ToString()
	{
		return row.Name;
	}
}
