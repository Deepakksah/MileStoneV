using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class EventStatus : StatusBase
{
	private Guid eventIdField;

	private Guid sourceIdField;

	private KeyValue[] metadataField;

	public Guid EventId
	{
		get
		{
			return eventIdField;
		}
		set
		{
			eventIdField = value;
		}
	}

	public Guid SourceId
	{
		get
		{
			return sourceIdField;
		}
		set
		{
			sourceIdField = value;
		}
	}

	public KeyValue[] Metadata
	{
		get
		{
			return metadataField;
		}
		set
		{
			metadataField = value;
		}
	}
}
