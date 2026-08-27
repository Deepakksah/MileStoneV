using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class MapCommandCredentialsClient : ClientBase<IMapCommandCredentials>, IMapCommandCredentials
{
	public MapCommandCredentialsClient()
	{
	}

	public MapCommandCredentialsClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public MapCommandCredentialsClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public MapCommandCredentialsClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public MapCommandCredentialsClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public VisualElement CreateObject(VisualElement visualElement)
	{
		return base.Channel.CreateObject(visualElement);
	}

	public VisualElement[] CreateObjects(VisualElement[] visualElements)
	{
		return base.Channel.CreateObjects(visualElements);
	}

	public DateTime UpdateObject(VisualElement visualElement, bool force)
	{
		return base.Channel.UpdateObject(visualElement, force);
	}

	public DateTime[] UpdateObjects(VisualElement[] visualElements, bool force)
	{
		return base.Channel.UpdateObjects(visualElements, force);
	}

	public DateTime UpdateName(string ID, string visualOwnerID, string name, DateTime timestamp, bool force)
	{
		return base.Channel.UpdateName(ID, visualOwnerID, name, timestamp, force);
	}

	public DateTime UpdateBounds(string ID, string visualOwnerID, Bounds bounds, DateTime timestamp, bool force)
	{
		return base.Channel.UpdateBounds(ID, visualOwnerID, bounds, timestamp, force);
	}

	public DateTime UpdateDisplayXML(string ID, string visualOwnerID, string displayXML, DateTime timestamp, bool force)
	{
		return base.Channel.UpdateDisplayXML(ID, visualOwnerID, displayXML, timestamp, force);
	}

	public VisualElement GetObject(string ID, string visualOwnerID)
	{
		return base.Channel.GetObject(ID, visualOwnerID);
	}

	public TypeForTree[] GetAllObjectsOfType(Guid type, Guid relationType, string ID, int maxLevels, bool includeRoot)
	{
		return base.Channel.GetAllObjectsOfType(type, relationType, ID, maxLevels, includeRoot);
	}

	public VisualElement[] GetAllObjectsOnVisualElement(string ID)
	{
		return base.Channel.GetAllObjectsOnVisualElement(ID);
	}

	public SimpleVisualElement[] GetAllObjectsContaining(Guid type, string[] ids)
	{
		return base.Channel.GetAllObjectsContaining(type, ids);
	}

	public VisualElement[] GetObjectsWithinGPSArea(Bounds area)
	{
		return base.Channel.GetObjectsWithinGPSArea(area);
	}

	public TypeID[] GetAllObjectIDsOnVisualElement(string ID)
	{
		return base.Channel.GetAllObjectIDsOnVisualElement(ID);
	}

	public VisualElement[] DeleteObject(VisualElement visualElement)
	{
		return base.Channel.DeleteObject(visualElement);
	}

	public VisualElement[] DeleteObjects(VisualElement[] elementsToDelete)
	{
		return base.Channel.DeleteObjects(elementsToDelete);
	}

	public void DeleteObjectAndChildObjects(string ID, string visualOwnerID, bool recursive)
	{
		base.Channel.DeleteObjectAndChildObjects(ID, visualOwnerID, recursive);
	}

	public MapPlugIn[] GetPlugIns()
	{
		return base.Channel.GetPlugIns();
	}

	public MapPlugIn[] GetPlugInsExtended(Guid[] kinds, string language)
	{
		return base.Channel.GetPlugInsExtended(kinds, language);
	}

	public MapPlugInItems GetPlugInItems()
	{
		return base.Channel.GetPlugInItems();
	}

	public MapPlugInItems GetPlugInItemsExtended(Guid[] kinds, string language)
	{
		return base.Channel.GetPlugInItemsExtended(kinds, language);
	}

	public MapPlugInItems GetPlugInItemChanges(DateTime timestamp, int timeout)
	{
		return base.Channel.GetPlugInItemChanges(timestamp, timeout);
	}

	public MapPlugInItems GetPlugInItemChangesExtended(DateTime timestamp, int timeout, Guid[] kinds, string language)
	{
		return base.Channel.GetPlugInItemChangesExtended(timestamp, timeout, kinds, language);
	}

	public void ActivatePlugInAction(Guid typeID, string itemID, string actionID)
	{
		base.Channel.ActivatePlugInAction(typeID, itemID, actionID);
	}

	public byte[] GetIconData(Guid type, Guid iconID)
	{
		return base.Channel.GetIconData(type, iconID);
	}

	public TypeChanged GetTypeChanged(Guid[] types, DateTime timestamp, int timeout)
	{
		return base.Channel.GetTypeChanged(types, timestamp, timeout);
	}

	public VisualElementChange[] GetVisualElementChanges(bool includeSubElementChanges, bool ignoreCleanup, string ID, DateTime timestamp, int timeout)
	{
		return base.Channel.GetVisualElementChanges(includeSubElementChanges, ignoreCleanup, ID, timestamp, timeout);
	}
}
