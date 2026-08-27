using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class SystemConfigurationHelper
{
	public static string ConvertCollectionToCsvString(IEnumerable<string> collection)
	{
		return string.Join(", ", collection);
	}

	public static ICollection<string> ConvertCsvStringToCollection(string value)
	{
		List<string> result = new List<string>();
		if (string.IsNullOrEmpty(value))
		{
			return result;
		}
		return (from p in value.Split(',')
			select p.Trim()).ToList();
	}
}
