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
public class InstallerSettingsTypeFlowSettings20
{
	private string[] pageSequencerField;

	private InstallerSettingsTypeFlowSettings20AdvancePageSettings advancePageSettingsField;

	[XmlArrayItem("PageName", IsNullable = false)]
	public string[] PageSequencer
	{
		get
		{
			return pageSequencerField;
		}
		set
		{
			pageSequencerField = value;
		}
	}

	public InstallerSettingsTypeFlowSettings20AdvancePageSettings AdvancePageSettings
	{
		get
		{
			return advancePageSettingsField;
		}
		set
		{
			advancePageSettingsField = value;
		}
	}
}
