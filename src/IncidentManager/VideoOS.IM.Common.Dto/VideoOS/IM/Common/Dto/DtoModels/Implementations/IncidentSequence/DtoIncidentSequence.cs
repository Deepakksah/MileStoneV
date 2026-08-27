using System;
using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DraftSequence;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.IncidentSequence;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentSequence;

public class DtoIncidentSequence : ADtoBaseEntity, IDtoIncidentSequence, IDtoBaseEntity
{
	private Guid _CameraId;

	private string _CameraTitle;

	private DateTime? _StartTime;

	private DateTime? _EndTime;

	private int _OrderNumber;

	private Guid _Tmp_CameraGuid;

	private long? _IncidentId;

	private DtoIncident _Incident;

	private long? _DraftSequenceId;

	private string _User;

	private DtoDraftSequence _DraftSequence;

	public Guid CameraId
	{
		get
		{
			return _CameraId;
		}
		set
		{
			if (_CameraId != value)
			{
				_CameraId = value;
				NotifyPropertyChanged("CameraId");
			}
		}
	}

	public string CameraTitle
	{
		get
		{
			return _CameraTitle;
		}
		set
		{
			if (_CameraTitle != value)
			{
				_CameraTitle = value;
				NotifyPropertyChanged("CameraTitle");
			}
		}
	}

	public DateTime? StartTime
	{
		get
		{
			return _StartTime;
		}
		set
		{
			if (_StartTime != value)
			{
				_StartTime = value;
				NotifyPropertyChanged("StartTime");
			}
		}
	}

	public DateTime? EndTime
	{
		get
		{
			return _EndTime;
		}
		set
		{
			if (_EndTime != value)
			{
				_EndTime = value;
				NotifyPropertyChanged("EndTime");
			}
		}
	}

	public bool IsValid
	{
		get
		{
			if (_StartTime.HasValue)
			{
				return _EndTime.HasValue;
			}
			return false;
		}
	}

	public int OrderNumber
	{
		get
		{
			return _OrderNumber;
		}
		set
		{
			if (_OrderNumber != value)
			{
				_OrderNumber = value;
				NotifyPropertyChanged("OrderNumber");
			}
		}
	}

	public Guid Tmp_CameraGuid
	{
		get
		{
			return _Tmp_CameraGuid;
		}
		set
		{
			if (_Tmp_CameraGuid != value)
			{
				_Tmp_CameraGuid = value;
				NotifyPropertyChanged("Tmp_CameraGuid");
			}
		}
	}

	public long? IncidentId
	{
		get
		{
			return _IncidentId;
		}
		set
		{
			if (_IncidentId != value)
			{
				_IncidentId = value;
				NotifyPropertyChanged("IncidentId");
			}
		}
	}

	public DtoIncident Incident
	{
		get
		{
			return _Incident;
		}
		set
		{
			if (_Incident != value)
			{
				_Incident = value;
				NotifyPropertyChanged("Incident");
			}
		}
	}

	public long? DraftSequenceId
	{
		get
		{
			return _DraftSequenceId;
		}
		set
		{
			if (_DraftSequenceId != value)
			{
				_DraftSequenceId = value;
				NotifyPropertyChanged("DraftSequenceId");
			}
		}
	}

	public string User
	{
		get
		{
			string text = base.CreatedUserName;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = base.ModifiedUserName;
			}
			return text;
		}
		set
		{
			if (_User != value)
			{
				_User = value;
				NotifyPropertyChanged("User");
			}
		}
	}

	public DtoDraftSequence DraftSequence
	{
		get
		{
			return _DraftSequence;
		}
		set
		{
			if (_DraftSequence != value)
			{
				_DraftSequence = value;
				NotifyPropertyChanged("DraftSequence");
			}
		}
	}
}
