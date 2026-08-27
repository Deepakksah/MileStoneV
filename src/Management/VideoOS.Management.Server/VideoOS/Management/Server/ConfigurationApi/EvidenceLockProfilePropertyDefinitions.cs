using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class EvidenceLockProfilePropertyDefinitions
{
	public const string RetentionTimeOptions = "RetentionTimeOptions";

	public static Property GetRetentionTimeOptions(List<RetentionTime> retentionTimes)
	{
		Property property = new Property("RetentionTimeOptions", "")
		{
			ValueType = "Array",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.EvidenceLockPropertyRetentionTimeOptions,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "EvidenceLockPropertyRetentionTimeOptions",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
		List<string> list = new List<string>();
		foreach (RetentionTime retentionTime in retentionTimes)
		{
			list.Add(retentionTime.Option.ToString() + ";" + retentionTime.Unit);
		}
		property.ValueArray = list.ToArray();
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}
}
