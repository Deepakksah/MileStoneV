using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "RestrictedMediaLive", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaLive : IRestrictedMediaLive
{
	private Guid DeviceIdField;

	private DateTime LastModifiedField;

	private DateTime StartTimeField;

	private string UserNameField;

	[DataMember]
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			DeviceIdField = value;
		}
	}

	[DataMember]
	public DateTime LastModified
	{
		get
		{
			return LastModifiedField;
		}
		set
		{
			LastModifiedField = value;
		}
	}

	[DataMember]
	public DateTime StartTime
	{
		get
		{
			return StartTimeField;
		}
		set
		{
			StartTimeField = value;
		}
	}

	[DataMember]
	public string UserName
	{
		get
		{
			return UserNameField;
		}
		set
		{
			UserNameField = value;
		}
	}
}
