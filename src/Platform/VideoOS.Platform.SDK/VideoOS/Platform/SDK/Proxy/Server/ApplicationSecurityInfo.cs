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
public class ApplicationSecurityInfo : IApplicationSecurityInfo
{
	private SmartClientSecurityInfo smartClientSecurityField;

	ISmartClientSecurityInfo IApplicationSecurityInfo.SmartClientSecurity
	{
		get
		{
			return SmartClientSecurity;
		}
		set
		{
			SmartClientSecurity = (SmartClientSecurityInfo)value;
		}
	}

	public SmartClientSecurityInfo SmartClientSecurity
	{
		get
		{
			return smartClientSecurityField;
		}
		set
		{
			smartClientSecurityField = value;
		}
	}
}
