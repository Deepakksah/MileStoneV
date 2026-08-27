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
public class InputInfo : DeviceInfo, IInputInfo, IDeviceInfo
{
	private InputSecurityInfo inputSecurityField;

	IInputSecurityInfo IInputInfo.InputSecurity
	{
		get
		{
			return InputSecurity;
		}
		set
		{
			InputSecurity = (InputSecurityInfo)value;
		}
	}

	public InputSecurityInfo InputSecurity
	{
		get
		{
			return inputSecurityField;
		}
		set
		{
			inputSecurityField = value;
		}
	}
}
