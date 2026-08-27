using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileSpecification
{
	public Dictionary<string, ClientProfileControlSpecification> Controls = new Dictionary<string, ClientProfileControlSpecification>();

	public Dictionary<string, ClientProfileWarningSpecification> Warnings = new Dictionary<string, ClientProfileWarningSpecification>();

	public Dictionary<string, ClientProfileWarningTriggerSpecification> WarningTriggers = new Dictionary<string, ClientProfileWarningTriggerSpecification>();

	public List<ClientProfileSectionSpecification> Sections { get; private set; }

	public ClientProfileSpecification()
	{
		XmlDocument xmlDocument = new XmlDocument();
		StreamReader txtReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("VideoOS.Common.SharedComponent.ClientProfileSpecification.xml"));
		xmlDocument.Load(txtReader);
		foreach (XmlNode item2 in xmlDocument.SelectNodes("/Specification/Controls/Control"))
		{
			ClientProfileControlSpecification clientProfileControlSpecification = new ClientProfileControlSpecification(item2);
			Controls.Add(clientProfileControlSpecification.ID, clientProfileControlSpecification);
		}
		foreach (XmlNode item3 in xmlDocument.SelectNodes("/Specification/Warnings/Warning"))
		{
			ClientProfileWarningSpecification clientProfileWarningSpecification = new ClientProfileWarningSpecification(item3);
			Warnings.Add(clientProfileWarningSpecification.ID, clientProfileWarningSpecification);
		}
		foreach (XmlNode item4 in xmlDocument.SelectNodes("/Specification/WarningTriggers/WarningTrigger"))
		{
			ClientProfileWarningTriggerSpecification clientProfileWarningTriggerSpecification = new ClientProfileWarningTriggerSpecification(item4);
			WarningTriggers.Add(clientProfileWarningTriggerSpecification.ID, clientProfileWarningTriggerSpecification);
		}
		Sections = new List<ClientProfileSectionSpecification>();
		foreach (XmlNode item5 in xmlDocument.SelectNodes("/Specification/Settings/Section"))
		{
			ClientProfileSectionSpecification item = new ClientProfileSectionSpecification(item5, Controls, Warnings, WarningTriggers);
			Sections.Add(item);
		}
	}
}
