using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "Change", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class Change : IChange
{
	private ChangeType ChangeTypeField;

	private string IdField;

	private DateTime TimeField;

	[DataMember]
	public ChangeType ChangeType
	{
		get
		{
			return ChangeTypeField;
		}
		set
		{
			ChangeTypeField = value;
		}
	}

	[DataMember]
	public string Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public DateTime Time
	{
		get
		{
			return TimeField;
		}
		set
		{
			TimeField = value;
		}
	}

	IChangeType IChange.ChangeType
	{
		get
		{
			return ChangeType;
		}
		set
		{
			ChangeType = (ChangeType)value;
		}
	}
}
