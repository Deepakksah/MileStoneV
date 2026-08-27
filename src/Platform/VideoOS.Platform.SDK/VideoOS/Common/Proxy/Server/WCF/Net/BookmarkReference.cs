using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "BookmarkReference", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class BookmarkReference : IBookmarkReference
{
	private string ReferenceField;

	private DateTime TimeField;

	[DataMember]
	public string Reference
	{
		get
		{
			return ReferenceField;
		}
		set
		{
			ReferenceField = value;
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
}
