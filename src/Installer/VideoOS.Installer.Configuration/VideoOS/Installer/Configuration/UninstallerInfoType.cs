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
[XmlType(Namespace = "urn:milestone-systems")]
public class UninstallerInfoType
{
	private PackageRefType[] uninstallerSequencerField;

	private ARPEntryType aRPEntryField;

	[XmlArrayItem("PackageRef", IsNullable = false)]
	public PackageRefType[] UninstallerSequencer
	{
		get
		{
			return uninstallerSequencerField;
		}
		set
		{
			uninstallerSequencerField = value;
		}
	}

	public ARPEntryType ARPEntry
	{
		get
		{
			return aRPEntryField;
		}
		set
		{
			aRPEntryField = value;
		}
	}
}
