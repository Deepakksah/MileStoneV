using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config;

public class DBLoginServer : LoginServerBase, ILoginServer
{
	private UserContext _userContext;

	public string RootPath { get; set; }

	public override ILoginInfo LoginInfo
	{
		get
		{
			return new LoginInfo
			{
				Token = "#a-login_token",
				RegistrationTime = DateTime.UtcNow,
				TimeToLive = new TimeDuration
				{
					MicroSeconds = 3600000000L
				},
				TimeToLiveLimited = false
			};
		}
		set
		{
		}
	}

	public override ServerTypes ServerType
	{
		get
		{
			return ServerTypes.DATABASE;
		}
		set
		{
		}
	}

	public override UserContext UserContext
	{
		get
		{
			return _userContext;
		}
		set
		{
			_userContext = value;
		}
	}

	public IConfigurationInfo CorporateSystemInfo
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ISystemLicenseInfo GetSystemLicenseInfo()
	{
		return null;
	}

	public override bool IsServerAvailable(Uri uri)
	{
		return true;
	}

	public override bool Login(Uri uri)
	{
		return true;
	}

	public override void GetGuidAndName()
	{
	}

	public override bool IsLoggedIn()
	{
		return true;
	}

	public override void Logout()
	{
	}

	public bool GetSystemInfoAndLogin()
	{
		return true;
	}

	public IChange[] QueryChanges(DateTime changesFrom)
	{
		return Array.Empty<IChange>();
	}

	public IRecorderInfo QueryRecorderInfo(Guid recorderId)
	{
		throw new NotImplementedException();
	}

	public IRecorderInfo[] GetRecorderInfos(Guid[] recorderIds)
	{
		throw new NotImplementedException();
	}

	public IHardwareInfo[] GetHardwareInfos(Guid[] hardwareIds)
	{
		throw new NotImplementedException();
	}

	public IAlertOccurrence[] GetAlerts(Guid guid, DateTime time, ITimeDuration timeBefore, int maxCountBefore, ITimeDuration timeAfter, int maxCountAfter)
	{
		return Array.Empty<IAlertOccurrence>();
	}

	public void EventTriggerWithMetadata(Guid eventGuid, IKeyValue[] metadata)
	{
	}

	public void MatrixMonitorActivateCamera(Guid matrixGuid, Guid cameraGuid)
	{
	}

	public void EventTrigger(Guid eventGuid)
	{
	}

	public ICustomSetting GetCustomSettingDataGlobal(Guid platformDefinitionId)
	{
		string directoryName = Path.GetDirectoryName(RootPath);
		string text = (RootPath.ToUpper().EndsWith("SCP") ? (directoryName + "\\Data\\CustomSettings") : ((!RootPath.ToUpper().EndsWith("INI")) ? (directoryName + "\\..\\..\\CustomSettings") : (directoryName + "\\CustomSettings")));
		if (Directory.Exists(text))
		{
			string path = Path.Combine(text, platformDefinitionId.ToString() + ".xml");
			if (File.Exists(path))
			{
				string customSettingDataXml = File.ReadAllText(path);
				CustomSettingInternalData customSettingData = new CustomSettingInternalData
				{
					CustomSettingDataXml = customSettingDataXml,
					DataVersion = 1
				};
				CustomSettingInfo customSettingInfo = new CustomSettingInfo
				{
					Id = platformDefinitionId,
					LastModifiedTime = File.GetCreationTime(path),
					LastModifiedUser = "Unknown"
				};
				return new CustomSettingInternal
				{
					CustomSettingData = customSettingData,
					CustomSettingInfo = customSettingInfo
				};
			}
		}
		return null;
	}

	public ICustomSetting GetCustomSettingDataUser(Guid platformDefinitionId)
	{
		return null;
	}

	public Item GetItemConfiguration(Guid platformDefinitionId, Guid itemKind, Guid itemId)
	{
		try
		{
			ICustomSetting customSettingDataGlobal = GetCustomSettingDataGlobal(platformDefinitionId);
			if (customSettingDataGlobal == null)
			{
				return null;
			}
			XmlNode customSettingDataXml = customSettingDataGlobal.CustomSettingData.CustomSettingDataXml;
			if (customSettingDataXml != null)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(customSettingDataXml.OuterXml);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("/PlatformConfiguration");
				if (xmlNode == null)
				{
					return null;
				}
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("Configurations/GUID" + itemKind.ToString() + "[@itemID=\"" + itemId.ToString() + "\"]");
				if (xmlNode2 == null)
				{
					return null;
				}
				Item item = ConfigurationUtil.DeserializeItem(xmlNode2);
				item.FQID.ServerId.UserContext = UserContext;
				return item;
			}
			return null;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public IBookmarkReference BookmarkGetNewReference(Guid deviceId, bool live)
	{
		throw new NotSupportedException();
	}

	public Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		throw new NotSupportedException();
	}

	public Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		throw new NotSupportedException();
	}

	public Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		throw new NotSupportedException();
	}

	public Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		throw new NotSupportedException();
	}

	public void BookmarkDelete(Guid bookmarkId)
	{
		throw new NotSupportedException();
	}

	public IBookmark BookmarkCreate(Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		throw new NotSupportedException();
	}

	public IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		throw new NotSupportedException();
	}

	public IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		throw new NotSupportedException();
	}

	public IBookmark BookmarkGet(Guid bookmarkId)
	{
		throw new NotSupportedException();
	}

	public IBookmark BookmarkUpdate(Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		throw new NotSupportedException();
	}

	public IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		throw new NotSupportedException();
	}

	public IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		throw new NotSupportedException();
	}

	public IRestrictedMediaResult RestrictedMediaCreate(Guid restrictedMediaId, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId)
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaResult RestrictedMediaLiveModeExit(Guid restrictedMediaId, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaLive[] RestrictedMediaLiveQueryAll()
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		throw new NotImplementedException();
	}

	public IRestrictedMedia[] RestrictedMediaQueryAll()
	{
		throw new NotImplementedException();
	}

	public IRestrictedMediaResult RestrictedMediaUpdate(Guid restrictedMediaId, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		throw new NotImplementedException();
	}

	public IViewGroup GetViewGroupData(Guid groupId)
	{
		return null;
	}

	public IViewGroupInfo[] GetViewGroups()
	{
		return Array.Empty<IViewGroupInfo>();
	}

	public IPresetInfo[] GetPresets(Guid deviceId)
	{
		throw new NotSupportedException();
	}

	public void DeletePreset(Guid id, Guid deviceId)
	{
		throw new NotSupportedException();
	}

	public void UpdatePresetName(Guid id, Guid deviceId, string newName)
	{
		throw new NotSupportedException();
	}

	public void UpdatePresetPosition(Guid id, Guid deviceId)
	{
		throw new NotSupportedException();
	}

	public Guid CreatePreset(Guid deviceId, string name)
	{
		throw new NotSupportedException();
	}

	public void OrderPresets(Guid deviceId, Guid[] ids)
	{
		throw new NotSupportedException();
	}

	public override SequenceCollection GetSequences(Guid deviceId, DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		throw new NotSupportedException("Should be support via the SDK.Export dll");
	}

	public override void PTZMove(Guid deviceGuid, string direction)
	{
		throw new NotSupportedException();
	}

	public IDeviceDisabledInfo[] GetDisabledDevices()
	{
		throw new NotSupportedException();
	}
}
