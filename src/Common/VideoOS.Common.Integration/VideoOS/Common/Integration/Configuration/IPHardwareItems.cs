using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Configuration;

public class IPHardwareItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<Guid, IPHardware> dictionary;

	public IPHardwareItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<Guid, IPHardware>(configurationManager, this);
		foreach (ConfigurationDataSet.HardwareRow row in this.configurationManager.Dataset.Hardware.Rows)
		{
			AddItem(row);
		}
	}

	public IPHardware AddNew(Guid id, string name, string description, bool enabled, Uri uri, string loginId, string password, Recorder recorder, Guid IDHardwareDriver, string settings, string eventSettings, string productId, string C14NDigest, int driverNumber, int? hostedHardwareId, string cameraData, string edgeSettings, Subtypes subtype)
	{
		ConfigurationDataSet.HardwareRow hardwareRow;
		if (hostedHardwareId.HasValue)
		{
			hardwareRow = configurationManager.Dataset.Hardware.AddHardwareRow(DateTime.UtcNow, id.ToString(), IDHardwareDriver.ToString(), name, description, uri.ToString(), loginId, password, recorder.ID.ToString(), enabled, settings, eventSettings, productId, C14NDigest, driverNumber, Convert.ToString(driverNumber, CultureInfo.InvariantCulture), hostedHardwareId.Value, cameraData, edgeSettings, (int)subtype, DateTime.UtcNow, DateTime.UtcNow);
		}
		else
		{
			hardwareRow = configurationManager.Dataset.Hardware.NewHardwareRow();
			object[] itemArray = new object[22]
			{
				DateTime.UtcNow,
				id.ToString(),
				IDHardwareDriver.ToString(),
				name,
				description,
				uri.ToString(),
				loginId,
				password,
				recorder.ID.ToString(),
				enabled,
				settings,
				eventSettings,
				productId,
				C14NDigest,
				driverNumber,
				Convert.ToString(driverNumber, CultureInfo.InvariantCulture),
				DBNull.Value,
				cameraData,
				edgeSettings,
				(int)subtype,
				DateTime.UtcNow,
				DateTime.UtcNow
			};
			hardwareRow.ItemArray = itemArray;
			hardwareRow.SetCredentialsLastModifiedNull();
			configurationManager.Dataset.Hardware.Rows.Add(hardwareRow);
		}
		return AddItem(hardwareRow);
	}

	public IPHardware GetHardwareByHardwareId(Guid id)
	{
		if (dictionary.TryGetValue(id, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(IPHardware), id);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHardwareByHardwareId(id);
		if (configurationDataSet.Hardware.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(IPHardware), id);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.Hardware);
		ConfigurationDataSet.HardwareRow hardwareRow = (ConfigurationDataSet.HardwareRow)configurationDataSet.Hardware.Rows[0];
		configurationManager.Dataset.Hardware.ImportRow(hardwareRow);
		ConfigurationDataSet.HardwareRow row = configurationManager.Dataset.Hardware.FindByIDHardware(hardwareRow.IDHardware);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.Hardware);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	public IPHardware GetHardwareByHostedHardwareId(int hostedHardwareId)
	{
		IPHardware iPHardware = null;
		foreach (IPHardware value in dictionary.Values)
		{
			if (value.HasHostedHardware && value.HostedHardwareId == hostedHardwareId)
			{
				return value;
			}
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(IPHardware), hostedHardwareId);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHardwareByHostedHardwareId(hostedHardwareId);
		if (configurationDataSet.Hardware.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(IPHardware), hostedHardwareId);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.Hardware);
		ConfigurationDataSet.HardwareRow hardwareRow = (ConfigurationDataSet.HardwareRow)configurationDataSet.Hardware.Rows[0];
		configurationManager.Dataset.Hardware.ImportRow(hardwareRow);
		ConfigurationDataSet.HardwareRow row = configurationManager.Dataset.Hardware.FindByIDHardware(hardwareRow.IDHardware);
		iPHardware = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.Hardware);
		configurationManager.DisableEventHandling = false;
		return iPHardware;
	}

	public void EnsureDefinitionLoaded(Guid[] hardwareIds)
	{
		EnsureHardwareItemsLoaded(hardwareIds);
	}

	public void EnsureHardwareItemsLoaded(Guid[] hardwareIds)
	{
		List<Guid> list = new List<Guid>();
		foreach (Guid guid in hardwareIds)
		{
			if (!dictionary.TryGetValue(guid, out var _))
			{
				list.Add(guid);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.Create(typeof(IPHardware));
		}
		int num = 400;
		for (int j = 0; j < list.Count; j += num)
		{
			int num2 = list.Count - j;
			int num3 = ((num2 > num) ? num : num2);
			Guid[] array = new Guid[num3];
			list.CopyTo(j, array, 0, num3);
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHardwareByHardwareIds(array);
			if (configurationDataSet.Hardware.Rows.Count != num3)
			{
				throw ConfigurationItemNotFoundException.Create(typeof(IPHardware));
			}
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.Hardware);
			foreach (ConfigurationDataSet.HardwareRow row in configurationDataSet.Hardware.Rows)
			{
				configurationManager.Dataset.Hardware.ImportRow(row);
				AddItem(configurationManager.Dataset.Hardware.FindByIDHardware(row.IDHardware));
			}
			configurationManager.EndLoadData(configurationManager.Dataset.Hardware);
			configurationManager.DisableEventHandling = false;
		}
	}

	public void Remove(Guid id)
	{
		IPHardware hardwareByHardwareId = GetHardwareByHardwareId(id);
		if (hardwareByHardwareId == null)
		{
			return;
		}
		configurationManager.HardwareBlocks.RemoveByHardware(hardwareByHardwareId);
		hardwareByHardwareId.Devices.Clear();
		if (hardwareByHardwareId.IsIcpOrMipDriver && (configurationManager.UseDynamicLoad || hardwareByHardwareId.IsDefinitionLoaded))
		{
			IEnumerable<EventType> source = configurationManager.EventTypes.Values.Cast<EventType>();
			List<Guid> list = new List<Guid>();
			foreach (EventDefinition ev in hardwareByHardwareId.Definition.Events)
			{
				EventType eventType2 = source.FirstOrDefault((EventType et) => et.GeneratorId.Equals(ev.Base.referenceid, StringComparison.OrdinalIgnoreCase));
				if (eventType2 != null)
				{
					list.Add(eventType2.ID);
				}
			}
			foreach (Guid item in list)
			{
				configurationManager.EventTypes.Remove(item);
			}
		}
		dictionary.Remove(id);
		hardwareByHardwareId.DataRow.Delete();
	}

	public IPHardware[] GetHardwareByRecorderId(Guid recorderId)
	{
		if (configurationManager.UseDynamicLoad)
		{
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHardwareByRecorderId(recorderId);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.Hardware);
			foreach (ConfigurationDataSet.HardwareRow row in configurationDataSet.Hardware.Rows)
			{
				Guid key = new Guid(row.IDHardware);
				if (!dictionary.TryGetValue(key, out var value))
				{
					configurationManager.Dataset.Hardware.ImportRow(row);
					value = AddItem(configurationManager.Dataset.Hardware.FindByIDHardware(row.IDHardware));
				}
			}
			configurationManager.EndLoadData(configurationManager.Dataset.Hardware);
			configurationManager.DisableEventHandling = false;
		}
		return dictionary.Values.Where((IPHardware h) => h.IDRecorder == recorderId).ToArray();
	}

	private IPHardware AddItem(ConfigurationDataSet.HardwareRow row)
	{
		IPHardware iPHardware = new IPHardware(configurationManager, row);
		dictionary.Add(iPHardware.ID, iPHardware);
		return iPHardware;
	}

	public void LoadHardwareByDeviceGroupId(Guid deviceGroupId)
	{
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHardwareByDeviceGroupId(deviceGroupId);
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.Hardware);
		foreach (ConfigurationDataSet.HardwareRow row in configurationDataSet.Hardware.Rows)
		{
			Guid key = new Guid(row.IDHardware);
			if (!dictionary.TryGetValue(key, out var value))
			{
				configurationManager.Dataset.Hardware.ImportRow(row);
				value = AddItem(configurationManager.Dataset.Hardware.FindByIDHardware(row.IDHardware));
			}
		}
		configurationManager.EndLoadData(configurationManager.Dataset.Hardware);
		configurationManager.DisableEventHandling = false;
	}
}
