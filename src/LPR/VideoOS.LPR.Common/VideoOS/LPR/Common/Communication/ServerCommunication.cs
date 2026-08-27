using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.AlarmClient;
using VideoOS.LPR.Common.Communication.Info;
using VideoOS.LPR.Common.Communication.Sites;
using VideoOS.LPR.Common.Communication.VideoSources;
using VideoOS.LPR.Common.Utils;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Communication;

public class ServerCommunication
{
	private enum LoadingState
	{
		Success,
		Pending,
		Faulted
	}

	private readonly Dictionary<Guid, ServerController> _serverControllers = new Dictionary<Guid, ServerController>();

	private readonly ConcurrentDictionary<Guid, LoadingState> _listStates = new ConcurrentDictionary<Guid, LoadingState>();

	private readonly ConcurrentDictionary<Guid, LoadingState> _countryCodeStates = new ConcurrentDictionary<Guid, LoadingState>();

	private readonly ConcurrentDictionary<Guid, LoadingState> _videoSourceStates = new ConcurrentDictionary<Guid, LoadingState>();

	private readonly Dictionary<Guid, string> _serverNames = new Dictionary<Guid, string>();

	public int ServerCount => _serverControllers.Count;

	public bool IsLicensePlateListsComplete => _listStates.Values.All((LoadingState v) => v != LoadingState.Pending);

	public bool IsAllCompleted
	{
		get
		{
			if (_listStates.Values.All((LoadingState v) => v != LoadingState.Pending) && _countryCodeStates.Values.All((LoadingState v) => v != LoadingState.Pending))
			{
				return _videoSourceStates.Values.All((LoadingState v) => v != LoadingState.Pending);
			}
			return false;
		}
	}

	public IEnumerable<Guid> FaultedServerIds => (from i in _listStates.Where((KeyValuePair<Guid, LoadingState> l) => l.Value == LoadingState.Faulted).Concat(_countryCodeStates.Where((KeyValuePair<Guid, LoadingState> cc) => cc.Value == LoadingState.Faulted)).Concat(_videoSourceStates.Where((KeyValuePair<Guid, LoadingState> vs) => vs.Value == LoadingState.Faulted))
		select i.Key).Distinct().ToList();

	public bool IsVideoSourcesSiteComplete(Guid siteId)
	{
		if (_videoSourceStates.TryGetValue(siteId, out var value))
		{
			return value != LoadingState.Pending;
		}
		return true;
	}

	public string GetServerName(Guid serverId)
	{
		if (!_serverNames.TryGetValue(serverId, out var value))
		{
			return string.Empty;
		}
		return value;
	}

	public void Init()
	{
		foreach (Item item in ServerReceiver.ServersWithLPRServer())
		{
			string text = ServerReceiver.EventServerAddress(item.FQID.ServerId.Id);
			if (!string.IsNullOrWhiteSpace(text))
			{
				_serverNames.Add(item.FQID.ServerId.Id, item.Name);
				_listStates.TryAdd(item.FQID.ServerId.Id, LoadingState.Pending);
				_countryCodeStates.TryAdd(item.FQID.ServerId.Id, LoadingState.Pending);
				_videoSourceStates.TryAdd(item.FQID.ServerId.Id, LoadingState.Pending);
				ServerController serverController = new ServerController(item.FQID.ServerId, new Uri(text));
				serverController.SearchListsResultEvent += controller_SearchListsResultEvent;
				_serverControllers.Add(item.FQID.ServerId.Id, serverController);
			}
		}
		GetSearchLists();
	}

	public void Close()
	{
		foreach (ServerController value in _serverControllers.Values)
		{
			value.SearchListsResultEvent -= controller_SearchListsResultEvent;
			value.Close();
		}
		_serverControllers.Clear();
	}

	private void GetSearchLists()
	{
		foreach (ServerController value in _serverControllers.Values)
		{
			value.GetSearchLists();
		}
	}

	public static GetSitesResponse GetSites(GetSitesParams siteParameters)
	{
		List<SiteInfo> list = new List<SiteInfo>();
		if (siteParameters != null)
		{
			foreach (Tuple<Item, bool> item in ServerReceiver.ChildServersWithLPRServer(siteParameters.SiteId))
			{
				if (item?.Item1?.FQID?.ServerId != null)
				{
					list.Add(new SiteInfo
					{
						Id = item.Item1.FQID.ServerId.Id,
						Name = item.Item1.Name,
						IsLPRSite = item.Item2
					});
				}
			}
		}
		return new GetSitesResponse
		{
			SiteInfos = list
		};
	}

