using System;
using System.Collections.Generic;
using VideoOS.EventServer.Communication.Map.Client;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Map;

public class MapClientManager
{
	public delegate void FaultExceptionHandler(Exception error, int requestId);

	public delegate void CreateObjectHandler(int requestId, VisualElement visualElement);

	public delegate void CreateObjectsHandler(int requestId, VisualElement[] visualElements);

	public delegate void UpdateObjectHandler(int requestId, DateTime time);

	public delegate void UpdateObjectsHandler(int requestId, DateTime[] times);

	public delegate void UpdateNameHandler(int requestId, DateTime time);

	public delegate void UpdateBoundsHandler(int requestId, DateTime time);

	public delegate void UpdateDisplayXMLHandler(int requestId, DateTime time);

	public delegate void GetObjectHandler(int requestId, VisualElement visualElement);

	public delegate void GetAllObjectsOfTypeHandler(int requestId, TypeForTree[] typeForTreeTypes);

	public delegate void GetAllObjectsOnVisualElementHandler(int requestId, VisualElement[] visualElements);

	public delegate void GetAllObjectsContainingHandler(int requestId, SimpleVisualElement[] simpleVisualElements);

	public delegate void GetObjectsWithinGPSAreaHandler(int requestId, VisualElement[] visualElements);

	public delegate void GetAllObjectIDsOnVisualElementHandler(int requestId, TypeID[] typeIds);

	public delegate void DeleteObjectHandler(int requestId, VisualElement[] visualElements);

	public delegate void DeleteObjectsHandler(int requestId, VisualElement[] visualElements);

	public delegate void DeleteObjectAndChildObjectsHandler(int requestId);

	public delegate void GetPlugInsHandler(int requestId, MapPlugIn[] mapPlugins);

	public delegate void GetPluginsExtendedEventHandler(int requestId, MapPlugIn[] mapPlugins);

	public delegate void GetPlugInItemsHandler(int requestId, MapPlugInItems mapPluginItems);

	public delegate void GetPluginItemsExtendedEventHandler(int requestId, MapPlugInItems mapPluginItems);

	public delegate void GetPluginItemChangesEventHandler(int requestId, MapPlugInItems mapPluginItems);

	public delegate void GetPluginItemChangesExtendedEventHandler(int requestId, MapPlugInItems mapPluginItems);

	public delegate void ActivatePlugInActionHandler(int requestId);

	public delegate void GetIconDataHandler(int requestId, byte[] iconData);

	public delegate void GetTypeChangedHandler(int requestId, TypeChanged typeChanged);

	public delegate void GetVisualElementChangesHandler(int requestId, VisualElementChange[] visualElementChanges);

	private Dictionary<ServerId, MapClient> _clients = new Dictionary<ServerId, MapClient>();

	private object _lockObject = new object();

	private MapClientFactory _clientFactory = new MapClientFactory();

	public event FaultExceptionHandler FaultExceptionEvent;

	public event CreateObjectHandler CreateObjectEvent;

	public event CreateObjectsHandler CreateObjectsEvent;

	public event UpdateObjectHandler UpdateObjectEvent;

	public event UpdateObjectsHandler UpdateObjectsEvent;

	public event UpdateNameHandler UpdateNameEvent;

	public event UpdateBoundsHandler UpdateBoundsEvent;

	public event UpdateDisplayXMLHandler UpdateDisplayXMLEvent;

	public event GetObjectHandler GetObjectEvent;

	public event GetAllObjectsOfTypeHandler GetAllObjectsOfTypeEvent;

	public event GetAllObjectsOnVisualElementHandler GetAllObjectsOnVisualElementEvent;

	public event GetAllObjectsContainingHandler GetAllObjectsContainingEvent;

	public event GetObjectsWithinGPSAreaHandler GetObjectsWithinGPSAreaEvent;

	public event GetAllObjectIDsOnVisualElementHandler GetAllObjectIDsOnVisualElementEvent;

	public event DeleteObjectHandler DeleteObjectEvent;

	public event DeleteObjectsHandler DeleteObjectsEvent;

	public event DeleteObjectAndChildObjectsHandler DeleteObjectAndChildObjectsEvent;

	public event GetPlugInsHandler GetPlugInsEvent;

