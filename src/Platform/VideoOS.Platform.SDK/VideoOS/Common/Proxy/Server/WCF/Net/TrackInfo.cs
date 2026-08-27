using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "TrackInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class TrackInfo : ITrackInfo
{
	private bool EdgeField;

	private Guid TrackIdField;

	[DataMember]
	public bool Edge
	{
		get
		{
			return EdgeField;
		}
		set
		{
			EdgeField = value;
		}
	}

	[DataMember]
	public Guid TrackId
	{
		get
		{
			return TrackIdField;
		}
		set
		{
			TrackIdField = value;
		}
	}
}
