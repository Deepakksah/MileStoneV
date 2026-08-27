using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.EventServer.Communication.Map.Client;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Map;

public class MapClientCredentials : MapClient
{
	private readonly ClientPool<MapCommandCredentialsClient> _clientPool;

	public MapClientCredentials(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/MapServiceCredentials"))
	{
		_clientPool = new ClientPool<MapCommandCredentialsClient>(CreateClient, 2);
	}

	private MapCommandCredentialsClient CreateClient()
	{
		MapCommandCredentialsClient mapCommandCredentialsClient = new MapCommandCredentialsClient(BindingFactory.CreateWSHttpBindingCredentials(_uri.IsHttps()), new EndpointAddress(identity: AbstractClient.CreateSpnIdentity(SpnFactory.GetSpn(_uri)), uri: _uri, addressHeaders: Array.Empty<AddressHeader>()));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		mapCommandCredentialsClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(mapCommandCredentialsClient);
		mapCommandCredentialsClient.EnsureLargeObjectGraphsAllowed<MapCommandCredentialsClient, IMapCommandCredentials>();
		return mapCommandCredentialsClient;
	}

	public override VisualElement CreateObject(VisualElement visualElement)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.CreateObject(visualElement));
	}

	public override VisualElement[] CreateObjects(VisualElement[] visualElements)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.CreateObjects(visualElements));
	}

	public override DateTime UpdateObject(VisualElement visualElement, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.UpdateObject(visualElement, force));
	}

	public override DateTime[] UpdateObjects(VisualElement[] visualElements, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.UpdateObjects(visualElements, force));
	}

	public override DateTime UpdateName(string id, string visualOwnerId, string name, DateTime timestamp, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.UpdateName(id, visualOwnerId, name, timestamp, force));
	}

	public override DateTime UpdateBounds(string id, string visualOwnerId, Bounds bounds, DateTime timestamp, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.UpdateBounds(id, visualOwnerId, bounds, timestamp, force));
	}

	public override DateTime UpdateDisplayXML(string id, string visualOwnerId, string displayXML, DateTime timestamp, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.UpdateDisplayXML(id, visualOwnerId, displayXML, timestamp, force));
	}

	public override VisualElement GetObject(string id, string visualOwnerId)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetObject(id, visualOwnerId));
	}

	public override TypeForTree[] GetAllObjectsOfType(Guid type, Guid relationType, string id, int maxLevels, bool includeRoot)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetAllObjectsOfType(type, relationType, id, maxLevels, includeRoot));
	}

	public override VisualElement[] GetAllObjectsOnVisualElement(string id)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetAllObjectsOnVisualElement(id));
	}

	public override SimpleVisualElement[] GetAllObjectsContaining(Guid type, string[] ids)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetAllObjectsContaining(type, ids));
	}

	public override VisualElement[] GetObjectsWithinGPSArea(Bounds area)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetObjectsWithinGPSArea(area));
	}

	public override TypeID[] GetAllObjectIDsOnVisualElement(string id)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetAllObjectIDsOnVisualElement(id));
	}

	public override VisualElement[] DeleteObject(VisualElement visualElement)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.DeleteObject(visualElement));
	}

	public override VisualElement[] DeleteObjects(VisualElement[] elementsToDelete)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.DeleteObjects(elementsToDelete));
	}

	public override void DeleteObjectAndChildObjects(string id, string visualOwnerId, bool recursive)
	{
		_clientPool.CallWithRetry(delegate(MapCommandCredentialsClient client)
		{
			client.DeleteObjectAndChildObjects(id, visualOwnerId, recursive);
		});
	}

	public override MapPlugIn[] GetPlugIns()
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetPlugIns());
	}

	public override MapPlugIn[] GetPluginsExtended(Guid[] kinds, string language)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetPlugInsExtended(kinds, language));
	}

	public override MapPlugInItems GetPlugInItems()
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetPlugInItems());
	}

	public override MapPlugInItems GetPluginItemsExtended(Guid[] kinds, string language)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetPlugInItemsExtended(kinds, language));
	}

	public override MapPlugInItems GetPlugInItemChanges(DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetPlugInItemChanges(timestamp, timeout));
	}

	public override MapPlugInItems GetPluginItemChangesExtended(DateTime timestamp, int timeout, Guid[] kinds, string language)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetPlugInItemChangesExtended(timestamp, timeout, kinds, language));
	}

	public override void ActivatePlugInAction(Guid typeId, string itemId, string actionId)
	{
		_clientPool.CallWithRetry(delegate(MapCommandCredentialsClient client)
		{
			client.ActivatePlugInAction(typeId, itemId, actionId);
		});
	}

	public override byte[] GetIconData(Guid type, Guid iconId)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetIconData(type, iconId));
	}

	public override TypeChanged GetTypeChanged(Guid[] types, DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetTypeChanged(types, timestamp, timeout));
	}

	public override VisualElementChange[] GetVisualElementChanges(bool includeSubElementChanges, bool ignoreCleanup, string id, DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((MapCommandCredentialsClient client) => client.GetVisualElementChanges(includeSubElementChanges, ignoreCleanup, id, timestamp, timeout));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
