using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ApplicationSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ApplicationSecurityInfo : IApplicationSecurityInfo
{
	private SmartClientSecurityInfo SmartClientSecurityField;

	[DataMember]
	public SmartClientSecurityInfo SmartClientSecurity
	{
		get
		{
			return SmartClientSecurityField;
		}
		set
		{
			SmartClientSecurityField = value;
		}
	}

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
}
