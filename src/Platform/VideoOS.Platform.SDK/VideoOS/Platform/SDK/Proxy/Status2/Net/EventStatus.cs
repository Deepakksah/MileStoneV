using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventStatus", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class EventStatus : StatusBase
{
	private Guid EventIdField;

	private Guid SourceIdField;

	private KeyValue[] MetadataField;

	[DataMember(IsRequired = true)]
	public Guid EventId
	{
		get
		{
			return EventIdField;
		}
		set
		{
			EventIdField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public Guid SourceId
	{
		get
		{
			return SourceIdField;
		}
		set
		{
			SourceIdField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public KeyValue[] Metadata
	{
		get
		{
			return MetadataField;
		}
		set
		{
			MetadataField = value;
		}
	}
}
