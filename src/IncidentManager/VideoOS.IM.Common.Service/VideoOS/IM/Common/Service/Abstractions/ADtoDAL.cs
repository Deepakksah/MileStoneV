using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.User;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.GlobalSettings;
using VideoOS.IM.Common.Service.Common;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Common.Service.Interfaces.DraftSequence;
using VideoOS.IM.Common.Service.Interfaces.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Service.Interfaces.GlobalSettings;
using VideoOS.IM.Common.Service.Interfaces.Incident;
using VideoOS.IM.Common.Service.Interfaces.IncidentActivityLog;
using VideoOS.IM.Common.Service.Interfaces.IncidentCall;
using VideoOS.IM.Common.Service.Interfaces.IncidentComment;
using VideoOS.IM.Common.Service.Interfaces.IncidentSequence;

namespace VideoOS.IM.Common.Service.Abstractions;

public abstract class ADtoDAL : IDtoDAL, INotifyPropertyChanged
{
	public delegate void ConfigurationsChangedEventHandler();

	public static long TimeInSecondsToWaitForReloadingGlobalSettingsFromDatabase = 45L;

	public static DateTime? LastLoadingTimeGlobalSettingsFromDb = null;

	private static DtoGlobalSettingsManager _globalSettingsManager;

	public static DtoGlobalSettingsManager GlobalSettingsManager
	{
		get
		{
			return _globalSettingsManager;
		}
		set
		{
			if (_globalSettingsManager != value)
			{
				if (_globalSettingsManager != null)
				{
					_globalSettingsManager.GlobalSettingsConfigurationChanged -= _GlobalSettingsManager_GlobalSettingsConfigurationChanged;
				}
				_globalSettingsManager = value;
				if (_globalSettingsManager != null)
				{
					_globalSettingsManager.GlobalSettingsConfigurationChanged += _GlobalSettingsManager_GlobalSettingsConfigurationChanged;
				}
				StaticNotifyPropertyChanged("GlobalSettingsManager");
			}
		}
	}

	public IDtoIncidentDAL IncidentDALService { get; set; }

	public IDtoDraftSequenceDAL DraftSequenceDALService { get; set; }

	public IDtoDynamicalDataTypesAndOptionsDAL DynamicalDataTypesAndOptionsDALService { get; set; }

	public IDtoIncidentCallDAL IncidentCallDALService { get; set; }

	public IDtoIncidentSequenceDAL IncidentSequenceDALService { get; set; }

	public IDtoIncidentCommentDAL IncidentCommentDALService { get; set; }

	public IDtoIncidentActivityLogDAL IncidentActivityLogDALService { get; set; }

	public IDtoGlobalSettingsDAL GlobalSettingsDALService { get; set; }

	public static event ConfigurationsChangedEventHandler ConfigurationChanged;

	public static event PropertyChangedEventHandler StaticPropertyChanged;

	public event PropertyChangedEventHandler PropertyChanged;

	public static void NotifyConfigurationChanged()
	{
		ADtoDAL.ConfigurationChanged?.Invoke();
	}

	private static void _GlobalSettingsManager_GlobalSettingsConfigurationChanged()
	{
		NotifyConfigurationChanged();
	}

	public virtual List<DtoGlobalSettings> LoadGlobalSettingsFromDb()
	{
		DtoPaging inPaging = new DtoPaging
		{
			CountPerPage = 10000
		};
		DtoGlobalSettingsListResponse dtoGlobalSettingsListResponse = GetGlobalSettingsDAL()?.GetList(inPaging);
		if (dtoGlobalSettingsListResponse != null && dtoGlobalSettingsListResponse.Success && dtoGlobalSettingsListResponse.Value != null)
		{
			return dtoGlobalSettingsListResponse.Value.ToList();
		}
		return null;
	}

