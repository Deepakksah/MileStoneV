using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.RecorderCommunication;

public class RecorderDataBase : IRecorderData
{
	private Guid _recorderId;

	private Guid _serviceId;

	private RecordingServerServiceType _serviceType;

	private RecorderConnectionStatus _connectionStatus;

	private RecorderAttachStatus _attachStatus;

	private RecorderStatuses _recorderStatus;

	public Guid RecorderId => _recorderId;

	public Guid ServiceId => _serviceId;

	public RecordingServerServiceType ServiceType => _serviceType;

	public RecorderConnectionStatus ConnectionStatus
	{
		get
		{
			return _connectionStatus;
		}
		set
		{
			_connectionStatus = value;
		}
	}

	public RecorderAttachStatus AttachStatus
	{
		get
		{
			return _attachStatus;
		}
		set
		{
			_attachStatus = value;
		}
	}

	public RecorderStatuses RecorderStatus
	{
		get
		{
			return _recorderStatus;
		}
		set
		{
			_recorderStatus = value;
		}
	}

	public RecorderDataBase(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus)
	{
		_recorderId = recorderId;
		_serviceId = serviceId;
		_serviceType = serviceType;
		_connectionStatus = connectionStatus;
		_attachStatus = attachStatus;
		_recorderStatus = recorderStatus;
	}
}