	public event GetPluginsExtendedEventHandler GetPluginsExtendedEvent;

	public event GetPlugInItemsHandler GetPlugInItemsEvent;

	public event GetPluginItemsExtendedEventHandler GetPluginItemsExtendedEvent;

	public event GetPluginItemChangesEventHandler GetPluginItemChangesEvent;

	public event GetPluginItemChangesExtendedEventHandler GetPluginItemChangesExtendedEvent;

	public event ActivatePlugInActionHandler ActivatePlugInActionEvent;

	public event GetIconDataHandler GetIconDataEvent;

	public event GetTypeChangedHandler GetTypeChangedEvent;

	public event GetVisualElementChangesHandler GetVisualElementChangesEvent;

	public MapClient GetMapClient(ServerId serverId)
	{
		lock (_lockObject)
		{
			if (!_clients.TryGetValue(serverId, out var value))
			{
				MapClient mapClient = CreateMapClient(serverId);
				if (mapClient == null)
				{
					throw new ServiceNotRegisteredException();
				}
				_clients.Add(serverId, mapClient);
			}
			else
			{
				MapClient mapClient = value;
			}
			return _clients[serverId];
		}
	}

	public void Close()
	{
		lock (_lockObject)
		{
			foreach (MapClient value in _clients.Values)
			{
				DestroyMapClient(value);
			}
			_clients.Clear();
		}
	}

	private MapClient CreateMapClient(ServerId serverId)
	{
		VideoOS.Platform.Configuration instance = VideoOS.Platform.Configuration.Instance;
		try
		{
			List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
			if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
			{
				throw new ServiceNotRegisteredException();
			}
			Version eventServerVersion = ServerReceiver.GetEventServerVersion2(registeredServiceUriInfo[0]);
			MapClient mapClient = _clientFactory.Create(serverId, registeredServiceUriInfo[0].UriArray[0], eventServerVersion);
			mapClient.FaultExceptionEvent += FireFaultException;
			mapClient.CreateObjectEvent += FireCreateObject;
			mapClient.CreateObjectsEvent += FireCreateObjects;
			mapClient.UpdateObjectEvent += FireUpdateObject;
			mapClient.UpdateObjectsEvent += FireUpdateObjects;
			mapClient.UpdateNameEvent += FireUpdateName;
			mapClient.UpdateBoundsEvent += FireUpdateBounds;
			mapClient.UpdateDisplayXMLEvent += FireUpdateDisplayXML;
			mapClient.GetObjectEvent += FireGetObject;
			mapClient.GetAllObjectsOfTypeEvent += FireGetAllObjectsOfType;
			mapClient.GetAllObjectsOnVisualElementEvent += FireGetAllObjectsOnVisualElement;
			mapClient.GetAllObjectsContainingEvent += FireGetAllObjectsContaining;
			mapClient.GetObjectsWithinGPSAreaEvent += FireGetObjectsWithinGPSArea;
			mapClient.GetAllObjectIDsOnVisualElementEvent += FireGetAllObjectIDsOnVisualElement;
			mapClient.DeleteObjectEvent += FireDeleteObject;
			mapClient.DeleteObjectsEvent += FireDeleteObjects;
			mapClient.DeleteObjectAndChildObjectsEvent += FireDeleteObjectAndChildObjects;
			mapClient.GetPlugInsEvent += FireGetPlugIns;
			mapClient.GetPluginsExtendedEvent += FireGetPluginsExtended;
			mapClient.GetPlugInItemsEvent += FireGetPlugInItems;
			mapClient.GetPluginItemsExtendedEvent += FireGetPluginItemsExtended;
			mapClient.GetPlugInItemChangesEvent += FireGetPlugInItemChanges;
			mapClient.GetPluginItemChangesExtendedEvent += FireGetPluginItemChangesExtended;
			mapClient.ActivatePlugInActionEvent += FireActivatePlugInAction;
			mapClient.GetIconDataEvent += FireGetIconData;
			mapClient.GetTypeChangedEvent += FireGetTypeChanged;
			mapClient.GetVisualElementChangesEvent += FireGetVisualElementChanges;
			return mapClient;
		}
		catch (CommunicationMIPException innerException)
		{
			throw new ServiceNotRegisteredException("Error retrieving registered services", innerException);
		}
	}

