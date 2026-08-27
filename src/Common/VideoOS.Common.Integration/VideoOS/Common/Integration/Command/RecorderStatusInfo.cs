using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[DebuggerDisplay("Id: {Id}; Service: {ServiceId}; {ConnectionStatus}; {AttachStatus}; {Status}")]
public class RecorderStatusInfo
{
	[DataMember]
	public Guid Id { get; private set; }

	[DataMember]
	public Guid ServiceId { get; private set; }

	[DataMember]
	public RecorderStatuses Status { get; private set; }

	[DataMember]
	public RecorderConnectionStatus ConnectionStatus { get; private set; }

	[DataMember]
	public RecorderAttachStatus AttachStatus { get; private set; }

	public RecorderStatusInfo(Guid recorderId, Guid serviceId, RecorderStatuses status, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus)
	{
		Id = recorderId;
		ServiceId = serviceId;
		Status = status;
		ConnectionStatus = connectionStatus;
		AttachStatus = attachStatus;
	}

	public RecorderStatusInfo Clone()
	{
		return new RecorderStatusInfo(Id, ServiceId, Status, ConnectionStatus, AttachStatus);
	}
}
