using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "OutputInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputInfo : DeviceInfo, IOutputInfo, IDeviceInfo
{
	private OutputSecurityInfo OutputSecurityField;

	[DataMember]
	public OutputSecurityInfo OutputSecurity
	{
		get
		{
			return OutputSecurityField;
		}
		set
		{
			OutputSecurityField = value;
		}
	}

	IOutputSecurityInfo IOutputInfo.OutputSecurity
	{
		get
		{
			return OutputSecurity;
		}
		set
		{
			OutputSecurity = (OutputSecurityInfo)value;
		}
	}
}
