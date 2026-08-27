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
public class supportedeventType
{
	private string eventreferenceidField;

	[XmlAttribute]
	public string eventreferenceid
	{
		get
		{
			return eventreferenceidField;
		}
		set
		{
			eventreferenceidField = value;
		}
	}
}
