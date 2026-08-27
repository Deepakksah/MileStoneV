using System.Collections.Generic;

namespace VideoOS.Common.Integration.Configuration;

public class HostedHardwareItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<int, HostedHardware> dictionary;

	public HostedHardwareItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<int, HostedHardware>(configurationManager, this);
		foreach (ConfigurationDataSet.HostedHardwareRow row in this.configurationManager.Dataset.HostedHardware.Rows)
		{
			AddItem(row);
		}
	}

	public HostedHardware GetHostedHardwareById(int id)
	{
		if (dictionary.TryGetValue(id, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(HostedHardware), id);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHostedHardwareByIds(new int[1] { id });
		if (configurationDataSet.HostedHardware.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(HostedHardware), id);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.HostedHardware);
		ConfigurationDataSet.HostedHardwareRow hostedHardwareRow = (ConfigurationDataSet.HostedHardwareRow)configurationDataSet.HostedHardware.Rows[0];
		configurationManager.Dataset.HostedHardware.ImportRow(hostedHardwareRow);
		ConfigurationDataSet.HostedHardwareRow row = configurationManager.Dataset.HostedHardware.FindBypkid(hostedHardwareRow.pkid);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.HostedHardware);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	public HostedHardware[] GetHostedHardwareByIds(int[] ids)
	{
		List<int> list = new List<int>();
		int[] array = ids;
		foreach (int num in array)
		{
			if (!dictionary.TryGetValue(num, out var _))
			{
				list.Add(num);
			}
		}
		if (list.Count > 0)
		{
			if (!configurationManager.UseDynamicLoad)
			{
				throw ConfigurationItemNotFoundException.Create(typeof(HostedHardware));
			}
			int num2 = 400;
			for (int j = 0; j < list.Count; j += num2)
			{
				int num3 = list.Count - j;
				int num4 = ((num3 > num2) ? num2 : num3);
				int[] array2 = new int[num4];
				list.CopyTo(j, array2, 0, num4);
				using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetHostedHardwareByIds(array2);
				if (configurationDataSet.HostedHardware.Rows.Count != num4)
				{
					throw ConfigurationItemNotFoundException.Create(typeof(HostedHardware));
				}
				configurationManager.DisableEventHandling = true;
				configurationManager.BeginLoadData(configurationManager.Dataset.HostedHardware);
				foreach (ConfigurationDataSet.HostedHardwareRow row2 in configurationDataSet.HostedHardware.Rows)
				{
					configurationManager.Dataset.HostedHardware.ImportRow(row2);
					ConfigurationDataSet.HostedHardwareRow row = configurationManager.Dataset.HostedHardware.FindBypkid(row2.pkid);
					AddItem(row);
				}
				configurationManager.EndLoadData(configurationManager.Dataset.HostedHardware);
				configurationManager.DisableEventHandling = false;
			}
		}
		List<HostedHardware> list2 = new List<HostedHardware>();
		array = ids;
		foreach (int key in array)
		{
			if (!dictionary.TryGetValue(key, out var value2))
			{
				throw ConfigurationItemNotFoundException.Create(typeof(HostedHardware));
			}
			list2.Add(value2);
		}
		return list2.ToArray();
	}

	private HostedHardware AddItem(ConfigurationDataSet.HostedHardwareRow row)
	{
		HostedHardware hostedHardware = new HostedHardware(configurationManager, row);
		dictionary.Add(hostedHardware.Id, hostedHardware);
		return hostedHardware;
	}
}