	public virtual bool SaveChangedGlobalSettingsToDb(ManagerOptions inChangedSettings)
	{
		List<DtoGlobalSettings> globalSettingsForUpdate = GlobalSettingsManager.GetGlobalSettingsForUpdate(inChangedSettings);
		if (globalSettingsForUpdate != null && globalSettingsForUpdate.Count > 0)
		{
			foreach (DtoGlobalSettings item in globalSettingsForUpdate)
			{
				if (item != null)
				{
					DtoGlobalSettingsResponse dtoGlobalSettingsResponse = GetGlobalSettingsDAL()?.Update(item);
					if (dtoGlobalSettingsResponse == null || !dtoGlobalSettingsResponse.Success)
					{
						return false;
					}
				}
			}
		}
		List<DtoGlobalSettings> globalSettingsForUpdate2 = GlobalSettingsManager.GetGlobalSettingsForUpdate(inChangedSettings, DtoGlobalSettingsManager._tmpTypeOfGlobalSettingsUpdate.Create);
		if (globalSettingsForUpdate2 != null && globalSettingsForUpdate2.Count > 0)
		{
			foreach (DtoGlobalSettings item2 in globalSettingsForUpdate2)
			{
				if (item2 != null)
				{
					DtoGlobalSettingsResponse dtoGlobalSettingsResponse2 = GetGlobalSettingsDAL()?.Create(item2);
					if (dtoGlobalSettingsResponse2 == null || !dtoGlobalSettingsResponse2.Success)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public virtual void UpdateGlobalSettingsInManager(List<DtoGlobalSettings> inSettings)
	{
		if (GlobalSettingsManager == null)
		{
			GlobalSettingsManager = new DtoGlobalSettingsManager();
		}
		GlobalSettingsManager.SetAllGlobalSettingsFromDb(inSettings);
	}

	public virtual void UpdateMilestoneUserInGlobalSettingsManager(IDtoMilestoneUser inUser)
	{
		if (GlobalSettingsManager == null)
		{
			GlobalSettingsManager = new DtoGlobalSettingsManager();
		}
		GlobalSettingsManager.SetActiveUser(inUser);
	}

	public void InitializeGlobalSettingsManager(IDtoMilestoneUser inUser)
	{
		if (GlobalSettingsManager == null)
		{
			GlobalSettingsManager = new DtoGlobalSettingsManager(inUser);
		}
		GlobalSettingsManager.SetActiveUser(inUser);
		RefreshGlobalSettingsManager();
	}

	public void RefreshGlobalSettingsManager()
	{
		bool flag = false;
		if (!LastLoadingTimeGlobalSettingsFromDb.HasValue)
		{
			flag = true;
		}
		else if ((long)(DateTime.UtcNow - LastLoadingTimeGlobalSettingsFromDb.Value).TotalSeconds > TimeInSecondsToWaitForReloadingGlobalSettingsFromDatabase)
		{
			flag = true;
		}
		if (flag)
		{
			LastLoadingTimeGlobalSettingsFromDb = DateTime.UtcNow;
			List<DtoGlobalSettings> allGlobalSettingsFromDb = LoadGlobalSettingsFromDb();
			GlobalSettingsManager.SetAllGlobalSettingsFromDb(allGlobalSettingsFromDb);
		}
	}

	public virtual IDtoIncidentDAL GetIncidentDAL()
	{
		return IncidentDALService;
	}

	public virtual IDtoDraftSequenceDAL GetDraftSequenceDAL()
	{
		return DraftSequenceDALService;
	}

	public virtual IDtoDynamicalDataTypesAndOptionsDAL GetDynamicalDataTypesAndOptionsDAL()
	{
		return DynamicalDataTypesAndOptionsDALService;
	}

	public virtual IDtoIncidentCallDAL GetIncidentCallDAL()
	{
		return IncidentCallDALService;
	}

	public virtual IDtoIncidentSequenceDAL GetIncidentSequenceDAL()
	{
		return IncidentSequenceDALService;
	}

	public virtual IDtoIncidentCommentDAL GetIncidentCommentDAL()
	{
		return IncidentCommentDALService;
	}

	public virtual IDtoIncidentActivityLogDAL GetIncidentActivityLogDAL()
	{
		return IncidentActivityLogDALService;
	}

	public IDtoGlobalSettingsDAL GetGlobalSettingsDAL()
	{
		return GlobalSettingsDALService;
	}

	private static void StaticNotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		ADtoDAL.StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
	}

	private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
