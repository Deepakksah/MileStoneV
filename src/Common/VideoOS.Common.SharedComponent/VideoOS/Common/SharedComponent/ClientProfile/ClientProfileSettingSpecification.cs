using System.Collections.Generic;
using System.Xml;
using VideoOS.Common.SharedComponent.ClientProfile.Providers;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileSettingSpecification
{
	public string ID { get; private set; }

	public ClientProfileControlSpecification Control { get; private set; }

	public string DefaultValue { get; private set; }

	public bool LockedAvailable { get; private set; }

	public bool DefaultLocked { get; private set; }

	public string LocalizedName { get; private set; }

	public bool IsSeperator { get; set; }

	public string Feature { get; private set; }

	public ClientProfileWarningSpecification Warning { get; }

	public ClientProfileWarningTriggerSpecification WarningTrigger { get; }

	public ClientProfileSettingSpecification(XmlNode sectionNode, Dictionary<string, ClientProfileControlSpecification> controls, Dictionary<string, ClientProfileWarningSpecification> warnings, Dictionary<string, ClientProfileWarningTriggerSpecification> warningTriggers)
	{
		ID = sectionNode.SelectSingleNode("@ID").Value;
		if (sectionNode.SelectSingleNode("@IsSeperator") != null && bool.TryParse(sectionNode.SelectSingleNode("@IsSeperator").Value, out var result))
		{
			IsSeperator = result;
		}
		if (!IsSeperator)
		{
			string value = sectionNode.SelectSingleNode("@ControlID").Value;
			Control = controls[value];
		}
		if (sectionNode.SelectSingleNode("@DefaultValue") != null)
		{
			DefaultValue = sectionNode.SelectSingleNode("@DefaultValue").Value;
		}
		if (sectionNode.SelectSingleNode("@LockedAvailable") != null && bool.TryParse(sectionNode.SelectSingleNode("@LockedAvailable").Value, out var result2))
		{
			LockedAvailable = result2;
		}
		if (sectionNode.SelectSingleNode("@DefaultLocked") != null && bool.TryParse(sectionNode.SelectSingleNode("@DefaultLocked").Value, out var result3))
		{
			DefaultLocked = result3;
		}
		if (sectionNode.SelectSingleNode("@Feature") != null)
		{
			Feature = sectionNode.SelectSingleNode("@Feature").Value;
		}
		else
		{
			Feature = string.Empty;
		}
		if (sectionNode.SelectSingleNode("@WarningID") != null)
		{
			string value2 = sectionNode.SelectSingleNode("@WarningID").Value;
			Warning = warnings[value2];
		}
		if (sectionNode.SelectSingleNode("@WarningTriggerID") != null)
		{
			string value3 = sectionNode.SelectSingleNode("@WarningTriggerID").Value;
			WarningTrigger = warningTriggers[value3];
		}
		string value4 = sectionNode.SelectSingleNode("@LocalizationID").Value;
		LocalizedName = ClientProfileSettingSpecificationProviderName.Instance.GetClientProfileSettingSpecificationName(Feature, value4);
	}
}
