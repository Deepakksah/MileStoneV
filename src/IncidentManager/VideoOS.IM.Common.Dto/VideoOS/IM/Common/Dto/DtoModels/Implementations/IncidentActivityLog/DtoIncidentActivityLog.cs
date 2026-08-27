using System;
using VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.IncidentActivityLog;

namespace VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;

public class DtoIncidentActivityLog : ADtoBaseEntity, IDtoIncidentActivityLog, IDtoBaseEntity
{
	private DateTime? _TimeOfActivityLog;

	private string _Description;

	private string _EntityType;

	private string _EntityId;

	private string _JsonOldValues;

	private string _JsonNewValues;

	private long _IncidentId;

	private DtoIncident _Incident;

	public DateTime? TimeOfActivityLog
	{
		get
		{
			return _TimeOfActivityLog;
		}
		set
		{
			if (_TimeOfActivityLog != value)
			{
				_TimeOfActivityLog = value;
				NotifyPropertyChanged("TimeOfActivityLog");
			}
		}
	}

	public string Description
	{
		get
		{
			return _Description;
		}
		set
		{
			if (_Description != value)
			{
				_Description = value;
				NotifyPropertyChanged("Description");
			}
		}
	}

	public string EntityType
	{
		get
		{
			return _EntityType;
		}
		set
		{
			if (_EntityType != value)
			{
				_EntityType = value;
				NotifyPropertyChanged("EntityType");
			}
		}
	}

	public string EntityId
	{
		get
		{
			return _EntityId;
		}
		set
		{
			if (_EntityId != value)
			{
				_EntityId = value;
				NotifyPropertyChanged("EntityId");
			}
		}
	}

	public string JsonOldValues
	{
		get
		{
			return _JsonOldValues;
		}
		set
		{
			if (_JsonOldValues != value)
			{
				_JsonOldValues = value;
				NotifyPropertyChanged("JsonOldValues");
			}
		}
	}

	public string JsonNewValues
	{
		get
		{
			return _JsonNewValues;
		}
		set
		{
			if (_JsonNewValues != value)
			{
				_JsonNewValues = value;
				NotifyPropertyChanged("JsonNewValues");
			}
		}
	}

	public long IncidentId
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
