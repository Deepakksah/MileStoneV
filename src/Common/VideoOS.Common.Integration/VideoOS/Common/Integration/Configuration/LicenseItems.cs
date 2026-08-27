using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Configuration;

public class LicenseItems
{
	private ConfigurationManager configurationManager;

	private Dictionary<Guid, License> dictionary;

	public LicenseItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new Dictionary<Guid, License>();
		foreach (ConfigurationDataSet.LicenseRow row in this.configurationManager.Dataset.License.Rows)
		{
			AddItem(row);
		}
	}

	public License AddNew(Guid id, byte[] data)
	{
		ConfigurationDataSet.LicenseRow row = configurationManager.Dataset.License.AddLicenseRow(DateTime.UtcNow, id, data);
		return AddItem(row);
	}

	public void Remove(Guid id)
	{
		License licenseByLicenseId = GetLicenseByLicenseId(id);
		dictionary.Remove(id);
		licenseByLicenseId.DataRow.Delete();
	}

	public License GetLicenseByLicenseId(Guid id)
	{
		if (dictionary.TryGetValue(id, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(License), id);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetLicenseByLicenseId(id);
		if (configurationDataSet.License.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(License), id);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.License);
		ConfigurationDataSet.LicenseRow licenseRow = (ConfigurationDataSet.LicenseRow)configurationDataSet.License.Rows[0];
		configurationManager.Dataset.License.ImportRow(licenseRow);
		ConfigurationDataSet.LicenseRow row = configurationManager.Dataset.License.FindByIDLicense(licenseRow.IDLicense);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.License);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	public void TryPreloadLicenseItems(IEnumerable<Guid> licenseIds)
	{
		List<Guid> list = new List<Guid>();
		foreach (Guid licenseId in licenseIds)
		{
			if (!dictionary.TryGetValue(licenseId, out var _))
			{
				list.Add(licenseId);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.Create(typeof(License));
		}
		int num = 200;
		for (int i = 0; i < list.Count; i += num)
		{
			int num2 = list.Count - i;
			int num3 = ((num2 > num) ? num : num2);
			Guid[] array = new Guid[num3];
			list.CopyTo(i, array, 0, num3);
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetLicensesByLicenseIds(array);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.License);
			foreach (ConfigurationDataSet.LicenseRow item in configurationDataSet.License)
			{
				configurationManager.Dataset.License.ImportRow(item);
				AddItem(configurationManager.Dataset.License.FindByIDLicense(item.IDLicense));
			}
			configurationManager.EndLoadData(configurationManager.Dataset.License);
			configurationManager.DisableEventHandling = false;
		}
	}

	private License AddItem(ConfigurationDataSet.LicenseRow row)
	{
		License license = new License(configurationManager, row);
		dictionary[license.IDLicense] = license;
		return license;
	}
}
