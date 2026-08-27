using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "OutputSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class OutputSecurityInfo : IOutputSecurityInfo
{
	private bool ActivateField;

	[DataMember]
	public bool Activate
	{
		get
		{
			return ActivateField;
		}
		set
		{
			ActivateField = value;
		}
	}
}
