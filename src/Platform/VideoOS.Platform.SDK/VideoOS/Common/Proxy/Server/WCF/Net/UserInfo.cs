using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "UserInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UserInfo : IUserInfo
{
	private string nameField;

	private string sidField;

	[DataMember]
	public string name
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

	[DataMember]
	public string sid
	{
		get
		{
			return sidField;
		}
		set
		{
			sidField = value;
		}
	}
}
