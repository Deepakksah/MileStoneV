using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.Common;

namespace VideoOS.IM.Common.Dto.DtoModels.Abstractions.Common;

public abstract class ADtoBaseEntity : IDtoBaseEntity, INotifyPropertyChanged
{
	private long _Id;

	private DateTime? _TimeCreated;

	private string _CreatedUserId;

	private DateTime? _TimeModified;

	private string _ModifiedUserId;

	private string _SystemString;

	private string _CreatedUserName;

	private string _ModifiedUserName;

	private bool _UiIsSelected;

	private bool _FlagNoDbUserOpenedIncident;

	private string _FlagNoDbUserOpenedIncidentStrInfo;

	private bool _FlagNoDbUpdatingInSingleActivityLogNoNewOperatorActivityLog;

	public long Id
	{
		get
		{
			return _Id;
		}
		set
		{
			if (_Id != value)
			{
				_Id = value;
				NotifyPropertyChanged("Id");
			}
		}
	}

	public DateTime? TimeCreated
	{
		get
		{
			return _TimeCreated;
		}
		set
		{
			if (_TimeCreated != value)
			{
				_TimeCreated = value;
				NotifyPropertyChanged("TimeCreated");
			}
		}
	}

	public string CreatedUserId
	{
		get
		{
			return _CreatedUserId;
		}
		set
		{
			if (_CreatedUserId != value)
			{
				_CreatedUserId = value;
				NotifyPropertyChanged("CreatedUserId");
			}
		}
	}

	public DateTime? TimeModified
	{
		get
		{
			return _TimeModified;
		}
		set
		{
			if (_TimeModified != value)
			{
				_TimeModified = value;
				NotifyPropertyChanged("TimeModified");
			}
		}
	}

	public string ModifiedUserId
	{
		get
		{
			return _ModifiedUserId;
		}
		set
		{
			if (_ModifiedUserId != value)
			{
				_ModifiedUserId = value;
				NotifyPropertyChanged("ModifiedUserId");
			}
		}
	}

	public string SystemString
	{
		get
		{
			return _SystemString;
		}
		set
		{
			if (_SystemString != value)
			{
				_SystemString = value;
				NotifyPropertyChanged("SystemString");
			}
		}
	}

	public string CreatedUserName
	{
		get
		{
			return _CreatedUserName;
		}
		set
		{
			if (_CreatedUserName != value)
			{
				_CreatedUserName = value;
				NotifyPropertyChanged("CreatedUserName");
			}
		}
	}

	public string ModifiedUserName
	{
		get
		{
			return _ModifiedUserName;
		}
		set
		{
			if (_ModifiedUserName != value)
			{
				_ModifiedUserName = value;
				NotifyPropertyChanged("ModifiedUserName");
			}
		}
	}

	public bool UIIsSelected
	{
		get
		{
			return _UiIsSelected;
		}
		set
		{
			if (_UiIsSelected != value)
			{
				_UiIsSelected = value;
				NotifyPropertyChanged("UIIsSelected");
			}
		}
	}

	public bool FlagNoDb_UserOpenedIncident
	{
		get
		{
			return _FlagNoDbUserOpenedIncident;
		}
		set
		{
			if (_FlagNoDbUserOpenedIncident != value)
			{
				_FlagNoDbUserOpenedIncident = value;
				NotifyPropertyChanged("FlagNoDb_UserOpenedIncident");
			}
		}
	}

	public string FlagNoDb_UserOpenedIncidentStrInfo
	{
		get
		{
			return _FlagNoDbUserOpenedIncidentStrInfo;
		}
		set
		{
			if (_FlagNoDbUserOpenedIncidentStrInfo != value)
			{
				_FlagNoDbUserOpenedIncidentStrInfo = value;
				NotifyPropertyChanged("FlagNoDb_UserOpenedIncidentStrInfo");
			}
		}
	}

	public bool FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog
	{
		get
		{
			return _FlagNoDbUpdatingInSingleActivityLogNoNewOperatorActivityLog;
		}
		set
		{
			if (_FlagNoDbUpdatingInSingleActivityLogNoNewOperatorActivityLog != value)
			{
				_FlagNoDbUpdatingInSingleActivityLogNoNewOperatorActivityLog = value;
				NotifyPropertyChanged("FlagNoDb_ResavingUpdateingInSingleActivityLogNoNewOperatorActivityLog");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public virtual ADtoBaseEntity ShallowCopy()
	{
		return (ADtoBaseEntity)MemberwiseClone();
	}

	public virtual ADtoBaseEntity DeepCopy()
	{
		return (ADtoBaseEntity)MemberwiseClone();
	}

	protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
