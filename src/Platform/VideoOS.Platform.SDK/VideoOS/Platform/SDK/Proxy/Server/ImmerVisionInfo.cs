using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ImmerVisionInfo : IImmerVisionInfo
{
	private string lensProfileRplField;

	private string lensProfileNameField;

	private string lensProfileDataField;

	public string LensProfileRpl
	{
		get
		{
			return lensProfileRplField;
		}
		set
		{
			lensProfileRplField = value;
		}
	}

	public string LensProfileName
	{
		get
		{
			return lensProfileNameField;
		}
		set
		{
			lensProfileNameField = value;
		}
	}

	public string LensProfileData
	{
		get
		{
			return lensProfileDataField;
		}
		set
		{
			lensProfileDataField = value;
		}
	}
}
