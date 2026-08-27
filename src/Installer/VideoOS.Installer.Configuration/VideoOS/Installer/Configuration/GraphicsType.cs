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
public class GraphicsType
{
	private GraphicsTypeIcon iconField;

	private GraphicsTypeBackgroundImage backgroundImageField;

	private GraphicsTypeRegistryNames registryNamesField;

	public GraphicsTypeIcon Icon
	{
		get
		{
			return iconField;
		}
		set
		{
			iconField = value;
		}
	}

	public GraphicsTypeBackgroundImage BackgroundImage
	{
		get
		{
			return backgroundImageField;
		}
		set
		{
			backgroundImageField = value;
		}
	}

	public GraphicsTypeRegistryNames RegistryNames
	{
		get
		{
			return registryNamesField;
		}
		set
		{
			registryNamesField = value;
		}
	}
}
