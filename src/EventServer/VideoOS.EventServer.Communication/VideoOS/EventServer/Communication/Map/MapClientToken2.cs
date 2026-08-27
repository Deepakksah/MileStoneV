using System;
using System.ServiceModel;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.EventServer.Communication.Map.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Map;

public class MapClientToken2 : MapClient
{
	private readonly ClientPool<MapCommandToken2Client> _clientPool;

	public MapClientToken2(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/MapServiceToken2"))
	{
		_clientPool = new ClientPool<MapCommandToken2Client>(CreateClient, 2);
	}

	private MapCommandToken2Client CreateClient()
	{
		MapCommandToken2Client mapCommandToken2Client = new MapCommandToken2Client(BindingFactory.CreateBasicHttpBindingToken(_uri.IsHttps()), new EndpointAddress(_uri));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(mapCommandToken2Client);
		mapCommandToken2Client.EnsureLargeObjectGraphsAllowed<MapCommandToken2Client, IMapCommandToken2>();
		return mapCommandToken2Client;
	}

	private string GetToken()
	{
		return (LoginSettingsCache.GetLoginSettings(base.ServerId) ?? throw new LoginSettingsNotFoundException(base.ServerId)).Token;
	}

	public override VisualElement CreateObject(VisualElement visualElement)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.CreateObject(GetToken(), visualElement));
	}

	public override VisualElement[] CreateObjects(VisualElement[] visualElements)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.CreateObjects(GetToken(), visualElements));
	}

	public override DateTime UpdateObject(VisualElement visualElement, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.UpdateObject(GetToken(), visualElement, force));
	}

	public override DateTime[] UpdateObjects(VisualElement[] visualElements, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.UpdateObjects(GetToken(), visualElements, force));
	}

	public override DateTime UpdateName(string id, string visualOwnerId, string name, DateTime timestamp, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.UpdateName(GetToken(), id, visualOwnerId, name, timestamp, force));
	}

	public override DateTime UpdateBounds(string id, string visualOwnerId, Bounds bounds, DateTime timestamp, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.UpdateBounds(GetToken(), id, visualOwnerId, bounds, timestamp, force));
	}

	public override DateTime UpdateDisplayXML(string id, string visualOwnerId, string displayXML, DateTime timestamp, bool force)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.UpdateDisplayXML(GetToken(), id, visualOwnerId, displayXML, timestamp, force));
	}

	public override VisualElement GetObject(string id, string visualOwnerId)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetObject(GetToken(), id, visualOwnerId));
	}

	public override TypeForTree[] GetAllObjectsOfType(Guid type, Guid relationType, string id, int maxLevels, bool includeRoot)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetAllObjectsOfType(GetToken(), type, relationType, id, maxLevels, includeRoot));
	}

	public override VisualElement[] GetAllObjectsOnVisualElement(string id)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetAllObjectsOnVisualElement(GetToken(), id));
	}

	public override SimpleVisualElement[] GetAllObjectsContaining(Guid type, string[] ids)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetAllObjectsContaining(GetToken(), type, ids));
	}

	public override VisualElement[] GetObjectsWithinGPSArea(Bounds area)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetObjectsWithinGPSArea(GetToken(), area));
	}

	public override TypeID[] GetAllObjectIDsOnVisualElement(string id)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetAllObjectIDsOnVisualElement(GetToken(), id));
	}

	public override VisualElement[] DeleteObject(VisualElement visualElement)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.DeleteObject(GetToken(), visualElement));
	}

	public override VisualElement[] DeleteObjects(VisualElement[] elementsToDelete)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.DeleteObjects(GetToken(), elementsToDelete));
	}

	public override void DeleteObjectAndChildObjects(string id, string visualOwnerId, bool recursive)
	{
		_clientPool.CallWithRetry(delegate(MapCommandToken2Client client)
		{
			client.DeleteObjectAndChildObjects(GetToken(), id, visualOwnerId, recursive);
		});
	}

	public override MapPlugIn[] GetPlugIns()
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetPlugIns(GetToken()));
	}

	public override MapPlugIn[] GetPluginsExtended(Guid[] kinds, string language)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetPlugInsExtended(GetToken(), kinds, language));
	}

	public override MapPlugInItems GetPlugInItems()
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetPlugInItems(GetToken()));
	}

	public override MapPlugInItems GetPluginItemsExtended(Guid[] kinds, string language)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetPlugInItemsExtended(GetToken(), kinds, language));
	}

	public override MapPlugInItems GetPlugInItemChanges(DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetPlugInItemChanges(GetToken(), timestamp, timeout));
	}

	public override MapPlugInItems GetPluginItemChangesExtended(DateTime timestamp, int timeout, Guid[] kinds, string language)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetPlugInItemChangesExtended(GetToken(), timestamp, timeout, kinds, language));
	}

	public override void ActivatePlugInAction(Guid typeId, string itemId, string actionId)
	{
		_clientPool.CallWithRetry(delegate(MapCommandToken2Client client)
		{
			client.ActivatePlugInAction(GetToken(), typeId, itemId, actionId);
		});
	}

	public override byte[] GetIconData(Guid type, Guid iconId)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetIconData(GetToken(), type, iconId));
	}

	public override TypeChanged GetTypeChanged(Guid[] types, DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetTypeChanged(GetToken(), types, timestamp, timeout));
	}

	public override VisualElementChange[] GetVisualElementChanges(bool includeSubElementChanges, bool ignoreCleanup, string id, DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((MapCommandToken2Client client) => client.GetVisualElementChanges(GetToken(), includeSubElementChanges, ignoreCleanup, id, timestamp, timeout));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
