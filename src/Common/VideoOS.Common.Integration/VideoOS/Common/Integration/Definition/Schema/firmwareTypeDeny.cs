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
[XmlType(AnonymousType = true)]
public class firmwareTypeDeny
{
	private string versionField;

	private string fromversionField;

	private string toversionField;

	[XmlAttribute]
	public string version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	[XmlAttribute]
	public string fromversion
	{
		get
		{
			return fromversionField;
		}
		set
		{
			fromversionField = value;
		}
	}

	[XmlAttribute]
	public string toversion
	{
		get
		{
			return toversionField;
		}
		set
		{
			toversionField = value;
		}
	}
}
