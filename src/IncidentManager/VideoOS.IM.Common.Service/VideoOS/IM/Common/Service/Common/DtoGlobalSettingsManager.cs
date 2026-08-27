using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoModels.Interfaces.User;
using VideoOS.IM.Common.GlobalDefinitions.Configuration;

namespace VideoOS.IM.Common.Service.Common;

public class DtoGlobalSettingsManager : IDtoGlobalSettingsManager, INotifyPropertyChanged
{
	public delegate void GlobalSettingsConfigurationChangedEventHandler();

	public enum _tmpTypeOfGlobalSettingsUpdate
	{
		Update,
		Create,
		Delete
	}

	public const string KeyId_IncidentManager_GlobalDefaultAccessTypeAllowEverything = "{incidentManager-globalDefaultAccessTypeAllowEverything}";

	public const bool DefaultVal_KeyId_IncidentManager_GlobalDefaultAccessTypeAllowEverything = true;

	public const string KeyId_IncidentManager_Incident_ExtendedInformationConfiguration = "{incidentManager-incident-extendedInformationConfiguration}";

	private IDtoMilestoneUser _ActiveUser;

	private ManagerOptions _ManagerOptionInstance;

	public List<DtoGlobalSettings> AllGlobalSettings { get; set; }

	public IDtoMilestoneUser ActiveUser
	{
		get
		{
			return _ActiveUser;
		}
		set
		{
			if (_ActiveUser != value)
			{
				_ActiveUser = value;
				NotifyPropertyChanged("ActiveUser");
				ConfigureGlobalSettingsObject();
			}
		}
	}

	public ManagerOptions ManagerOptionInstance
	{
		get
		{
			return _ManagerOptionInstance;
		}
		set
		{
			if (_ManagerOptionInstance != value)
			{
				_ManagerOptionInstance = value;
				NotifyPropertyChanged("ManagerOptionInstance");
			}
		}
	}

	public event GlobalSettingsConfigurationChangedEventHandler GlobalSettingsConfigurationChanged;

	public event PropertyChangedEventHandler PropertyChanged;

	public void Notify_GlobalSettingsConfigurationChanged()
	{
		this.GlobalSettingsConfigurationChanged?.Invoke();
	}

	public DtoGlobalSettingsManager()
	{
		ConfigureGlobalSettingsObject();
	}

	public DtoGlobalSettingsManager(IDtoMilestoneUser inUser)
		: this()
	{
		SetActiveUser(inUser);
	}

	public void SetAllGlobalSettingsFromDb(List<DtoGlobalSettings> inList)
	{
		AllGlobalSettings = inList;
		ConfigureGlobalSettingsObject();
	}

	public void SetActiveUser(IDtoMilestoneUser inUser)
	{
		ActiveUser = inUser;
	}

	public void ConfigureGlobalSettingsObject()
	{
		IncidentProperties incidentExtendedPropsConfig = new IncidentProperties();
		ManagerOptions managerOptions = new ManagerOptions
		{
			GlobalDefaultAccessTypeAllowEverything = true,
			IncidentExtendedPropsConfig = incidentExtendedPropsConfig
		};
		lock (this)
		{
			if (AllGlobalSettings != null && AllGlobalSettings.Count > 0)
			{
				DtoGlobalSettings dtoGlobalSettings = AllGlobalSettings.FirstOrDefault((DtoGlobalSettings o) => o.GlobalSettingsIndentificator == "{incidentManager-incident-extendedInformationConfiguration}");
				if (dtoGlobalSettings != null)
				{
					IncidentProperties incidentProperties = null;
					incidentProperties = JsonConvert.DeserializeObject<IncidentProperties>(dtoGlobalSettings.ValueJSon);
					incidentProperties?.ReplacePropsWithObjectsFromList();
					if (incidentProperties != null)
					{
						managerOptions.IncidentExtendedPropsConfig = incidentProperties;
					}
				}
			}
		}
		ManagerOptionInstance = managerOptions;
		Notify_GlobalSettingsConfigurationChanged();
	}

	public List<DtoGlobalSettings> GetGlobalSettingsForUpdate(ManagerOptions inChangedSettings, _tmpTypeOfGlobalSettingsUpdate inTypeOfUpdate = _tmpTypeOfGlobalSettingsUpdate.Update)
	{
		if (inChangedSettings != null)
		{
			List<DtoGlobalSettings> list = new List<DtoGlobalSettings>();
			List<DtoGlobalSettings> list2 = new List<DtoGlobalSettings>();
			DtoGlobalSettings dtoGlobalSettings = AllGlobalSettings?.FirstOrDefault((DtoGlobalSettings o) => o.GlobalSettingsIndentificator == "{incidentManager-globalDefaultAccessTypeAllowEverything}");
			if (dtoGlobalSettings != null)
			{
				dtoGlobalSettings.ModifiedUserId = ActiveUser?.MilestoneUserId;
				dtoGlobalSettings.ModifiedUserName = ActiveUser?.UserName;
				list.Add(dtoGlobalSettings);
			}
			else
			{
				DtoGlobalSettings item = new DtoGlobalSettings
				{
					CreatedUserId = ActiveUser?.MilestoneUserId,
					CreatedUserName = ActiveUser?.UserName,
					ModifiedUserId = ActiveUser?.MilestoneUserId,
					ModifiedUserName = ActiveUser?.UserName,
					GlobalSettingsIndentificator = "{incidentManager-globalDefaultAccessTypeAllowEverything}"
				};
				list2.Add(item);
			}
			DtoGlobalSettings dtoGlobalSettings2 = AllGlobalSettings?.FirstOrDefault((DtoGlobalSettings o) => o.GlobalSettingsIndentificator == "{incidentManager-incident-extendedInformationConfiguration}");
			if (dtoGlobalSettings2 != null)
			{
				dtoGlobalSettings2.ValueJSon = JsonConvert.SerializeObject(inChangedSettings.IncidentExtendedPropsConfig);
				dtoGlobalSettings2.ModifiedUserId = ActiveUser?.MilestoneUserId;
				dtoGlobalSettings2.ModifiedUserName = ActiveUser?.UserName;
				list.Add(dtoGlobalSettings2);
			}
			else
			{
				DtoGlobalSettings item2 = new DtoGlobalSettings
				{
					CreatedUserId = ActiveUser?.MilestoneUserId,
					CreatedUserName = ActiveUser?.UserName,
					ModifiedUserId = ActiveUser?.MilestoneUserId,
					ModifiedUserName = ActiveUser?.UserName,
					GlobalSettingsIndentificator = "{incidentManager-incident-extendedInformationConfiguration}",
					ValueJSon = JsonConvert.SerializeObject(inChangedSettings.IncidentExtendedPropsConfig)
				};
				list2.Add(item2);
			}
			if (inTypeOfUpdate == _tmpTypeOfGlobalSettingsUpdate.Update)
			{
				return list;
			}
			return list2;
		}
		return null;
	}

	private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
