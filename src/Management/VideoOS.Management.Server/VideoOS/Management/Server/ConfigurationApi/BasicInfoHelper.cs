using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi;

public class BasicInfoHelper
{
	public const string AddressKey = "address";

	public const string AdminKey = "admin";

	public const string AdditionalKey = "additional";

	public Dictionary<string, TagValue> Address { get; private set; }

	public Dictionary<string, TagValue> Admin { get; private set; }

	public Dictionary<string, TagValue> Additional { get; private set; }

	public BasicInfoHelper(BasicOwnerInfo[] basicOwnerInfo)
	{
		Address = new Dictionary<string, TagValue>();
		Admin = new Dictionary<string, TagValue>();
		Additional = new Dictionary<string, TagValue>();
		if (basicOwnerInfo == null)
		{
			return;
		}
		foreach (BasicOwnerInfo item in basicOwnerInfo.OrderBy((BasicOwnerInfo c1) => int.Parse(c1.Order, CultureInfo.InvariantCulture)))
		{
			AddInfo(item.Id, item.Name, item.Property, int.Parse(item.Order, CultureInfo.InvariantCulture));
		}
	}

	public void AddInfo(string id, string name, string property, int order)
	{
		if (name != null && name.Contains("."))
		{
			string[] array = name.Split('.');
			switch (array[0])
			{
			case "address":
				Address[id] = new TagValue
				{
					Key = name,
					Tag = array[1],
					Value = property,
					Order = order
				};
				break;
			case "admin":
				Admin[id] = new TagValue
				{
					Key = name,
					Tag = array[1],
					Value = property,
					Order = order
				};
				break;
			case "additional":
				Additional[id] = new TagValue
				{
					Key = name,
					Tag = array[1],
					Value = property,
					Order = order
				};
				break;
			}
		}
	}
}
