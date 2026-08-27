using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Plugin;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public class PropertyConverter
{
	public static IEnumerable<ACProperty> ToACProperties(IEnumerable<Property> properties)
	{
		List<ACProperty> list = new List<ACProperty>();
		if (properties != null)
		{
			foreach (Property property in properties)
			{
				if (property != null)
				{
					list.Add(new ACProperty(property.Key, property.Value));
				}
			}
		}
		return list;
	}

	public static IEnumerable<Property> ToProperties(IEnumerable<ACProperty> properties)
	{
		List<Property> list = new List<Property>();
		if (properties != null)
		{
			foreach (ACProperty property in properties)
			{
				if (property != null)
				{
					list.Add(new Property
					{
						Key = property.Key,
						Value = property.Value
					});
				}
			}
		}
		return list;
	}
}
