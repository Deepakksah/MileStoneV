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
public class InstallerPrerequisitesType
{
	private InstallerPrerequisitesTypeSystemDetections[] systemDetectionsField;

	private WinVersionType winVersionField;

	private InstallerPrerequisiteCollectionType[] installerPrerequisiteField;

	[XmlElement("SystemDetections")]
	public InstallerPrerequisitesTypeSystemDetections[] SystemDetections
	{
		get
		{
			return systemDetectionsField;
		}
		set
		{
			systemDetectionsField = value;
		}
	}

	public WinVersionType WinVersion
	{
		get
		{
			return winVersionField;
		}
		set
		{
			winVersionField = value;
		}
	}

	[XmlElement("InstallerPrerequisite")]
	public InstallerPrerequisiteCollectionType[] InstallerPrerequisite
	{
		get
		{
			return installerPrerequisiteField;
		}
		set
		{
			installerPrerequisiteField = value;
		}
	}
}
