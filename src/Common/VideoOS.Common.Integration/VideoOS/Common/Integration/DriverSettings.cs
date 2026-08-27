using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;

namespace VideoOS.Common.Integration;

public class DriverSettings : SectionSettings
{
	public static string EmptyXml => new DriverSettings().SaveAsXml();

	public IEnumerable<SettingEntry> AllEntries
	{
		get
		{
			List<SettingEntry> list = new List<SettingEntry>();
			string[] sectionNames = GetSectionNames();
			foreach (string section in sectionNames)
			{
				SettingsCollection sectionSettings = base.GetSectionSettings(section);
				list.AddRange(sectionSettings);
			}
			return list;
		}
	}

	public int Count
	{
		get
		{
			int num = 0;
			string[] sectionNames = GetSectionNames();
			foreach (string section in sectionNames)
			{
				SettingsCollection sectionSettings = GetSectionSettings(section);
				num += sectionSettings.Count;
			}
			return num;
		}
	}

	public DriverSettings()
	{
		base.SectionAttributeText = "hwid";
		base.SettingAttributeText = "typeid";
	}

	public DriverSettings(string xml)
		: this()
	{
		LoadXml(xml, SettingsLoadMethod.Reload);
	}

	public void CreateOrUpdate(string section, string name, string value, string typeId)
	{
		if (typeId == null)
		{
			throw new ArgumentNullException("typeId");
		}
		Set(section, new SettingEntry(section, name, value, typeId.ToUpperInvariant()), allowCreate: true);
	}

	public void Update(string section, string name, string value, bool silent = false)
	{
		Set(section, new SettingEntry(section, name, value), allowCreate: false, silent);
	}

	public void UpdateByTypeId(string section, string typeId, string value)
	{
		string nameByTypeId = base.GetNameByTypeId(section, typeId);
		if (nameByTypeId != null)
		{
			Set(section, new SettingEntry(section, nameByTypeId, value), allowCreate: false);
		}
	}

	public new string Get(string section, string name)
	{
		return base.Get(section, name);
	}

	public new string GetTypeId(string section, string name)
	{
		return base.GetTypeId(section, name);
	}

	public new string GetNameByTypeId(string section, string typeId)
	{
		return base.GetNameByTypeId(section, typeId);
	}

	public new string GetValueByTypeId(string section, string typeId)
	{
		return base.GetValueByTypeId(section, typeId);
	}

	public new void Remove(string section, string name)
	{
		base.Remove(section, name);
	}

	public new void RemoveSection(string section)
	{
		base.RemoveSection(section);
	}

	public SettingEntry GetSettingEntryByTypeId(string section, string typeId)
	{
		return GetSettingByTypeId(section, typeId);
	}

	public IEnumerable<SettingEntry> GetSettingsByTypeReferenceId(string typeId)
	{
		List<SettingEntry> list = new List<SettingEntry>();
		string[] sectionNames = GetSectionNames();
		foreach (string section in sectionNames)
		{
			SettingEntry settingByTypeId = GetSettingByTypeId(section, typeId);
			if (settingByTypeId != null)
			{
				list.Add(settingByTypeId.Clone());
			}
		}
		return list;
	}

	public DriverSettings Clone()
	{
		return new DriverSettings(SaveAsXml());
	}

	public new string[] GetSectionNames()
	{
		return base.GetSectionNames();
	}

	public new SettingsCollection GetSectionSettings(string section)
	{
		return base.GetSectionSettings(section);
	}

	public new string[] GetSectionSettingNames(string section)
	{
		return base.GetSectionSettingNames(section);
	}

	public MultiDriverSettings ConvertToMultiDriverSettings(DeviceDefinition definition)
	{
		if (definition == null)
		{
			throw new ArgumentNullException("definition");
		}
		definition definition2 = definition.MainDefinition.Base;
		deviceType deviceType2 = definition.Base;
		MultiDriverSettings multiDriverSettings = new MultiDriverSettings();
		string[] sectionNames = GetSectionNames();
		foreach (string text in sectionNames)
		{
			string collectionReferenceId = string.Empty;
			capabilitiesType capabilitiesType2 = null;
			if (deviceType2.capabilities.hwid == text)
			{
				capabilitiesType2 = deviceType2.capabilities;
				collectionReferenceId = string.Empty;
			}
			else
			{
				if (deviceType2.streams == null)
				{
					continue;
				}
				streamType[] streams = deviceType2.streams;
				foreach (streamType streamType2 in streams)
				{
					if (streamType2.capabilities.hwid == text)
					{
						capabilitiesType2 = streamType2.capabilities;
						collectionReferenceId = streamType2.referenceid;
						break;
					}
				}
			}
			string[] sectionSettingNames = GetSectionSettingNames(text);
			foreach (string name in sectionSettingNames)
			{
				capabilityType capabilityType2 = capabilitiesType2.FindCapabilityByName(name);
				if (capabilityType2 != null)
				{
					typeType typeType2 = definition2.FindTypeByName(capabilityType2.type);
					if (typeType2 != null)
					{
						string value = Get(text, name);
						multiDriverSettings.Set(collectionReferenceId, typeType2.referenceid, value);
					}
				}
			}
		}
		return multiDriverSettings;
	}

	public void Merge(DriverSettings eventSettings)
	{
		if (eventSettings == null)
		{
			throw new ArgumentNullException("eventSettings");
		}
		string[] sectionNames = eventSettings.GetSectionNames();
		foreach (string section in sectionNames)
		{
			foreach (SettingEntry sectionSetting in eventSettings.GetSectionSettings(section))
			{
				Set(section, sectionSetting, allowCreate: true);
			}
		}
	}
}
