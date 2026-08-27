using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus")]
public class EventStatus : StatusBase
{
	private Guid eventIdField;

	private Guid sourceIdField;

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
}
