using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileWarningTriggerSpecification
{
	public string ID { get; }

	public bool TriggerUnlocked { get; }

	public bool TriggerLocked { get; }

	public bool TriggerOnNonDefaultValue { get; }

	public ClientProfileWarningTriggerSpecification(XmlNode warningTriggerNode)
	{
		ID = warningTriggerNode.SelectSingleNode("@ID").Value;
		if (warningTriggerNode.SelectSingleNode("@TriggerUnlocked") != null && bool.TryParse(warningTriggerNode.SelectSingleNode("@TriggerUnlocked").Value, out var result))
		{
			TriggerUnlocked = result;
		}
		if (warningTriggerNode.SelectSingleNode("@TriggerLocked") != null && bool.TryParse(warningTriggerNode.SelectSingleNode("@TriggerLocked").Value, out var result2))
		{
			TriggerLocked = result2;
		}
		if (warningTriggerNode.SelectSingleNode("@TriggerOnNonDefaultValue") != null && bool.TryParse(warningTriggerNode.SelectSingleNode("@TriggerOnNonDefaultValue").Value, out var result3))
		{
			TriggerOnNonDefaultValue = result3;
		}
	}
}
