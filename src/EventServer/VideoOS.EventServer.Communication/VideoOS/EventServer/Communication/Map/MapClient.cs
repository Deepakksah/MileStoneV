using System;
using VideoOS.EventServer.Communication.Map.Client;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Map;

public abstract class MapClient : AbstractClient
{
	public delegate void FaultExceptionHandler(Exception exception, int requestId);

	public delegate void CreateObjectHandler(int requestId, VisualElement visualElement);

	public delegate void CreateObjectsHandler(int requestId, VisualElement[] visualElements);

	public delegate void UpdateObjectHandler(int requestId, DateTime time);

	public delegate void UpdateObjectsHandler(int requestId, DateTime[] times);

	public delegate void UpdateNameHandler(int requestId, DateTime time);

	public delegate void UpdateBoundsHandler(int requestId, DateTime time);

	public delegate void UpdateDisplayXMLHandler(int requestId, DateTime time);

	public delegate void GetObjectHandler(int requestId, VisualElement visualElement);

	public delegate void GetAllObjectsOfTypeHandler(int requestId, TypeForTree[] typesForTree);

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

	public delegate void GetPlugInItemChangesHandler(int requestId, MapPlugInItems mapPluginItems);

	public delegate void GetPluginItemChangesExtendedEventHandler(int requestId, MapPlugInItems mapPluginItems);

	public delegate void ActivatePlugInActionHandler(int requestId);

	public delegate void GetIconDataHandler(int requestId, byte[] iconData);

	public delegate void GetTypeChangedHandler(int requestId, TypeChanged typeChanged);

	public delegate void GetVisualElementChangesHandler(int requestId, VisualElementChange[] visualElementChanges);

	protected ServerId ServerId { get; private set; }

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

	public event GetPlugInItemChangesHandler GetPlugInItemChangesEvent;

	public event GetPluginItemChangesExtendedEventHandler GetPluginItemChangesExtendedEvent;

	public event ActivatePlugInActionHandler ActivatePlugInActionEvent;

	public event GetIconDataHandler GetIconDataEvent;

	public event GetTypeChangedHandler GetTypeChangedEvent;

	public event GetVisualElementChangesHandler GetVisualElementChangesEvent;

	protected MapClient(ServerId serverId, string eventServerAddress, Version eventServerVersion, Uri uri)
		: base(eventServerAddress, eventServerVersion, uri)
	{
		ServerId = serverId;
	}

