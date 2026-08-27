using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "StreamInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class StreamInfo : IStreamInfo
{
	private bool DefaultField;

	private string NameField;

	private Guid StreamIdField;

	[DataMember]
	public bool Default
	{
		get
		{
			return DefaultField;
		}
		set
		{
			DefaultField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public Guid StreamId
	{
		get
		{
			return StreamIdField;
		}
		set
		{
			StreamIdField = value;
		}
	}
}
