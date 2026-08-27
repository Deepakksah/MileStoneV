using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoOS.Management.Common;
using VideoOS.Vms.Types;

namespace VideoOS.Common.Integration.ServiceController;

[Serializable]
public sealed class RecorderFailoverServiceState : INotifyPropertyChanged
{
	private ServiceState _status;

	private ConnectStatus _connectStatus;

	private volatile WatchingState _watchingState;

	private RecorderWatchItem[] _recorderWatchItems = new RecorderWatchItem[0];

	private string _failingRecorderHostName;

	private int _failingRecorderHostPort;

	private Guid _failingRecorderId;

	private bool _hasSslErrorWithLocalRecorder;

	private bool _hasAuthorizationErrorWithLocalRecorder;

	private bool _localRecorderStartupError;

	public ServiceState ServiceStatus
	{
		get
		{
			return _status;
		}
		set
		{
			if (_status != value)
			{
				_status = value;
				OnPropertyChanged("ServiceStatus");
			}
		}
	}

	public ConnectStatus ConnectStatus
	{
		get
		{
			return _connectStatus;
		}
		set
		{
			if (_connectStatus != value)
			{
				_connectStatus = value;
				OnPropertyChanged("ConnectStatus");
			}
		}
	}

	public WatchingState WatchingState
	{
		get
		{
			return _watchingState;
		}
		set
		{
			if (_watchingState != value)
			{
				_watchingState = value;
				OnPropertyChanged("WatchingState");
			}
		}
	}

	public bool HasSslErrorWithLocalRecorder
	{
		get
		{
			return _hasSslErrorWithLocalRecorder;
		}
		set
		{
			if (_hasSslErrorWithLocalRecorder != value)
			{
				_hasSslErrorWithLocalRecorder = value;
				OnPropertyChanged("HasSslErrorWithLocalRecorder");
			}
		}
	}

	public bool HasAuthorizationErrorWithLocalRecorder
	{
		get
		{
			return _hasAuthorizationErrorWithLocalRecorder;
		}
		set
		{
			if (_hasAuthorizationErrorWithLocalRecorder != value)
			{
				_hasAuthorizationErrorWithLocalRecorder = value;
				OnPropertyChanged("HasAuthorizationErrorWithLocalRecorder");
			}
		}
	}

	public bool LocalRecorderStartupError
	{
		get
		{
			return _localRecorderStartupError;
		}
		set
		{
			if (_localRecorderStartupError != value)
			{
				_localRecorderStartupError = value;
				OnPropertyChanged("LocalRecorderStartupError");
			}
		}
	}

	public string FailingRecorderHostName
	{
		get
		{
			return _failingRecorderHostName;
		}
		set
		{
			if (_failingRecorderHostName != value)
			{
				_failingRecorderHostName = value;
				OnPropertyChanged("FailingRecorderHostName");
			}
		}
	}

	public int FailingRecorderHostPort
	{
		get
		{
			return _failingRecorderHostPort;
		}
		set
		{
			if (_failingRecorderHostPort != value)
			{
				_failingRecorderHostPort = value;
				OnPropertyChanged("FailingRecorderHostPort");
			}
		}
	}

	public Guid FailingRecorderId
	{
		get
		{
			return _failingRecorderId;
		}
		set
		{
			if (_failingRecorderId != value)
			{
				_failingRecorderId = value;
				OnPropertyChanged("FailingRecorderId");
			}
		}
	}

	public RecorderWatchItem[] RecorderWatchItems
	{
		get
		{
			return _recorderWatchItems;
		}
		set
		{
			if (_recorderWatchItems != value)
			{
				_recorderWatchItems = value;
				OnPropertyChanged("RecorderWatchItems");
			}
		}
	}

	[field: NonSerialized]
	public event PropertyChangedEventHandler PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public FailoverStatusDto ToDto()
	{
		CommunicationErrors communicationErrors = CommunicationErrors.None;
		if (HasSslErrorWithLocalRecorder)
		{
			communicationErrors |= CommunicationErrors.SslCommunication;
		}
		if (HasAuthorizationErrorWithLocalRecorder)
		{
			communicationErrors |= CommunicationErrors.AuthorizationError;
		}
		return new FailoverStatusDto
		{
			Status = ServiceStatus.ToString(),
			ConnectStatus = ConnectStatus.ToString(),
			WatchingState = WatchingState.ToString(),
			RecorderWatchItems = (from rwi in RecorderWatchItems
				where rwi.Position != -1
				select rwi.ToDto()).ToArray(),
			FailingRecorderHostName = FailingRecorderHostName,
			FailingRecorderHostPort = FailingRecorderHostPort,
			FailingRecorderId = FailingRecorderId,
			LocalRecorderCommunicationErrors = communicationErrors,
			LocalRecorderStartupError = LocalRecorderStartupError
		};
	}

	public static RecorderFailoverServiceState FromDto(FailoverStatusDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		if (!Enum.TryParse<ConnectStatus>(dto.ConnectStatus, out var result))
		{
			result = ConnectStatus.Reconnecting;
		}
		if (!Enum.TryParse<ServiceState>(dto.Status, out var result2))
		{
			result2 = ServiceState.Indeterminate;
		}
		if (!Enum.TryParse<WatchingState>(dto.WatchingState, out var result3))
		{
			result3 = WatchingState.Unknown;
		}
		return new RecorderFailoverServiceState
		{
			ConnectStatus = result,
			FailingRecorderHostName = dto.FailingRecorderHostName,
			FailingRecorderHostPort = dto.FailingRecorderHostPort,
			FailingRecorderId = dto.FailingRecorderId,
			RecorderWatchItems = (from rwi in dto.RecorderWatchItems
				where rwi.Position != -1
				select RecorderWatchItem.FromDto(rwi)).ToArray(),
			ServiceStatus = result2,
			WatchingState = result3,
			HasSslErrorWithLocalRecorder = dto.LocalRecorderCommunicationErrors.HasFlag(CommunicationErrors.SslCommunication),
			HasAuthorizationErrorWithLocalRecorder = dto.LocalRecorderCommunicationErrors.HasFlag(CommunicationErrors.AuthorizationError),
			LocalRecorderStartupError = dto.LocalRecorderStartupError
		};
	}
}
