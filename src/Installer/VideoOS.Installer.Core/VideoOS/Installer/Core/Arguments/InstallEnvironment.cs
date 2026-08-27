using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Core.Arguments;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class InstallEnvironment
{
	private KeyValueParametersOfStringString[] parametersField;

	private LicenseType licenseField;

	private bool licenseFieldSpecified;

	public KeyValueParametersOfStringString[] Parameters
	{
		get
		{
			return parametersField;
		}
		set
		{
			parametersField = value;
		}
	}

	[XmlAttribute]
	public LicenseType License
	{
		get
		{
			return licenseField;
		}
		set
		{
			licenseField = value;
		}
	}

	[XmlIgnore]
	public bool LicenseSpecified
	{
		get
		{
			return licenseFieldSpecified;
		}
		set
		{
			licenseFieldSpecified = value;
		}
	}
}
