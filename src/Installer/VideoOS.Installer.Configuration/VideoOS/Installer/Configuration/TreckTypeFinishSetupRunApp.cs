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
public class TreckTypeFinishSetupRunApp
{
	private TreckTypeFinishSetupRunAppDependentPackage dependentPackageField;

	private string appTitleField;

	private string appExecField;

	private bool checkedField;

	public TreckTypeFinishSetupRunAppDependentPackage DependentPackage
	{
		get
		{
			return dependentPackageField;
		}
		set
		{
			dependentPackageField = value;
		}
	}

	[XmlAttribute]
	public string AppTitle
	{
		get
		{
			return appTitleField;
		}
		set
		{
			appTitleField = value;
		}
	}

	[XmlAttribute]
	public string AppExec
	{
		get
		{
			return appExecField;
		}
		set
		{
			appExecField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(true)]
	public bool Checked
	{
		get
		{
			return checkedField;
		}
		set
		{
			checkedField = value;
		}
	}

	public TreckTypeFinishSetupRunApp()
	{
		checkedField = true;
	}
}