	private void DestroyMapClient(MapClient mapClient)
	{
		if (mapClient != null)
		{
			mapClient.FaultExceptionEvent -= FireFaultException;
			mapClient.CreateObjectEvent -= FireCreateObject;
			mapClient.CreateObjectsEvent -= FireCreateObjects;
			mapClient.UpdateObjectEvent -= FireUpdateObject;
			mapClient.UpdateObjectsEvent -= FireUpdateObjects;
			mapClient.UpdateNameEvent -= FireUpdateName;
			mapClient.UpdateBoundsEvent -= FireUpdateBounds;
			mapClient.UpdateDisplayXMLEvent -= FireUpdateDisplayXML;
			mapClient.GetObjectEvent -= FireGetObject;
			mapClient.GetAllObjectsOfTypeEvent -= FireGetAllObjectsOfType;
			mapClient.GetAllObjectsOnVisualElementEvent -= FireGetAllObjectsOnVisualElement;
			mapClient.GetAllObjectsContainingEvent -= FireGetAllObjectsContaining;
			mapClient.GetObjectsWithinGPSAreaEvent -= FireGetObjectsWithinGPSArea;
			mapClient.GetAllObjectIDsOnVisualElementEvent -= FireGetAllObjectIDsOnVisualElement;
			mapClient.DeleteObjectEvent -= FireDeleteObject;
			mapClient.DeleteObjectsEvent -= FireDeleteObjects;
			mapClient.DeleteObjectAndChildObjectsEvent -= FireDeleteObjectAndChildObjects;
			mapClient.GetPlugInsEvent -= FireGetPlugIns;
			mapClient.GetPluginsExtendedEvent -= FireGetPluginsExtended;
			mapClient.GetPlugInItemsEvent -= FireGetPlugInItems;
			mapClient.GetPluginItemsExtendedEvent -= FireGetPluginItemsExtended;
			mapClient.GetPlugInItemChangesEvent -= FireGetPlugInItemChanges;
			mapClient.GetPluginItemChangesExtendedEvent -= FireGetPluginItemChangesExtended;
			mapClient.ActivatePlugInActionEvent -= FireActivatePlugInAction;
			mapClient.GetIconDataEvent -= FireGetIconData;
			mapClient.GetTypeChangedEvent -= FireGetTypeChanged;
			mapClient.GetVisualElementChangesEvent -= FireGetVisualElementChanges;
			mapClient.Close();
		}
	}

