using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SystemEventTypeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class SystemEventTypeInfo : IExtensibleDataObject, ISystemEventTypeInfo
{
	private ExtensionDataObject extensionDataField;

	private string DescriptionField;

	private EventSource EventSourceField;

	private Guid EventTypeIdField;

	private string NameField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public EventSource EventSource
	{
		get
		{
			return EventSourceField;
		}
		set
		{
			EventSourceField = value;
		}
	}

	[DataMember]
	public Guid EventTypeId
	{
		get
		{
			return EventTypeIdField;
		}
		set
		{
			EventTypeIdField = value;
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
}
