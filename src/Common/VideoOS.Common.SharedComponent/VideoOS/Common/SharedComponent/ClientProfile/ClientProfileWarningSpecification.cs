using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileWarningSpecification
{
	public string ID { get; }

	public string LocalizedName { get; }

	public List<ClientProfileWarningParameterSpecification> Parameters { get; }

	public ClientProfileWarningSpecification(XmlNode warningNode)
	{
		ID = warningNode.SelectSingleNode("@ID").Value;
		string value = warningNode.SelectSingleNode("@LocalizationID").Value;
		LocalizedName = ClientProfileResources.ResourceManager.GetString(value, ClientProfileResources.Culture);
		Parameters = new List<ClientProfileWarningParameterSpecification>();
		foreach (XmlNode item2 in warningNode.SelectNodes("Parameter"))
		{
			ClientProfileWarningParameterSpecification item = new ClientProfileWarningParameterSpecification(item2);
			Parameters.Add(item);
		}
	}
}
