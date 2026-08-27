using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileSectionSpecification
{
	public string ID { get; private set; }

	public string App { get; private set; }

	public string LocalizedName { get; private set; }

	public string LocalizedHeader { get; private set; }

	public Bitmap Image { get; private set; }

	public List<ClientProfileSettingSpecification> Settings { get; private set; }

	public string Feature { get; private set; }

	public ClientProfileSectionSpecification(XmlNode sectionNode, Dictionary<string, ClientProfileControlSpecification> controls, Dictionary<string, ClientProfileWarningSpecification> warnings, Dictionary<string, ClientProfileWarningTriggerSpecification> warningTriggers)
	{
		ID = sectionNode.SelectSingleNode("@ID").Value;
		App = sectionNode.SelectSingleNode("@App").Value;
		string value = sectionNode.SelectSingleNode("@LocalizationIDName").Value;
		LocalizedName = ResourcesUtility.GetResourceString(value);
		value = sectionNode.SelectSingleNode("@LocalizationIDHeader").Value;
		LocalizedHeader = ResourcesUtility.GetResourceString(value);
		string value2 = sectionNode.SelectSingleNode("@IconKey").Value;
		Image = ResourcesUtility.GetResourceImage(value2);
		if (sectionNode.SelectSingleNode("@Feature") != null)
		{
			Feature = sectionNode.SelectSingleNode("@Feature").Value;
		}
		else
		{
			Feature = string.Empty;
		}
		Settings = new List<ClientProfileSettingSpecification>();
		foreach (XmlNode item2 in sectionNode.SelectNodes("Setting"))
		{
			ClientProfileSettingSpecification item = new ClientProfileSettingSpecification(item2, controls, warnings, warningTriggers);
			Settings.Add(item);
		}
	}
}
