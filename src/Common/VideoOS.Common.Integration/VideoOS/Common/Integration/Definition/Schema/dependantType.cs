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
public class dependantType
{
	private string hwidField;

	private string capabilityField;

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

	[XmlAttribute]
	public string capability
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
}