	public static GetAllSitesResponse GetAllSites()
	{
		ICollection<Item> collection = ServerReceiver.ServersWithLPRServer();
		List<SiteInfo> list = new List<SiteInfo>();
		foreach (Item item in collection)
		{
			if (item != null && item.FQID != null && item.FQID.ServerId != null)
			{
				list.Add(new SiteInfo
				{
					Id = item.FQID.ServerId.Id,
					Name = item.Name,
					IsLPRSite = true
				});
			}
		}
		return new GetAllSitesResponse
		{
			SiteInfos = list
		};
	}

	public static GetSitesResponse GetParentSite(GetSitesParams siteParameters)
	{
		List<SiteInfo> list = new List<SiteInfo>();
		if (siteParameters != null)
		{
			Tuple<Item, bool> siteParent = ServerReceiver.GetSiteParent(siteParameters.SiteId);
			if (siteParent?.Item1?.FQID?.ServerId != null)
			{
				list.Add(new SiteInfo
				{
					Id = siteParent.Item1.FQID.ServerId.Id,
					Name = siteParent.Item1.Name,
					IsLPRSite = siteParent.Item2
				});
			}
		}
		return new GetSitesResponse
		{
			SiteInfos = list
		};
	}

	public async Task<int> AddLicensePlatesAsync(Guid siteId, Guid listId, string listName, IEnumerable<LicensePlateInfo> licensePlateInfos)
	{
		if (_serverControllers.TryGetValue(siteId, out var value))
		{
			return await value.AddLicensePlatesAsync(listId, listName, licensePlateInfos).ConfigureAwait(continueOnCapturedContext: false);
		}
		EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed to add plate from LPR list: {0} on site {1}. No serverController found ", listName, siteId));
		return 0;
	}

	public async Task<int> DeleteLicensePlatesAsync(Guid siteId, Guid listId, string listName, IEnumerable<LicensePlate> plates)
	{
		if (_serverControllers.TryGetValue(siteId, out var value))
		{
			return await value.DeleteLicensePlatesAsync(listId, listName, plates).ConfigureAwait(continueOnCapturedContext: false);
		}
		EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed to remove plate from LPR list: {0} on site {1}. No serverController found ", listName, siteId));
		return 0;
	}

	public async Task<bool> ClearLicensePlatesAsync(Guid siteId, Guid listId, string listName)
	{
		if (_serverControllers.TryGetValue(siteId, out var value))
		{
			return await value.ClearLicensePlatesAsync(listId, listName).ConfigureAwait(continueOnCapturedContext: false);
		}
		EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed to clear plates from LPR list: {0} on site {1}. No serverController found ", listName, siteId));
		return false;
	}

	public async Task<int> GetLicensePlateCountAsync(Guid siteId, Guid listId, string listName, string searchFilter)
	{
		if (_serverControllers.TryGetValue(siteId, out var value))
		{
			return await value.GetLicensePlateCountAsync(listId, listName, searchFilter).ConfigureAwait(continueOnCapturedContext: false);
		}
		EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed to get license plate count for LPR list: {0} on site {1}. No serverController found ", listName, siteId));
		return -1;
	}

