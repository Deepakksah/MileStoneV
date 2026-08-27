using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerSettingsTypeFlowSettings20AdvancePageSettings
{
	private TreckType[] flowTrecksField;

	private string intersectionPageNameField;

	[XmlArrayItem("Treck", IsNullable = false)]
	public TreckType[] FlowTrecks
	{
		get
		{
			return flowTrecksField;
		}
		set
		{
			flowTrecksField = value;
		}
	}

	[XmlAttribute]
	public string IntersectionPageName
	{
		get
		{
			return intersectionPageNameField;
		}
		set
		{
			intersectionPageNameField = value;
		}
	}
}
