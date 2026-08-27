using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Platform.Util;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;

public class ItemReader : IItemReader
{
	private readonly IStringCollectionConverter _collectionConverter = new StringCollectionConverter(new StringSplitter());

	public ConfigurationItem Item { get; }

	public LicensePlateList[] LicensePlateLists { get; }

	public LicensePlateList ItemLicensePlateList { get; }

	public Guid? Id { get; set; }

	public string Name { get; set; }

	public string[] TriggeredEvents { get; set; }

	public string[] CustomFields { get; set; }

	public ItemReader(ConfigurationItem item, LicensePlateList[] licensePlateLists)
	{
		item.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Validators\\ItemReader.cs", 27);
		Item = item;
		if (item.Properties != null)
		{
			InitProperties(item.Properties);
		}
		LicensePlateLists = licensePlateLists;
		ItemLicensePlateList = licensePlateLists?.FirstOrDefault(delegate(LicensePlateList s)
		{
			Guid id = s.Id;
			Guid? id2 = Id;
			return id == id2;
		});
	}

	private void InitProperties(Property[] properties)
	{
		if (((IEnumerable<Property>)properties).TryGetPropertyValue("Id", out string value) && Guid.TryParse(value, out var result))
		{
			Id = result;
		}
		if (((IEnumerable<Property>)properties).TryGetPropertyValue("Name", out string value2))
		{
			Name = value2;
		}
		if (((IEnumerable<Property>)properties).TryGetPropertyValue("TriggerEventList", out string value3))
		{
			TriggeredEvents = (from s in value3.Split(',')
				select s.Trim() into s
				where !string.IsNullOrEmpty(s)
				select s).ToArray();
		}
		if (((IEnumerable<Property>)properties).TryGetPropertyValue("CustomFields", out string value4))
		{
			CustomFields = _collectionConverter.StringToArray(value4);
		}
	}
}
