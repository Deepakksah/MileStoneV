using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetAccessControlEventsWithDowntimesResp", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
public class GetAccessControlEventsWithDowntimesResp : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private AccessControlDowntimeData[] DowntimeDataField;

	private AccessControlEventLine[] ResultField;

	private Guid SearchIdField;

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
	public AccessControlDowntimeData[] DowntimeData
	{
		get
		{
			return DowntimeDataField;
		}
		set
		{
			DowntimeDataField = value;
		}
	}

	[DataMember]
	public AccessControlEventLine[] Result
	{
		get
		{
			return ResultField;
		}
		set
		{
			ResultField = value;
		}
	}

	[DataMember]
	public Guid SearchId
	{
		get
		{
			return SearchIdField;
		}
		set
		{
			SearchIdField = value;
		}
	}
}
