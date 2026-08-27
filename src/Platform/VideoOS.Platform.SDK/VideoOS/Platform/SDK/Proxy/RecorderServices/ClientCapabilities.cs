using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class ClientCapabilities
{
	private bool privacyMaskField;

	private int privacyMaskVersionField;

	public bool PrivacyMask
	{
		get
		{
			return privacyMaskField;
		}
		set
		{
			privacyMaskField = value;
		}
	}

	public int PrivacyMaskVersion
	{
		get
		{
			return privacyMaskVersionField;
		}
		set
		{
			privacyMaskVersionField = value;
		}
	}
}
