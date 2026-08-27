using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.Installer.ProductConfig;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public class SchedulesTypeSchedule
{
	private SchedulesTypeScheduleBuildList buildListField;

	private SchedulesTypeScheduleBeforePackage beforePackageField;

	private SchedulesTypeScheduleAfterPackage afterPackageField;

	private string nameField;

	public SchedulesTypeScheduleBuildList BuildList
	{
		get
		{
			return buildListField;
		}
		set
		{
			buildListField = value;
		}
	}

	public SchedulesTypeScheduleBeforePackage BeforePackage
	{
		get
		{
			return beforePackageField;
		}
		set
		{
			beforePackageField = value;
		}
	}

	public SchedulesTypeScheduleAfterPackage AfterPackage
	{
		get
		{
			return afterPackageField;
		}
		set
		{
			afterPackageField = value;
		}
	}

	[XmlAttribute]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}
}
