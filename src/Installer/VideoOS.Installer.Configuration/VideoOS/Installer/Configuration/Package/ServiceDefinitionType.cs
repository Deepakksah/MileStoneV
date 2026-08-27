using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class ServiceDefinitionType
{
	private ServiceDefinitionTypeOnParameter onParameterField;

	private string serviceNameField;

	private string serviceTypeField;

	public ServiceDefinitionTypeOnParameter OnParameter
	{
		get
		{
			return onParameterField;
		}
		set
		{
			onParameterField = value;
		}
	}

	[XmlAttribute]
	public string ServiceName
	{
		get
		{
			return serviceNameField;
		}
		set
		{
			serviceNameField = value;
		}
	}

	[XmlAttribute]
	public string ServiceType
	{
		get
		{
			return serviceTypeField;
		}
		set
		{
			serviceTypeField = value;
		}
	}
}
