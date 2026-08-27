using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "UpgradeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UpgradeInfo : IUpgradeInfo
{
	private string HostnameField;

	private string MessageField;

	private string PathField;

	private int PortField;

	[DataMember]
	public string Hostname
	{
		get
		{
			return HostnameField;
		}
		set
		{
			HostnameField = value;
		}
	}

	[DataMember]
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			MessageField = value;
		}
	}

	[DataMember]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}

	[DataMember]
	public int Port
	{
		get
		{
			return PortField;
		}
		set
		{
			PortField = value;
		}
	}
}