	protected override void HandleRequestObject(RequestObject requestObject)
	{
		if (requestObject is CreateObjectRequestObject)
		{
			CreateObjectRequestObject createObjectRequestObject = requestObject as CreateObjectRequestObject;
			VisualElement visualElement = CreateObject(createObjectRequestObject.VisualElement);
			FireCreateObject(createObjectRequestObject.RequestId, visualElement);
			return;
		}
		if (requestObject is CreateObjectsRequestObject)
		{
			CreateObjectsRequestObject createObjectsRequestObject = requestObject as CreateObjectsRequestObject;
			VisualElement[] visualElements = CreateObjects(createObjectsRequestObject.VisualElements);
			FireCreateObjects(createObjectsRequestObject.RequestId, visualElements);
			return;
		}
		if (requestObject is UpdateObjectRequestObject)
		{
			UpdateObjectRequestObject updateObjectRequestObject = requestObject as UpdateObjectRequestObject;
			DateTime time = UpdateObject(updateObjectRequestObject.VisualElement, updateObjectRequestObject.Force);
			FireUpdateObject(updateObjectRequestObject.RequestId, time);
			return;
		}
		if (requestObject is UpdateObjectsRequestObject)
		{
			UpdateObjectsRequestObject updateObjectsRequestObject = requestObject as UpdateObjectsRequestObject;
			DateTime[] times = UpdateObjects(updateObjectsRequestObject.VisualElements, updateObjectsRequestObject.Force);
			FireUpdateObjects(updateObjectsRequestObject.RequestId, times);
			return;
		}
		if (requestObject is UpdateNameRequestObject)
		{
			UpdateNameRequestObject updateNameRequestObject = requestObject as UpdateNameRequestObject;
			DateTime time2 = UpdateName(updateNameRequestObject.Id, updateNameRequestObject.VisualOwnerId, updateNameRequestObject.Name, updateNameRequestObject.TimeStamp, updateNameRequestObject.Force);
			FireUpdateName(updateNameRequestObject.RequestId, time2);
			return;
		}
		if (requestObject is UpdateBoundsRequestObject)
		{
			UpdateBoundsRequestObject updateBoundsRequestObject = requestObject as UpdateBoundsRequestObject;
			DateTime time3 = UpdateBounds(updateBoundsRequestObject.Id, updateBoundsRequestObject.VisualOwnerId, updateBoundsRequestObject.Bounds, updateBoundsRequestObject.Timestamp, updateBoundsRequestObject.Force);
			FireUpdateBounds(updateBoundsRequestObject.RequestId, time3);
			return;
		}
		if (requestObject is UpdateDisplayXMLRequestObject)
		{
			UpdateDisplayXMLRequestObject updateDisplayXMLRequestObject = requestObject as UpdateDisplayXMLRequestObject;
			DateTime time4 = UpdateDisplayXML(updateDisplayXMLRequestObject.Id, updateDisplayXMLRequestObject.VisualOwnerId, updateDisplayXMLRequestObject.DisplayXml, updateDisplayXMLRequestObject.Timestamp, updateDisplayXMLRequestObject.Force);
			FireUpdateDisplayXML(updateDisplayXMLRequestObject.RequestId, time4);
			return;
		}
		if (requestObject is GetObjectRequestObject)
		{
			GetObjectRequestObject getObjectRequestObject = requestObject as GetObjectRequestObject;
			VisualElement visualElement2 = GetObject(getObjectRequestObject.Id, getObjectRequestObject.VisualOwnerId);
			FireGetObject(getObjectRequestObject.RequestId, visualElement2);
			return;
		}
		if (requestObject is GetAllObjectsOfTypeRequestObject)
		{
			GetAllObjectsOfTypeRequestObject getAllObjectsOfTypeRequestObject = requestObject as GetAllObjectsOfTypeRequestObject;
			TypeForTree[] allObjectsOfType = GetAllObjectsOfType(getAllObjectsOfTypeRequestObject.Type, getAllObjectsOfTypeRequestObject.RelationType, getAllObjectsOfTypeRequestObject.Id, getAllObjectsOfTypeRequestObject.MaxLevels, getAllObjectsOfTypeRequestObject.IncludeRoot);
			FireGetAllObjectsOfType(getAllObjectsOfTypeRequestObject.RequestId, allObjectsOfType);
			return;
		}
		if (requestObject is GetAllObjectsOnVisualElementRequestObject)
		{
			GetAllObjectsOnVisualElementRequestObject getAllObjectsOnVisualElementRequestObject = requestObject as GetAllObjectsOnVisualElementRequestObject;
			VisualElement[] allObjectsOnVisualElement = GetAllObjectsOnVisualElement(getAllObjectsOnVisualElementRequestObject.Id);
			FireGetAllObjectsOnVisualElement(getAllObjectsOnVisualElementRequestObject.RequestId, allObjectsOnVisualElement);
			return;
		}
		if (requestObject is GetAllObjectsContainingRequestObject)
		{
			GetAllObjectsContainingRequestObject getAllObjectsContainingRequestObject = requestObject as GetAllObjectsContainingRequestObject;
			SimpleVisualElement[] allObjectsContaining = GetAllObjectsContaining(getAllObjectsContainingRequestObject.Type, getAllObjectsContainingRequestObject.Ids);
			FireGetAllObjectsContaining(getAllObjectsContainingRequestObject.RequestId, allObjectsContaining);
			return;
		}
		if (requestObject is GetObjectsWithinGPSAreaRequestObject)
		{
			GetObjectsWithinGPSAreaRequestObject getObjectsWithinGPSAreaRequestObject = requestObject as GetObjectsWithinGPSAreaRequestObject;
			VisualElement[] objectsWithinGPSArea = GetObjectsWithinGPSArea(getObjectsWithinGPSAreaRequestObject.Area);
			FireGetObjectsWithinGPSArea(getObjectsWithinGPSAreaRequestObject.RequestId, objectsWithinGPSArea);
			return;
		}
		if (requestObject is GetAllObjectIDsOnVisualElementRequestObject)
		{
			GetAllObjectIDsOnVisualElementRequestObject getAllObjectIDsOnVisualElementRequestObject = requestObject as GetAllObjectIDsOnVisualElementRequestObject;
			TypeID[] allObjectIDsOnVisualElement = GetAllObjectIDsOnVisualElement(getAllObjectIDsOnVisualElementRequestObject.Id);
			FireGetAllObjectIDsOnVisualElement(getAllObjectIDsOnVisualElementRequestObject.RequestId, allObjectIDsOnVisualElement);
			return;
		}
		if (requestObject is DeleteObjectRequestObject)
		{
			DeleteObjectRequestObject deleteObjectRequestObject = requestObject as DeleteObjectRequestObject;
			VisualElement[] visualElements2 = DeleteObject(deleteObjectRequestObject.VisualElement);
			FireDeleteObject(deleteObjectRequestObject.RequestId, visualElements2);
			return;
		}
		if (requestObject is DeleteObjectsRequestObject)
		{
			DeleteObjectsRequestObject deleteObjectsRequestObject = requestObject as DeleteObjectsRequestObject;
			VisualElement[] visualElements3 = DeleteObjects(deleteObjectsRequestObject.ElementsToDelete);
			FireDeleteObjects(deleteObjectsRequestObject.RequestId, visualElements3);
			return;
		}
		if (requestObject is DeleteObjectAndChildObjectsRequestObject)
		{
			DeleteObjectAndChildObjectsRequestObject deleteObjectAndChildObjectsRequestObject = requestObject as DeleteObjectAndChildObjectsRequestObject;
			DeleteObjectAndChildObjects(deleteObjectAndChildObjectsRequestObject.Id, deleteObjectAndChildObjectsRequestObject.VisualOwnerId, deleteObjectAndChildObjectsRequestObject.Recursive);
			FireDeleteObjectAndChildObjects(deleteObjectAndChildObjectsRequestObject.RequestId);
			return;
		}
		if (requestObject is GetPlugInsRequestObject)
		{
			GetPlugInsRequestObject getPlugInsRequestObject = requestObject as GetPlugInsRequestObject;
			MapPlugIn[] plugIns = GetPlugIns();
			FireGetPlugIns(getPlugInsRequestObject.RequestId, plugIns);
			return;
		}
		if (requestObject is GetPluginsExtendedRequestObject)
		{
			GetPluginsExtendedRequestObject getPluginsExtendedRequestObject = requestObject as GetPluginsExtendedRequestObject;
			MapPlugIn[] pluginsExtended = GetPluginsExtended(getPluginsExtendedRequestObject.Kinds, getPluginsExtendedRequestObject.Language);
			FireGetPluginsExtended(getPluginsExtendedRequestObject.RequestId, pluginsExtended);
			return;
		}
		if (requestObject is GetPlugInItemsRequestObject)
		{
			GetPlugInItemsRequestObject getPlugInItemsRequestObject = requestObject as GetPlugInItemsRequestObject;
			MapPlugInItems plugInItems = GetPlugInItems();
			FireGetPlugInItems(getPlugInItemsRequestObject.RequestId, plugInItems);
			return;
		}
		if (requestObject is GetPluginItemsExtendedRequestObject)
		{
			GetPluginItemsExtendedRequestObject getPluginItemsExtendedRequestObject = requestObject as GetPluginItemsExtendedRequestObject;
			MapPlugInItems pluginItemsExtended = GetPluginItemsExtended(getPluginItemsExtendedRequestObject.Kinds, getPluginItemsExtendedRequestObject.Language);
			FireGetPluginItemsExtended(getPluginItemsExtendedRequestObject.RequestId, pluginItemsExtended);
			return;
		}
		if (requestObject is GetPlugInItemChangesRequestObject)
		{
			GetPlugInItemChangesRequestObject getPlugInItemChangesRequestObject = requestObject as GetPlugInItemChangesRequestObject;
			MapPlugInItems plugInItemChanges = GetPlugInItemChanges(getPlugInItemChangesRequestObject.Timestamp, getPlugInItemChangesRequestObject.Timeout);
			FireGetPlugInItemChanges(getPlugInItemChangesRequestObject.RequestId, plugInItemChanges);
			return;
		}
		if (requestObject is GetPluginItemChangesExtendedRequestObject)
		{
			GetPluginItemChangesExtendedRequestObject getPluginItemChangesExtendedRequestObject = requestObject as GetPluginItemChangesExtendedRequestObject;
			MapPlugInItems pluginItemChangesExtended = GetPluginItemChangesExtended(getPluginItemChangesExtendedRequestObject.Timestamp, getPluginItemChangesExtendedRequestObject.Timeout, getPluginItemChangesExtendedRequestObject.Kinds, getPluginItemChangesExtendedRequestObject.Language);
			FireGetPluginItemChangesExtended(getPluginItemChangesExtendedRequestObject.RequestId, pluginItemChangesExtended);
			return;
		}
		if (requestObject is ActivatePlugInActionRequestObject)
		{
			ActivatePlugInActionRequestObject activatePlugInActionRequestObject = requestObject as ActivatePlugInActionRequestObject;
			ActivatePlugInAction(activatePlugInActionRequestObject.TypeId, activatePlugInActionRequestObject.ItemId, activatePlugInActionRequestObject.ActionId);
			FireActivatePlugInAction(activatePlugInActionRequestObject.RequestId);
			return;
		}
		if (requestObject is GetIconDataRequestObject)
		{
			GetIconDataRequestObject getIconDataRequestObject = requestObject as GetIconDataRequestObject;
			byte[] iconData = GetIconData(getIconDataRequestObject.Type, getIconDataRequestObject.IconId);
			FireGetIconData(getIconDataRequestObject.RequestId, iconData);
			return;
		}
		if (requestObject is GetTypeChangedRequestObject)
		{
			GetTypeChangedRequestObject getTypeChangedRequestObject = requestObject as GetTypeChangedRequestObject;
			TypeChanged typeChanged = GetTypeChanged(getTypeChangedRequestObject.Types, getTypeChangedRequestObject.Timestamp, getTypeChangedRequestObject.Timeout);
			FireGetTypeChanged(getTypeChangedRequestObject.RequestId, typeChanged);
			return;
		}
		if (requestObject is GetVisualElementChangesRequestObject)
		{
			GetVisualElementChangesRequestObject getVisualElementChangesRequestObject = requestObject as GetVisualElementChangesRequestObject;
			VisualElementChange[] visualElementChanges = GetVisualElementChanges(getVisualElementChangesRequestObject.IncludeSubElementChanges, getVisualElementChangesRequestObject.IgnoreCleanup, getVisualElementChangesRequestObject.Id, getVisualElementChangesRequestObject.Timestamp, getVisualElementChangesRequestObject.Timeout);
			FireGetVisualElementChanges(getVisualElementChangesRequestObject.RequestId, visualElementChanges);
			return;
		}
		throw new ArgumentException("Unknown request object");
	}

