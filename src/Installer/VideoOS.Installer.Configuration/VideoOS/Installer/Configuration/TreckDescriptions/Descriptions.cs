using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.TreckDescriptions;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class Descriptions
{
	private string languageField;

	private TreckDescriptionType[] treckDescriptionsField;

	public string Language
	{
		get
		{
			return languageField;
		}
		set
		{
			languageField = value;
		}
	}

	[XmlArrayItem("TreckDescription", IsNullable = false)]
	public TreckDescriptionType[] TreckDescriptions
	{
		get
		{
			return treckDescriptionsField;
		}
		set
		{
			treckDescriptionsField = value;
		}
	}
}
