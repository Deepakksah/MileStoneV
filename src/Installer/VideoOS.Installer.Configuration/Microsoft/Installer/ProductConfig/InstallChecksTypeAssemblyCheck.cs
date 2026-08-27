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
public class InstallChecksTypeAssemblyCheck
{
	private string nameField;

	private string publicKeyTokenField;

	private string versionField;

	private string cultureField;

	private ProcessorArchitectureType processorArchitectureField;

	private bool processorArchitectureFieldSpecified;

	private string propertyField;

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

	[XmlAttribute]
	public string PublicKeyToken
	{
		get
		{
			return publicKeyTokenField;
		}
		set
		{
			publicKeyTokenField = value;
		}
	}

	[XmlAttribute]
	public string Version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue("neutral")]
	public string Culture
	{
		get
		{
			return cultureField;
		}
		set
		{
			cultureField = value;
		}
	}

	[XmlAttribute]
	public ProcessorArchitectureType ProcessorArchitecture
	{
		get
		{
			return processorArchitectureField;
		}
		set
		{
			processorArchitectureField = value;
		}
	}

	[XmlIgnore]
	public bool ProcessorArchitectureSpecified
	{
		get
		{
			return processorArchitectureFieldSpecified;
		}
		set
		{
			processorArchitectureFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public string Property
	{
		get
		{
			return propertyField;
		}
		set
		{
			propertyField = value;
		}
	}

	public InstallChecksTypeAssemblyCheck()
	{
		cultureField = "neutral";
	}
}
