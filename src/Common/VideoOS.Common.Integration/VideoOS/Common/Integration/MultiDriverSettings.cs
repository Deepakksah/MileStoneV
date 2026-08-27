using System;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration;

public class MultiDriverSettings : SectionSettings
{
	public MultiDriverSettings()
	{
		base.SectionAttributeText = "referenceid";
	}

	public MultiDriverSettings(string xml)
		: this()
	{
		LoadXml(xml, SettingsLoadMethod.Reload);
	}

	public void Set(string collectionReferenceId, string typeId, string value)
	{
		Set(collectionReferenceId, new SettingEntry(collectionReferenceId, typeId, value), allowCreate: true);
	}

	public new string Get(string collectionReferenceId, string typeId)
	{
		return base.Get(collectionReferenceId, typeId);
	}

	public new string[] GetSectionNames()
	{
		return base.GetSectionNames();
	}

	public new string[] GetSectionSettingNames(string collectionReferenceId)
	{
		return base.GetSectionSettingNames(collectionReferenceId);
	}

	public DriverSettings ConvertToDriverSettings(DeviceDefinition definition)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		definition definition2 = definition.MainDefinition.Base;
		deviceType deviceType2 = definition.Base;
		DriverSettings driverSettings = new DriverSettings();
		string[] sectionNames = GetSectionNames();
		foreach (string text in sectionNames)
		{
			string empty = string.Empty;
			capabilitiesType capabilitiesType2 = null;
			if (text.Length == 0)
			{
				capabilitiesType2 = deviceType2.capabilities;
				empty = deviceType2.capabilities.hwid;
			}
			else
			{
				streamType streamType2 = deviceType2.FindStreamByReferenceId(text);
				if (streamType2 == null)
				{
					continue;
				}
				capabilitiesType2 = streamType2.capabilities;
				empty = streamType2.capabilities.hwid;
			}
			string[] sectionSettingNames = GetSectionSettingNames(text);
			foreach (string text2 in sectionSettingNames)
			{
				string typeName = definition2.FindTypeNameByReferenceId(new Guid(text2));
				capabilityType capabilityType2 = capabilitiesType2.FindCapabilityByTypeName(typeName);
				if (capabilityType2 != null)
				{
					string value = Get(text, text2);
					driverSettings.CreateOrUpdate(empty, capabilityType2.name, value, text2);
				}
			}
		}
		return driverSettings;
	}
}
