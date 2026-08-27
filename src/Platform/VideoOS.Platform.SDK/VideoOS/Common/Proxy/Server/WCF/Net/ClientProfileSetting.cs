using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ClientProfileSetting", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ClientProfileSetting : IClientProfileSetting
{
	private string KeyField;

	private bool LockedField;

	private string ValueField;

	[DataMember]
	public string Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			KeyField = value;
		}
	}

	[DataMember]
	public bool Locked
	{
		get
		{
			return LockedField;
		}
		set
		{
			LockedField = value;
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			ValueField = value;
		}
	}
}
