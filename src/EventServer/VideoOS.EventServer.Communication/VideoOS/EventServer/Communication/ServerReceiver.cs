using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication;

public class ServerReceiver
{
	private readonly ConcurrentDictionary<Version, IEnumerable<Item>> _serversWithEventServer = new ConcurrentDictionary<Version, IEnumerable<Item>>();

	public IEnumerable<Item> ServersWithEventServer(int minimumMajorValue)
	{
		return ServersWithEventServer(new Version(Math.Max(minimumMajorValue, 0), 0, 0, 0));
	}

	public IEnumerable<Item> ServersWithEventServer(Version version)
	{
		if (!_serversWithEventServer.TryGetValue(version, out var value))
		{
			value = GetServersWithEventServers(version);
			_serversWithEventServer.AddOrUpdate(version, value, (Version v, IEnumerable<Item> newResult) => newResult);
		}
		return value;
	}

	public static int? GetEventServerVersion(ServerId serverId)
	{
		VideoOS.Platform.Configuration.ServiceURIInfo? eventServerService = GetEventServerService(serverId);
		if (!eventServerService.HasValue)
		{
			return null;
		}
		return GetEventServerVersion(eventServerService.Value);
	}

	public static int GetEventServerVersion(VideoOS.Platform.Configuration.ServiceURIInfo eventServerURIInfo)
	{
		if (!string.IsNullOrEmpty(eventServerURIInfo.ServiceData))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(eventServerURIInfo.ServiceData);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("EventServer/Version");
			if (xmlNode != null && xmlNode.HasChildNodes && xmlNode.InnerText != string.Empty)
			{
				Match match = Regex.Match(xmlNode.InnerText, "^(?<major>\\d+)\\.(?<minor>\\d+)(?<revision>[a-z]?)$");
				if (match.Success)
				{
					return int.Parse(match.Groups["major"].Value);
				}
			}
		}
		return 1;
	}

	public static Version GetEventServerVersion2(ServerId serverId)
	{
		VideoOS.Platform.Configuration.ServiceURIInfo? eventServerService = GetEventServerService(serverId);
		if (!eventServerService.HasValue)
		{
			return null;
		}
		return GetEventServerVersion2(eventServerService.Value);
	}

	public static Version GetEventServerVersion2(VideoOS.Platform.Configuration.ServiceURIInfo eventServerURIInfo)
	{
		if (!string.IsNullOrEmpty(eventServerURIInfo.ServiceData))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(eventServerURIInfo.ServiceData);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("EventServer/Version");
			if (xmlNode != null && xmlNode.HasChildNodes && xmlNode.InnerText != string.Empty)
			{
				Match match = Regex.Match(xmlNode.InnerText, "^(?<major>\\d+)\\.(?<minor>\\d+)(?<revision>[a-z]?)$");
				if (match.Success)
				{
					int minor = 0;
					int revision = 0;
					try
					{
						minor = int.Parse(match.Groups["minor"].Value);
					}
					catch (Exception)
					{
					}
					try
					{
						revision = Math.Max(match.Groups["revision"].Value[0] - 97, 0);
					}
					catch (Exception)
					{
					}
					return new Version(int.Parse(match.Groups["major"].Value), minor, 0, revision);
				}
			}
		}
		return null;
	}

	private static IEnumerable<Item> GetServersWithEventServers(Version requiredMinimumVersion)
	{
		ConcurrentBag<Item> result = new ConcurrentBag<Item>();
		Parallel.ForEach(GetServers().Values.Where((Item s) => s.FQID?.ServerId != null), delegate(Item server)
		{
			VideoOS.Platform.Configuration.ServiceURIInfo? eventServerService = GetEventServerService(server.FQID.ServerId);
			if (eventServerService.HasValue && GetEventServerVersion2(eventServerService.Value) >= requiredMinimumVersion)
			{
				result.Add(server);
			}
		});
		return result.Distinct();
	}

	private static VideoOS.Platform.Configuration.ServiceURIInfo? GetEventServerService(ServerId serverId)
	{
		try
		{
			List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = VideoOS.Platform.Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
			if (registeredServiceUriInfo.Count != 0 && registeredServiceUriInfo[0].UriArray.Count != 0)
			{
				return registeredServiceUriInfo.First();
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log("ServerReceiver", error: true, "GetServersWithEventServers", "GetRegisteredServiceUriInfo : " + ex.Message, null);
		}
		return null;
	}

	private static Dictionary<Guid, Item> GetServers()
	{
		Dictionary<Guid, Item> dictionary = new Dictionary<Guid, Item>();
		GetServersRecursive(EnvironmentManager.Instance.GetSiteItem(EnvironmentManager.Instance.MasterSite), dictionary);
		return dictionary;
	}

	private static void GetServersRecursive(Item server, Dictionary<Guid, Item> servers)
	{
		if (server != null)
		{
			if (server.FQID != null && server.FQID.Kind == Kind.Server && server.FQID.ServerId != null && server.FQID.ServerId.ServerType != null && !servers.ContainsKey(server.FQID.ServerId.Id))
			{
				servers.Add(server.FQID.ServerId.Id, server);
			}
			server.GetChildren().ForEach(delegate(Item child)
			{
				GetServersRecursive(child, servers);
			});
		}
	}
}
