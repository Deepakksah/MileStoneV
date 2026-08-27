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
public class deviceconnectionType
{
	private bool allowtcpcheckField;

	[XmlAttribute]
	public bool allowtcpcheck
	{
		get
		{
			return allowtcpcheckField;
		}
		set
		{
			allowtcpcheckField = value;
		}
	}
}