	private void FireFaultException(Exception error, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(error, requestId);
		}
	}

	private void FireCreateObject(int requestId, VisualElement visualElement)
	{
		if (this.CreateObjectEvent != null)
		{
			this.CreateObjectEvent(requestId, visualElement);
		}
	}

	private void FireCreateObjects(int requestId, VisualElement[] visualElements)
	{
		if (this.CreateObjectsEvent != null)
		{
			this.CreateObjectsEvent(requestId, visualElements);
		}
	}

	private void FireUpdateObject(int requestId, DateTime time)
	{
		if (this.UpdateObjectEvent != null)
		{
			this.UpdateObjectEvent(requestId, time);
		}
	}

	private void FireUpdateObjects(int requestId, DateTime[] times)
	{
		if (this.UpdateObjectsEvent != null)
		{
			this.UpdateObjectsEvent(requestId, times);
		}
	}

	private void FireUpdateName(int requestId, DateTime time)
	{
		if (this.UpdateNameEvent != null)
		{
			this.UpdateNameEvent(requestId, time);
		}
	}

	private void FireUpdateBounds(int requestId, DateTime time)
	{
		if (this.UpdateBoundsEvent != null)
		{
			this.UpdateBoundsEvent(requestId, time);
		}
	}

	private void FireUpdateDisplayXML(int requestId, DateTime time)
	{
		if (this.UpdateDisplayXMLEvent != null)
		{
			this.UpdateDisplayXMLEvent(requestId, time);
		}
	}

	private void FireGetObject(int requestId, VisualElement visualElement)
	{
		if (this.GetObjectEvent != null)
		{
			this.GetObjectEvent(requestId, visualElement);
		}
	}

	private void FireGetAllObjectsOfType(int requestId, TypeForTree[] typeForTreeTypes)
	{
		if (this.GetAllObjectsOfTypeEvent != null)
		{
			this.GetAllObjectsOfTypeEvent(requestId, typeForTreeTypes);
		}
	}

	private void FireGetAllObjectsOnVisualElement(int requestId, VisualElement[] visualElements)
	{
		if (this.GetAllObjectsOnVisualElementEvent != null)
		{
			this.GetAllObjectsOnVisualElementEvent(requestId, visualElements);
		}
	}

	private void FireGetAllObjectsContaining(int requestId, SimpleVisualElement[] simpleVisualElements)
	{
		if (this.GetAllObjectsContainingEvent != null)
		{
			this.GetAllObjectsContainingEvent(requestId, simpleVisualElements);
		}
	}

	private void FireGetObjectsWithinGPSArea(int requestId, VisualElement[] visualElements)
	{
		if (this.GetObjectsWithinGPSAreaEvent != null)
		{
			this.GetObjectsWithinGPSAreaEvent(requestId, visualElements);
		}
	}

	private void FireGetAllObjectIDsOnVisualElement(int requestId, TypeID[] typeIds)
	{
		if (this.GetAllObjectIDsOnVisualElementEvent != null)
		{
			this.GetAllObjectIDsOnVisualElementEvent(requestId, typeIds);
		}
	}

	private void FireDeleteObject(int requestId, VisualElement[] visualElements)
	{
		if (this.DeleteObjectEvent != null)
		{
			this.DeleteObjectEvent(requestId, visualElements);
		}
	}

	private void FireDeleteObjects(int requestId, VisualElement[] visualElements)
	{
		if (this.DeleteObjectsEvent != null)
		{
			this.DeleteObjectsEvent(requestId, visualElements);
		}
	}

	private void FireDeleteObjectAndChildObjects(int requestId)
	{
		if (this.DeleteObjectAndChildObjectsEvent != null)
		{
			this.DeleteObjectAndChildObjectsEvent(requestId);
		}
	}

	private void FireGetPlugIns(int requestId, MapPlugIn[] mapPlugins)
	{
		if (this.GetPlugInsEvent != null)
		{
			this.GetPlugInsEvent(requestId, mapPlugins);
		}
	}

	private void FireGetPluginsExtended(int requestId, MapPlugIn[] mapPlugins)
	{
		if (this.GetPluginsExtendedEvent != null)
		{
			this.GetPluginsExtendedEvent(requestId, mapPlugins);
		}
	}

	private void FireGetPlugInItems(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPlugInItemsEvent != null)
		{
			this.GetPlugInItemsEvent(requestId, mapPluginItems);
		}
	}

	private void FireGetPluginItemsExtended(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPluginItemsExtendedEvent != null)
		{
			this.GetPluginItemsExtendedEvent(requestId, mapPluginItems);
		}
	}

	private void FireGetPlugInItemChanges(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPluginItemChangesEvent != null)
		{
			this.GetPluginItemChangesEvent(requestId, mapPluginItems);
		}
	}

	private void FireGetPluginItemChangesExtended(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPluginItemChangesExtendedEvent != null)
		{
			this.GetPluginItemChangesExtendedEvent(requestId, mapPluginItems);
		}
	}

	private void FireActivatePlugInAction(int requestId)
	{
		if (this.ActivatePlugInActionEvent != null)
		{
			this.ActivatePlugInActionEvent(requestId);
		}
	}

	private void FireGetIconData(int requestId, byte[] iconData)
	{
		if (this.GetIconDataEvent != null)
		{
			this.GetIconDataEvent(requestId, iconData);
		}
	}

	private void FireGetTypeChanged(int requestId, TypeChanged typeChanged)
	{
		if (this.GetTypeChangedEvent != null)
		{
			this.GetTypeChangedEvent(requestId, typeChanged);
		}
	}

	private void FireGetVisualElementChanges(int requestId, VisualElementChange[] visualElementChanges)
	{
		if (this.GetVisualElementChangesEvent != null)
		{
			this.GetVisualElementChangesEvent(requestId, visualElementChanges);
		}
	}
}
