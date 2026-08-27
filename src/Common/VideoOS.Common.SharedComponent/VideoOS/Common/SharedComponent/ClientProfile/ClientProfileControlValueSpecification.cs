using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileControlValueSpecification
{
	public string ID { get; }

	public string LocalizedName { get; }

	public ClientProfileControlValueSpecification(XmlNode valueNode)
	{
		ID = valueNode.SelectSingleNode("@ID").Value;
		string value = valueNode.SelectSingleNode("@LocalizationID").Value;
		LocalizedName = ResourcesUtility.GetResourceString(value);
	}
}
