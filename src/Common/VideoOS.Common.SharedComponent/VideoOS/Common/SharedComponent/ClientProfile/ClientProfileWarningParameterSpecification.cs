using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public class ClientProfileWarningParameterSpecification
{
	public string Value { get; }

	public int Index { get; }

	public ClientProfileWarningParameterTypes Type { get; }

	public ClientProfileWarningParameterSpecification(XmlNode parameterNode)
	{
		string value = parameterNode.SelectSingleNode("@OEMID").Value;
		Value = ResourcesUtility.GetOemResource(value);
		Index = int.Parse(parameterNode.SelectSingleNode("@Index").Value, CultureInfo.InvariantCulture);
		if (parameterNode.SelectSingleNode("@Type") != null)
		{
			Type = (ClientProfileWarningParameterTypes)Enum.Parse(typeof(ClientProfileWarningParameterTypes), parameterNode.SelectSingleNode("@Type").Value);
		}
	}
}
