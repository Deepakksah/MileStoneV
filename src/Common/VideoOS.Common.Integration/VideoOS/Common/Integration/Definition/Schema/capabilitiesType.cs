using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class capabilitiesType
{
	private capabilityType[] capabilityField;

	private string hwidField;

	[XmlElement("capability")]
	public capabilityType[] capability
	{
		get
		{
			return capabilityField;
		}
		set
		{
			capabilityField = value;
		}
	}

	[XmlAttribute]
	public string hwid
	{
		get
		{
			return hwidField;
		}
		set
		{
			hwidField = value;
		}
	}

	public capabilityType FindCapabilityByType(typeType type)
	{
		if (type == null)
		{
			return null;
		}
		return FindCapabilityByType(type.name);
	}

	public capabilityType FindCapabilityByType(string typeName)
	{
		if (capability == null)
		{
			return null;
		}
		capabilityType[] array = capability;
		foreach (capabilityType capabilityType2 in array)
		{
			if (capabilityType2.type == typeName)
			{
				return capabilityType2;
			}
		}
		return null;
	}

	public capabilityType FindCapabilityByName(string name)
	{
		if (capability == null)
		{
			return null;
		}
		capabilityType[] array = capability;
		foreach (capabilityType capabilityType2 in array)
		{
			if (capabilityType2.name == name)
			{
				return capabilityType2;
			}
		}
		return null;
	}

	public capabilityType FindCapabilityByTypeName(string typeName)
	{
		if (capability == null)
		{
			return null;
		}
		capabilityType[] array = capability;
		foreach (capabilityType capabilityType2 in array)
		{
			if (capabilityType2.type == typeName)
			{
				return capabilityType2;
			}
		}
		return null;
	}
}
