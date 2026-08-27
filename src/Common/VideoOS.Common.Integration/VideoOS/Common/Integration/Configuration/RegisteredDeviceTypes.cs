using System;
using System.Collections;

namespace VideoOS.Common.Integration.Configuration;

public class RegisteredDeviceTypes : DictionaryBase
{
	private ConfigurationDataSet.RegisteredDeviceTypesDataTable _table;

	private ConfigurationManager _manager;

	public RegisteredDeviceType this[string deviceType]
	{
		get
		{
			if (deviceType == null)
			{
				throw new ArgumentNullException("deviceType");
			}
			return (RegisteredDeviceType)base.Dictionary[deviceType.Trim()];
		}
	}

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public ICollection ValuesBySortOrder
	{
		get
		{
			SortedList sortedList = new SortedList();
			foreach (RegisteredDeviceType value in base.Dictionary.Values)
			{
				sortedList.Add(value.SortOrder, value);
			}
			return sortedList.Values;
		}
	}

	public RegisteredDeviceTypes(ConfigurationManager manager)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_manager = manager;
		_table = _manager.Dataset.RegisteredDeviceTypes;
		_manager.EnsureTableDataLoaded(_table);
		foreach (ConfigurationDataSet.RegisteredDeviceTypesRow row in _table.Rows)
		{
			Add(row);
		}
	}

	public RegisteredDeviceType Add(ConfigurationDataSet.RegisteredDeviceTypesRow row)
	{
		RegisteredDeviceType registeredDeviceType = new RegisteredDeviceType(_manager, row);
		base.Dictionary.Add(registeredDeviceType.DeviceType, registeredDeviceType);
		return registeredDeviceType;
	}

	public RegisteredDeviceType Add(string deviceType, string description, int sortOrder, int primaryIconID, bool recordingDevice)
	{
		RegisteredDeviceType registeredDeviceType = new RegisteredDeviceType(_manager, _table.AddRegisteredDeviceTypesRow(DateTime.UtcNow, deviceType, description, sortOrder, primaryIconID, recordingDevice));
		base.Dictionary.Add(registeredDeviceType.DeviceType, registeredDeviceType);
		return registeredDeviceType;
	}

	public bool Contains(string deviceType)
	{
		if (deviceType == null)
		{
			throw new ArgumentNullException("deviceType");
		}
		return base.Dictionary.Contains(deviceType.Trim());
	}

	public void Remove(string deviceType)
	{
		if (deviceType == null)
		{
			throw new ArgumentNullException("deviceType");
		}
		if (this[deviceType.Trim()] != null)
		{
			ConfigurationDataSet.RegisteredDeviceTypesRow dataRow = this[deviceType].DataRow;
			base.Dictionary.Remove(deviceType);
			dataRow.Delete();
		}
	}

	public new void Clear()
	{
		ArrayList arrayList = new ArrayList();
		foreach (RegisteredDeviceType value in Values)
		{
			arrayList.Add(value.DeviceType);
		}
		foreach (string item in arrayList)
		{
			Remove(item);
		}
	}

	public bool ContainsRecordingDevice(Device[] devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		bool result = false;
		foreach (Device device in devices)
		{
			if (this[device.DeviceType].IsRecordingDevice)
			{
				result = true;
			}
		}
		return result;
	}

	public Device[] GetRecordingDevicesFromArray(Device[] devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		ArrayList arrayList = new ArrayList();
		foreach (Device device in devices)
		{
			if (device.IsRecordingDevice)
			{
				arrayList.Add(device);
			}
		}
		return (Device[])arrayList.ToArray(typeof(Device));
	}

	public string[] GetRecordingDeviceTypes()
	{
		ArrayList arrayList = new ArrayList();
		foreach (RegisteredDeviceType value in Values)
		{
			if (value.IsRecordingDevice)
			{
				arrayList.Add(value.DeviceType);
			}
		}
		return (string[])arrayList.ToArray(typeof(string));
	}
}
