using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "InputInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class InputInfo : DeviceInfo, IInputInfo, IDeviceInfo
{
	private InputSecurityInfo InputSecurityField;

	[DataMember]
	public InputSecurityInfo InputSecurity
	{
		get
		{
			return InputSecurityField;
		}
		set
		{
			InputSecurityField = value;
		}
	}

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
}
