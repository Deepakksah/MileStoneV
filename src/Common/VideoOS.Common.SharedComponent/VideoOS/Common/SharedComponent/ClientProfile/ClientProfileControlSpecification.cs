using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileControlSpecification
{
	public string ID { get; private set; }

	public string Enum { get; private set; }

	public string ControlType { get; private set; }

	public string ComboBoxPluginClassName { get; private set; }

	public string ButtonPluginClassName { get; private set; }

	public string ValidationPluginClassName { get; private set; }

	public string AssemblyFileName { get; private set; }

	public bool ReportDisplayValueAsPicture { get; private set; }

	public List<ClientProfileControlValueSpecification> Elements { get; private set; }

	public ClientProfileControlSpecification(XmlNode controlNode)
	{
		ID = controlNode.SelectSingleNode("@ID").Value;
		if (controlNode.SelectSingleNode("@Enum") != null)
		{
			Enum = controlNode.SelectSingleNode("@Enum").Value;
		}
		ControlType = controlNode.SelectSingleNode("@ControlType").Value;
		if (controlNode.SelectSingleNode("@ComboBoxPluginClassName") != null)
		{
			ComboBoxPluginClassName = controlNode.SelectSingleNode("@ComboBoxPluginClassName").Value;
		}
		if (controlNode.SelectSingleNode("@ButtonPluginClassName") != null)
		{
			ButtonPluginClassName = controlNode.SelectSingleNode("@ButtonPluginClassName").Value;
		}
		if (controlNode.SelectSingleNode("@ValidationPluginClassName") != null)
		{
			ValidationPluginClassName = controlNode.SelectSingleNode("@ValidationPluginClassName").Value;
		}
		if (controlNode.SelectSingleNode("@AssemblyFileName") != null)
		{
			AssemblyFileName = controlNode.SelectSingleNode("@AssemblyFileName").Value;
		}
		if (controlNode.SelectSingleNode("@ReportDisplayValueAsPicture") != null && bool.TryParse(controlNode.SelectSingleNode("@ReportDisplayValueAsPicture").Value, out var result))
		{
			ReportDisplayValueAsPicture = result;
		}
		Elements = new List<ClientProfileControlValueSpecification>();
		foreach (XmlNode item2 in controlNode.SelectNodes("Value"))
		{
			ClientProfileControlValueSpecification item = new ClientProfileControlValueSpecification(item2);
			Elements.Add(item);
		}
	}
}
