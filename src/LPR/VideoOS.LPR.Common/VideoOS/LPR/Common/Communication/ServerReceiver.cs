using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Communication;

public class ServerReceiver
{
	private Dictionary<Guid, Item> _servers;

	private object _serverLock = new object();

	public ICollection<Item> Servers()
	{
		lock (_serverLock)
		{
			if (_servers == null)
			{
				_servers = GetServers();
			}
		}
		return _servers.Values.ToList();
	}

	public static ICollection<Tuple<Item, bool>> ChildServersWithLPRServer(Guid parentSiteId)
	{
		if (parentSiteId.Equals(Guid.Empty))
		{
			return new List<Tuple<Item, bool>> { GetMasterWithLPRServers() };
		}
		return GetChildsWithLPRServers(parentSiteId);
	}

	public static ICollection<Item> ServersWithLPRServer()
	{
		return GetLPRServersOnly(EnvironmentManager.Instance.CurrentSite.ServerId.Id);
	}

	public static Tuple<Item, bool> GetSiteParent(Guid siteId)
	{
		Item siteItem = EnvironmentManager.Instance.GetSiteItem(siteId);
		if (siteItem != null)
		{
			Item parent = siteItem.GetParent();
			if (parent != null)
			{
				if (HasLPRServer(parent))
				{
					return new Tuple<Item, bool>(parent, item2: true);
				}
				return new Tuple<Item, bool>(parent, item2: false);
			}
		}
		return null;
	}

	public static string LPRServerAddress(Guid siteId)
	{
		Item siteItem = EnvironmentManager.Instance.GetSiteItem(siteId);
		if (siteItem != null)
		{
			try
			{
				List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_LPRServerService, siteItem.FQID.ServerId);
				if (registeredServiceUriInfo.Count > 0 && registeredServiceUriInfo[0].UriArray.Count > 0)
				{
					return registeredServiceUriInfo[0].UriArray[0];
				}
			}
			catch (CommunicationMIPException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServiceReceiver", string.Format(CultureInfo.InvariantCulture, "Failed to get registered LPR service {0}. Exception: {1}", siteItem.FQID.ServerId, ex.Message));
			}
		}
		return "";
	}

	public static string EventServerAddress(Guid siteId)
	{
		Item siteItem = EnvironmentManager.Instance.GetSiteItem(siteId);
		if (siteItem != null)
		{
			try
			{
				List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, siteItem.FQID.ServerId);
				if (registeredServiceUriInfo.Count > 0 && registeredServiceUriInfo[0].UriArray.Count > 0)
				{
					return registeredServiceUriInfo[0].UriArray[0];
				}
			}
			catch (CommunicationMIPException ex)
			{
				EnvironmentManager.Instance.Log(error: true, "LPR.ServiceReceiver", string.Format(CultureInfo.InvariantCulture, "Failed to get registered event server service {0}. Exception: {1}", siteItem.FQID.ServerId, ex.Message));
			}
		}
		return "";
	}

	private static Dictionary<Guid, Item> GetServers()
	{
		Dictionary<Guid, Item> dictionary = new Dictionary<Guid, Item>();
		GetServersRecursive(EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.CurrentSite), dictionary);
		return dictionary;
	}

	private static Tuple<Item, bool> GetMasterWithLPRServers()
	{
		Dictionary<Guid, Tuple<Item, bool>> dictionary = new Dictionary<Guid, Tuple<Item, bool>>();
		Item siteItem = EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.CurrentSite.ServerId.Id);
		bool lprServer = false;
		GetServersRecursiveWithLPRServers(siteItem, dictionary, ref lprServer);
		Tuple<Item, bool> result = new Tuple<Item, bool>(null, item2: false);
		if (dictionary.Count > 0)
		{
			result = new Tuple<Item, bool>(siteItem, item2: true);
		}
		return result;
	}

	private static Dictionary<Guid, Item>.ValueCollection GetLPRServersOnly(Guid siteId)
	{
		Dictionary<Guid, Item> dictionary = new Dictionary<Guid, Item>();
		GetLPRServersOnlyRecursive(EnvironmentManager.Instance.GetSiteItem(siteId), dictionary);
		return dictionary.Values;
	}

	private static List<Tuple<Item, bool>> GetChildsWithLPRServers(Guid siteId)
	{
		Dictionary<Guid, Tuple<Item, bool>> dictionary = new Dictionary<Guid, Tuple<Item, bool>>();
		Item siteItem = EnvironmentManager.Instance.GetSiteItem(siteId);
		bool lprServer = false;
		GetServersRecursiveWithLPRServers(siteItem, dictionary, ref lprServer);
		List<Item> children = siteItem.GetChildren();
		List<Tuple<Item, bool>> list = new List<Tuple<Item, bool>>();
		foreach (Item item in children)
		{
			if (dictionary.TryGetValue(item.FQID.ServerId.Id, out var value))
			{
				list.Add(value);
			}
		}
		return list;
	}

	private static bool HasLPRServer(Item server)
	{
		try
		{
			List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_LPRServerService, server.FQID.ServerId);
			return registeredServiceUriInfo.Count > 0 && registeredServiceUriInfo[0].UriArray.Count > 0;
		}
		catch (CommunicationMIPException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "LPR.ServiceReceiver", string.Format(CultureInfo.InvariantCulture, "Failed to get registered LPR server service {0}. Exception: {1}", server.FQID.ServerId, ex.Message));
		}
		return false;
	}

	private static void GetServersRecursiveWithLPRServers(Item server, Dictionary<Guid, Tuple<Item, bool>> servers, ref bool lprServer)
	{
		if (server == null)
		{
			return;
		}
		foreach (Item child in server.GetChildren())
		{
			GetServersRecursiveWithLPRServers(child, servers, ref lprServer);
		}
		if (server.FQID != null && server.FQID.ServerId != null && server.FQID.Kind == Kind.Server && server.FQID.ServerId.ServerType != null && !servers.ContainsKey(server.FQID.ServerId.Id))
		{
			bool flag = HasLPRServer(server);
			if (flag | lprServer)
			{
				lprServer = true;
				servers.Add(server.FQID.ServerId.Id, new Tuple<Item, bool>(server, flag));
			}
		}
	}

	private static void GetLPRServersOnlyRecursive(Item server, Dictionary<Guid, Item> servers)
	{
		if (server == null)
		{
			return;
		}
		foreach (Item child in server.GetChildren())
		{
			GetLPRServersOnlyRecursive(child, servers);
		}
		if (server.FQID != null && server.FQID.ServerId != null && server.FQID.Kind == Kind.Server && server.FQID.ServerId.ServerType != null && !servers.ContainsKey(server.FQID.ServerId.Id) && HasLPRServer(server))
		{
			servers.Add(server.FQID.ServerId.Id, server);
		}
	}

	private static void GetServersRecursive(Item server, Dictionary<Guid, Item> servers)
	{
		if (server == null)
		{
			return;
		}
		if (server.FQID != null && server.FQID.ServerId != null && server.FQID.Kind == Kind.Server && server.FQID.ServerId.ServerType != null && !servers.ContainsKey(server.FQID.ServerId.Id))
		{
			servers.Add(server.FQID.ServerId.Id, server);
		}
		foreach (Item child in server.GetChildren())
		{
			GetServersRecursive(child, servers);
		}
	}
}
