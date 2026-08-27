using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class SystemEventTypeInfo : ISystemEventTypeInfo
{
	private Guid eventTypeIdField;

	private string nameField;

	private string descriptionField;

	private EventSource eventSourceField;

	VideoOS.Platform.SDK.Interface.EventSource ISystemEventTypeInfo.EventSource
	{
		get
		{
			return EventSource switch
			{
				EventSource.Device => VideoOS.Platform.SDK.Interface.EventSource.Device, 
				EventSource.External => VideoOS.Platform.SDK.Interface.EventSource.External, 
				EventSource.Hardware => VideoOS.Platform.SDK.Interface.EventSource.Hardware, 
				EventSource.Recorder => VideoOS.Platform.SDK.Interface.EventSource.Recorder, 
				EventSource.Server => VideoOS.Platform.SDK.Interface.EventSource.Server, 
				EventSource.System => VideoOS.Platform.SDK.Interface.EventSource.System, 
				EventSource.Timer => VideoOS.Platform.SDK.Interface.EventSource.Timer, 
				EventSource.Undefined => VideoOS.Platform.SDK.Interface.EventSource.Undefined, 
				_ => VideoOS.Platform.SDK.Interface.EventSource.Undefined, 
			};
		}
		set
		{
			switch (value)
			{
			case VideoOS.Platform.SDK.Interface.EventSource.Device:
				EventSource = EventSource.Device;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.External:
				EventSource = EventSource.External;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.Hardware:
				EventSource = EventSource.Hardware;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.Recorder:
				EventSource = EventSource.Recorder;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.Server:
				EventSource = EventSource.Server;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.System:
				EventSource = EventSource.System;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.Timer:
				EventSource = EventSource.Timer;
				break;
			case VideoOS.Platform.SDK.Interface.EventSource.Undefined:
				EventSource = EventSource.Undefined;
				break;
			default:
				EventSource = EventSource.Undefined;
				break;
			}
		}
	}

	public Guid EventTypeId
	{
		get
		{
			return eventTypeIdField;
		}
		set
		{
			eventTypeIdField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public EventSource EventSource
	{
		get
		{
			return eventSourceField;
		}
		set
		{
			eventSourceField = value;
		}
	}
}
