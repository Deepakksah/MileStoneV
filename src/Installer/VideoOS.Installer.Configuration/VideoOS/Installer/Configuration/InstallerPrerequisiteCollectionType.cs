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
public class InstallerPrerequisiteCollectionType
{
	private PrerequisiteInfoType prerequisiteInfoField;

	private PrerequisiteType typeField;

	public PrerequisiteInfoType PrerequisiteInfo
	{
		get
		{
			return prerequisiteInfoField;
		}
		set
		{
			prerequisiteInfoField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(PrerequisiteType.Required)]
	public PrerequisiteType Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	public InstallerPrerequisiteCollectionType()
	{
		typeField = PrerequisiteType.Required;
	}
}