	protected override void FireFaultException(Exception exception, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(exception, requestId);
		}
	}

	private void FireCreateObject(int requestId, VisualElement visualElement)
	{
		if (this.CreateObjectEvent != null)
		{
			this.CreateObjectEvent(requestId, visualElement);
		}
	}

	public void BeginCreateObject(int requestId, VisualElement visualElement)
	{
		Enqueue(new CreateObjectRequestObject(requestId, visualElement));
	}

	public abstract VisualElement CreateObject(VisualElement visualElement);

	private void FireCreateObjects(int requestId, VisualElement[] visualElements)
	{
		if (this.CreateObjectsEvent != null)
		{
			this.CreateObjectsEvent(requestId, visualElements);
		}
	}

	public void BeginCreateObjects(int requestId, VisualElement[] visualElements)
	{
		Enqueue(new CreateObjectsRequestObject(requestId, visualElements));
	}

	public abstract VisualElement[] CreateObjects(VisualElement[] visualElements);

	private void FireUpdateObject(int requestId, DateTime time)
	{
		if (this.UpdateObjectEvent != null)
		{
			this.UpdateObjectEvent(requestId, time);
		}
	}

	public void BeginUpdateObject(int requestId, VisualElement visualElement, bool force)
	{
		Enqueue(new UpdateObjectRequestObject(requestId, visualElement, force));
	}

	public abstract DateTime UpdateObject(VisualElement visualElement, bool force);

	private void FireUpdateObjects(int requestId, DateTime[] times)
	{
		if (this.UpdateObjectsEvent != null)
		{
			this.UpdateObjectsEvent(requestId, times);
		}
	}

	public void BeginUpdateObjects(int requestId, VisualElement[] visualElements, bool force)
	{
		Enqueue(new UpdateObjectsRequestObject(requestId, visualElements, force));
	}

	public abstract DateTime[] UpdateObjects(VisualElement[] visualElements, bool force);

	private void FireUpdateName(int requestId, DateTime time)
	{
		if (this.UpdateNameEvent != null)
		{
			this.UpdateNameEvent(requestId, time);
		}
	}

	public void BeginUpdateName(int requestId, string id, string visualOwnerId, string name, DateTime timestamp, bool force)
	{
		Enqueue(new UpdateNameRequestObject(requestId, id, visualOwnerId, name, timestamp, force));
	}

	public abstract DateTime UpdateName(string id, string visualOwnerId, string name, DateTime timestamp, bool force);

	private void FireUpdateBounds(int requestId, DateTime time)
	{
		if (this.UpdateBoundsEvent != null)
		{
			this.UpdateBoundsEvent(requestId, time);
		}
	}

	public void BeginUpdateBounds(int requestId, string id, string visualOwnerId, Bounds bounds, DateTime timestamp, bool force)
	{
		Enqueue(new UpdateBoundsRequestObject(requestId, id, visualOwnerId, bounds, timestamp, force));
	}

	public abstract DateTime UpdateBounds(string id, string visualOwnerId, Bounds bounds, DateTime timestamp, bool force);

	private void FireUpdateDisplayXML(int requestId, DateTime time)
	{
		if (this.UpdateDisplayXMLEvent != null)
		{
			this.UpdateDisplayXMLEvent(requestId, time);
		}
	}

	public void BeginUpdateDisplayXML(int requestId, string id, string visualOwnerId, string displayXml, DateTime timestamp, bool force)
	{
		Enqueue(new UpdateDisplayXMLRequestObject(requestId, id, visualOwnerId, displayXml, timestamp, force));
	}

	public abstract DateTime UpdateDisplayXML(string id, string visualOwnerId, string displayXml, DateTime timestamp, bool force);

	private void FireGetObject(int requestId, VisualElement visualElement)
	{
		if (this.GetObjectEvent != null)
		{
			this.GetObjectEvent(requestId, visualElement);
		}
	}

	public void BeginGetObject(int requestId, string id, string visualOwnerId)
	{
		Enqueue(new GetObjectRequestObject(requestId, id, visualOwnerId));
	}

	public abstract VisualElement GetObject(string id, string visualOwnerId);

	private void FireGetAllObjectsOfType(int requestId, TypeForTree[] typesForTree)
	{
		if (this.GetAllObjectsOfTypeEvent != null)
		{
			this.GetAllObjectsOfTypeEvent(requestId, typesForTree);
		}
	}

	public void BeginGetAllObjectsOfType(int requestId, Guid type, Guid relationType, string id, int maxLevels, bool includeRoot)
	{
		Enqueue(new GetAllObjectsOfTypeRequestObject(requestId, type, relationType, id, maxLevels, includeRoot));
	}

	public abstract TypeForTree[] GetAllObjectsOfType(Guid type, Guid relationType, string id, int maxLevels, bool includeRoot);

	private void FireGetAllObjectsOnVisualElement(int requestId, VisualElement[] visualElements)
	{
		if (this.GetAllObjectsOnVisualElementEvent != null)
		{
			this.GetAllObjectsOnVisualElementEvent(requestId, visualElements);
		}
	}

	public void BeginGetAllObjectsOnVisualElement(int requestId, string id)
	{
		Enqueue(new GetAllObjectsOnVisualElementRequestObject(requestId, id));
	}

	public abstract VisualElement[] GetAllObjectsOnVisualElement(string id);

	private void FireGetAllObjectsContaining(int requestId, SimpleVisualElement[] simpleVisualElements)
	{
		if (this.GetAllObjectsContainingEvent != null)
		{
			this.GetAllObjectsContainingEvent(requestId, simpleVisualElements);
		}
	}

	public void BeginGetAllObjectsContaining(int requestId, Guid type, string[] ids)
	{
		Enqueue(new GetAllObjectsContainingRequestObject(requestId, type, ids));
	}

	public abstract SimpleVisualElement[] GetAllObjectsContaining(Guid type, string[] ids);

	private void FireGetObjectsWithinGPSArea(int requestId, VisualElement[] visualElements)
	{
		if (this.GetObjectsWithinGPSAreaEvent != null)
		{
			this.GetObjectsWithinGPSAreaEvent(requestId, visualElements);
		}
	}

	public void BeginGetObjectsWithinGPSArea(int requestId, Bounds area)
	{
		Enqueue(new GetObjectsWithinGPSAreaRequestObject(requestId, area));
	}

	public abstract VisualElement[] GetObjectsWithinGPSArea(Bounds area);

	private void FireGetAllObjectIDsOnVisualElement(int requestId, TypeID[] typeIds)
	{
		if (this.GetAllObjectIDsOnVisualElementEvent != null)
		{
			this.GetAllObjectIDsOnVisualElementEvent(requestId, typeIds);
		}
	}

	public void BeginGetAllObjectIDsOnVisualElement(int requestId, string id)
	{
		Enqueue(new GetAllObjectIDsOnVisualElementRequestObject(requestId, id));
	}

	public abstract TypeID[] GetAllObjectIDsOnVisualElement(string id);

	private void FireDeleteObject(int requestId, VisualElement[] visualElements)
	{
		if (this.DeleteObjectEvent != null)
		{
			this.DeleteObjectEvent(requestId, visualElements);
		}
	}

	public void BeginDeleteObject(int requestId, VisualElement visualElement)
	{
		Enqueue(new DeleteObjectRequestObject(requestId, visualElement));
	}

	public abstract VisualElement[] DeleteObject(VisualElement visualElement);

	private void FireDeleteObjects(int requestId, VisualElement[] visualElements)
	{
		if (this.DeleteObjectsEvent != null)
		{
			this.DeleteObjectsEvent(requestId, visualElements);
		}
	}

	public void BeginDeleteObjects(int requestId, VisualElement[] elementsToDelete)
	{
		Enqueue(new DeleteObjectsRequestObject(requestId, elementsToDelete));
	}

	public abstract VisualElement[] DeleteObjects(VisualElement[] elementsToDelete);

	private void FireDeleteObjectAndChildObjects(int requestId)
	{
		if (this.DeleteObjectAndChildObjectsEvent != null)
		{
			this.DeleteObjectAndChildObjectsEvent(requestId);
		}
	}

	public void BeginDeleteObjectAndChildObjects(int requestId, string id, string visualOwnerId, bool recursive)
	{
		Enqueue(new DeleteObjectAndChildObjectsRequestObject(requestId, id, visualOwnerId, recursive));
	}

	public abstract void DeleteObjectAndChildObjects(string id, string visualOwnerId, bool recursive);

	private void FireGetPlugIns(int requestId, MapPlugIn[] mapPlugins)
	{
		if (this.GetPlugInsEvent != null)
		{
			this.GetPlugInsEvent(requestId, mapPlugins);
		}
	}

	public void BeginGetPlugIns(int requestId)
	{
		Enqueue(new GetPlugInsRequestObject(requestId));
	}

	public abstract MapPlugIn[] GetPlugIns();

	private void FireGetPluginsExtended(int requestId, MapPlugIn[] mapPlugins)
	{
		if (this.GetPluginsExtendedEvent != null)
		{
			this.GetPluginsExtendedEvent(requestId, mapPlugins);
		}
	}

	public void BeginGetPluginsExtended(int requestId, Guid[] kinds, string language)
	{
		Enqueue(new GetPluginsExtendedRequestObject(requestId, kinds, language));
	}

	public abstract MapPlugIn[] GetPluginsExtended(Guid[] kinds, string language);

	private void FireGetPlugInItems(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPlugInItemsEvent != null)
		{
			this.GetPlugInItemsEvent(requestId, mapPluginItems);
		}
	}

	public void BeginGetPlugInItems(int requestId)
	{
		Enqueue(new GetPlugInItemsRequestObject(requestId));
	}

	public abstract MapPlugInItems GetPlugInItems();

	private void FireGetPluginItemsExtended(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPluginItemsExtendedEvent != null)
		{
			this.GetPluginItemsExtendedEvent(requestId, mapPluginItems);
		}
	}

	public void BeginGetPluginItemsExtended(int requestId, Guid[] kinds, string language)
	{
		Enqueue(new GetPluginItemsExtendedRequestObject(requestId, kinds, language));
	}

	public abstract MapPlugInItems GetPluginItemsExtended(Guid[] kinds, string language);

	private void FireGetPlugInItemChanges(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPlugInItemChangesEvent != null)
		{
			this.GetPlugInItemChangesEvent(requestId, mapPluginItems);
		}
	}

	public void BeginGetPlugInItemChanges(int requestId, DateTime timestamp, int timeout)
	{
		Enqueue(new GetPlugInItemChangesRequestObject(requestId, timestamp, timeout));
	}

	public abstract MapPlugInItems GetPlugInItemChanges(DateTime timestamp, int timeout);

	private void FireGetPluginItemChangesExtended(int requestId, MapPlugInItems mapPluginItems)
	{
		if (this.GetPluginItemChangesExtendedEvent != null)
		{
			this.GetPluginItemChangesExtendedEvent(requestId, mapPluginItems);
		}
	}

	public void BeginGetPluginItemChangesExtended(int requestId, DateTime timestamp, int timeout, Guid[] kinds, string language)
	{
		Enqueue(new GetPluginItemChangesExtendedRequestObject(requestId, timestamp, timeout, kinds, language));
	}

	public abstract MapPlugInItems GetPluginItemChangesExtended(DateTime timestamp, int timeout, Guid[] kinds, string language);

	private void FireActivatePlugInAction(int requestId)
	{
		if (this.ActivatePlugInActionEvent != null)
		{
			this.ActivatePlugInActionEvent(requestId);
		}
	}

	public void BeginActivatePlugInAction(int requestId, Guid typeId, string itemId, string actionId)
	{
		Enqueue(new ActivatePlugInActionRequestObject(requestId, typeId, itemId, actionId));
	}

	public abstract void ActivatePlugInAction(Guid typeId, string itemId, string actionId);

	private void FireGetIconData(int requestId, byte[] iconData)
	{
		if (this.GetIconDataEvent != null)
		{
			this.GetIconDataEvent(requestId, iconData);
		}
	}

	public void BeginGetIconData(int requestId, Guid type, Guid iconId)
	{
		Enqueue(new GetIconDataRequestObject(requestId, type, iconId));
	}

	public abstract byte[] GetIconData(Guid type, Guid iconId);

	private void FireGetTypeChanged(int requestId, TypeChanged typechanged)
	{
		if (this.GetTypeChangedEvent != null)
		{
			this.GetTypeChangedEvent(requestId, typechanged);
		}
	}

	public void BeginGetTypeChanged(int requestId, Guid[] types, DateTime timeStamp, int timeout)
	{
		Enqueue(new GetTypeChangedRequestObject(requestId, types, timeStamp, timeout));
	}

	public abstract TypeChanged GetTypeChanged(Guid[] types, DateTime timeStamp, int timeout);

	private void FireGetVisualElementChanges(int requestId, VisualElementChange[] visualElementChanges)
	{
		if (this.GetVisualElementChangesEvent != null)
		{
			this.GetVisualElementChangesEvent(requestId, visualElementChanges);
		}
	}

	public void BeginGetVisualElementChanges(int requestId, bool includeSubElementChanges, bool ignoreCleanup, string id, DateTime timestamp, int timeout)
	{
		Enqueue(new GetVisualElementChangesRequestObject(requestId, includeSubElementChanges, ignoreCleanup, id, timestamp, timeout));
	}

	public abstract VisualElementChange[] GetVisualElementChanges(bool includeSubElementChanges, bool ignoreCleanup, string id, DateTime timestamp, int timeout);
}
