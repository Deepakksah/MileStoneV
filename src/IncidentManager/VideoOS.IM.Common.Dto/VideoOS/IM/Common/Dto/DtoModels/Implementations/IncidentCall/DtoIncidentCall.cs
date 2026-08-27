using System;
using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.IncidentCall;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;

public class DtoIncidentCall : ADtoBaseEntity, IDtoIncidentCall, IDtoBaseEntity
{
	private DateTime? _Called;

	private DateTime? _Arrived;

	private DateTime? _Departed;

	private string _Notes;

	private DtoDynamicalDataTypesAndOptions _ActionType;

	private long? _IncidentId;

	private DtoIncident _Incident;

	public DateTime? Called
	{
		get
		{
			return _Called;
		}
		set
		{
			if (_Called != value)
			{
				_Called = value;
				NotifyPropertyChanged("Called");
			}
		}
	}

	public DateTime? Arrived
	{
		get
		{
			return _Arrived;
		}
		set
		{
			if (_Arrived != value)
			{
				_Arrived = value;
				NotifyPropertyChanged("Arrived");
			}
		}
	}

	public DateTime? Departed
	{
		get
		{
			return _Departed;
		}
		set
		{
			if (_Departed != value)
			{
				_Departed = value;
				NotifyPropertyChanged("Departed");
			}
		}
	}

	public string Notes
	{
		get
		{
			return _Notes;
		}
		set
		{
			if (_Notes != value)
			{
				_Notes = value;
				NotifyPropertyChanged("Notes");
			}
		}
	}

	public DtoDynamicalDataTypesAndOptions ActionType
	{
		get
		{
			return _ActionType;
		}
		set
		{
			if (_ActionType != value)
			{
				_ActionType = value;
				NotifyPropertyChanged("ActionType");
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
}