	public async Task<LicensePlateInfo[]> SearchListAsync(Guid siteId, Guid listId, string listName, string searchFilter, int startIndex, int count)
	{
		if (_serverControllers.TryGetValue(siteId, out var value))
		{
			return await value.SearchListAsync(listId, listName, searchFilter, startIndex, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed to search LPR list: {0} on site {1}. No serverController found ", listName, siteId));
		return null;
	}

	public GetVideoSourcesResponse GetVideoSources(VideoOS.LPR.Common.Communication.VideoSources.GetVideoSourcesParams videoSourceParameters)
	{
		if (videoSourceParameters != null)
		{
			if (_serverControllers.TryGetValue(videoSourceParameters.SiteId, out var value))
			{
				return value.GetVideoSources();
			}
			EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed get LPR camera on site {0}. No serverController found ", videoSourceParameters.SiteId));
		}
		return null;
	}

	public async Task<int> GetLicensePlateEventCountAsync(VideoOS.EventServer.Communication.AlarmClient.SearchGroup searchGroup, CancellationToken cancellationToken)
	{
		List<Task<int>> tasks = new List<Task<int>>();
		foreach (ServerController value in _serverControllers.Values)
		{
			tasks.Add(value.GetLicensePlateEventCountAsync(searchGroup));
		}
		if (tasks.Count != 0)
		{
			try
			{
				await TaskExtended.WhenAll(tasks, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (OperationCanceledException)
			{
				return -1;
			}
			catch
			{
				foreach (Task<int> item in tasks)
				{
					if (item.IsFaulted && !(item.Exception.InnerException is ActionNotSupportedException))
					{
						throw item.Exception;
					}
				}
				return -1;
			}
		}
		return tasks.Sum((Task<int> x) => x.Result);
	}

	public async Task<List<KeyValuePair<string, string>>> LookupLicensePlatesAsync(Guid siteId, string registrationNumber, MatchListItemType matchListItemType, Guid listId)
	{
		if (_serverControllers.TryGetValue(siteId, out var value))
		{
			return await value.LookupLicensePlatesAsync(registrationNumber, matchListItemType, listId).ConfigureAwait(continueOnCapturedContext: false);
		}
		EnvironmentManager.Instance.Log(error: true, "VideoOS.LPR.PluginClient.Communication.Server.Communication", string.Format(CultureInfo.InvariantCulture, "Failed to get license list properties from: {0}:{1} on site {2}. No serverController found", registrationNumber, listId, siteId));
		return null;
	}

	public IList<Task<GetLicensePlateEventsResult>> GetLicensePlateEvents(SearchInfo searchInfo, ReadOnlyCollection<Guid> ignoreSiteIds)
	{
		List<Task<GetLicensePlateEventsResult>> list = new List<Task<GetLicensePlateEventsResult>>();
		foreach (ServerController value in _serverControllers.Values)
		{
			if (ignoreSiteIds == null || !ignoreSiteIds.Contains(value.SiteId))
			{
				list.Add(value.GetLicensePlateEventsAsync(searchInfo));
			}
		}
		return list;
	}

	public IEnumerable<Task<SearchResult>> SearchLPREventsAsync(SearchInfo searchInfo, ReadOnlyCollection<Guid> ignoreSiteIds)
	{
		List<Task<SearchResult>> list = new List<Task<SearchResult>>();
		foreach (ServerController value in _serverControllers.Values)
		{
			if (ignoreSiteIds == null || !ignoreSiteIds.Contains(value.SiteId))
			{
				list.Add(value.SearchLPREvents(searchInfo));
			}
		}
		return list;
	}

	private void controller_SearchListsResultEvent(object sender, EventArgs args)
	{
		if (args is SearchListsResultEventArgs<GetLicensePlateListsResp> e)
		{
			_listStates[e.SiteId] = (e.IsFaulted ? LoadingState.Faulted : LoadingState.Success);
			ConfigurationSettingsCache.Instance.UpdateLicensePlateLists(e.Result?.LicensePlateLists, e.SiteId);
		}
		else if (args is SearchListsResultEventArgs<GetAllCountryCodesResp> e2)
		{
			_countryCodeStates[e2.SiteId] = (e2.IsFaulted ? LoadingState.Faulted : LoadingState.Success);
			ConfigurationSettingsCache.Instance.UpdateCountryCodes(e2.Result?.CountryCodes);
		}
		else if (args is SearchListsResultEventArgs<GetVideoSourcesResponse> e3)
		{
			_videoSourceStates[e3.SiteId] = (e3.IsFaulted ? LoadingState.Faulted : LoadingState.Success);
			ConfigurationSettingsCache.Instance.UpdateVideoSources(e3.Result?.VideoSourceInfos);
		}
		else if (args is SearchListsResultEventArgs<GetCountryModulesWithLicensePlateStylesResp> e4)
		{
			_videoSourceStates[e4.SiteId] = (e4.IsFaulted ? LoadingState.Faulted : LoadingState.Success);
			ConfigurationSettingsCache.Instance.UpdateLicensePlateStyles(e4.Result?.CountryModuleWithLicensePlateStyles.ToList());
		}
	}
}
