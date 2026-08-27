using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceLink : IConfigurationItemString, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.DeviceLinksRow _datarow;

	public string ID { get; private set; }

	public Guid IDDevice { get; private set; }

	public Guid IDDeviceLink { get; private set; }

	public DataRow DataRow => _datarow;

	public DeviceLink(ConfigurationManager manager, ConfigurationDataSet.DeviceLinksRow row)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		if (row == null)
		{
			throw new ArgumentNullException("row");
		}
		_configurationManager = manager;
		_datarow = row;
		ID = Key(_datarow.IDDevice, _datarow.IDDeviceLinked);
		IDDevice = new Guid(_datarow.IDDevice);
		IDDeviceLink = new Guid(_datarow.IDDeviceLinked);
	}

	public static string Key(Guid deviceId, Guid deviceLinkId)
	{
		return Key(deviceId.ToString(), deviceLinkId.ToString());
	}

	public static string Key(string deviceId, string deviceLinkId)
	{
		if (deviceId == null)
		{
			throw new ArgumentNullException("deviceId");
		}
		if (deviceLinkId == null)
		{
			throw new ArgumentNullException("deviceLinkId");
		}
		return deviceId + deviceLinkId;
	}
}
