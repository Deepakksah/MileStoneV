using System.Collections.Generic;
using System.Linq;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.Utils;

public class ListPropertiesHelper
{
	public static List<KeyValuePair<string, string>> BuildPropertyKeyValuePairsForUI(IEnumerable<LicensePlatePropertyDefinition> licensePlatePropertyDefinitions, IEnumerable<LicensePlateProperty> licensePlateProperties)
	{
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		if (licensePlatePropertyDefinitions != null)
		{
			foreach (LicensePlatePropertyDefinition licensePlatePropertyDefinition in licensePlatePropertyDefinitions.OrderBy((LicensePlatePropertyDefinition x) => x.DisplayOrder))
			{
				string key = licensePlatePropertyDefinition.Name ?? string.Empty;
				string value = string.Empty;
				if (licensePlateProperties != null)
				{
					LicensePlateProperty licensePlateProperty = licensePlateProperties.SingleOrDefault((LicensePlateProperty x) => x.ColumnIndex == licensePlatePropertyDefinition.ColumnIndex);
					if (licensePlateProperty != null)
					{
						value = licensePlateProperty.Value ?? string.Empty;
					}
					list.Add(new KeyValuePair<string, string>(key, value));
				}
			}
		}
		return list;
	}
}
