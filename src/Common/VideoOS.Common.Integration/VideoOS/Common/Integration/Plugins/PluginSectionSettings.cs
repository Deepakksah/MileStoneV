using System;
using System.Globalization;
using VideoOS.Common.Integration.Definition.Plugin.Schema;

namespace VideoOS.Common.Integration.Plugins;

[Serializable]
public class PluginSectionSettings : Settings
{
	public PluginSectionSettings()
	{
	}

	public PluginSectionSettings(string xml)
		: base(xml)
	{
	}

	public static PluginSectionSettings LoadXml(string xml)
	{
		return new PluginSectionSettings(xml);
	}

	public void ApplyDefaultValues(capabilitiesType capabilities)
	{
		lock (this)
		{
			if (capabilities == null || capabilities.capability == null)
			{
				return;
			}
			capabilityType[] capability = capabilities.capability;
			foreach (capabilityType capabilityType2 in capability)
			{
				if (capabilityType2.type == capabilityTypeType.data)
				{
					dataType dataType2 = (dataType)capabilityType2.Item;
					if (dataType2.datatype.ToString() == "string" && dataType2.defaultvalue.GetType() == typeof(object))
					{
						dataType2.defaultvalue = "";
					}
					Set(capabilityType2.name, Convert.ToString(dataType2.defaultvalue, CultureInfo.InvariantCulture));
				}
				else
				{
					enumerationType enumerationType2 = (enumerationType)capabilityType2.Item;
					Set(capabilityType2.name, enumerationType2.defaultvalue);
				}
			}
		}
	}
}
