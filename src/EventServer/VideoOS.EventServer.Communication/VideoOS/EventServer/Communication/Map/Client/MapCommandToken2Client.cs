using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class MapCommandToken2Client : ClientBase<IMapCommandToken2>, IMapCommandToken2
{
	public MapCommandToken2Client()
	{
	}

	public MapCommandToken2Client(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public MapCommandToken2Client(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public MapCommandToken2Client(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public MapCommandToken2Client(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public VisualElement CreateObject(string token, VisualElement visualElement)
	{
		return base.Channel.CreateObject(token, visualElement);
	}

	public VisualElement[] CreateObjects(string token, VisualElement[] visualElements)
	{
		return base.Channel.CreateObjects(token, visualElements);
	}

	public DateTime UpdateObject(string token, VisualElement visualElement, bool force)
	{
		return base.Channel.UpdateObject(token, visualElement, force);
	}

	public DateTime[] UpdateObjects(string token, VisualElement[] visualElements, bool force)
	{
		return base.Channel.UpdateObjects(token, visualElements, force);
	}

	public DateTime UpdateName(string token, string ID, string visualOwnerID, string name, DateTime timestamp, bool force)
	{
		return base.Channel.UpdateName(token, ID, visualOwnerID, name, timestamp, force);
	}

	public DateTime UpdateBounds(string token, string ID, string visualOwnerID, Bounds bounds, DateTime timestamp, bool force)
	{
		return base.Channel.UpdateBounds(token, ID, visualOwnerID, bounds, timestamp, force);
	}

	public DateTime UpdateDisplayXML(string token, string ID, string visualOwnerID, string displayXML, DateTime timestamp, bool force)
	{
		return base.Channel.UpdateDisplayXML(token, ID, visualOwnerID, displayXML, timestamp, force);
	}

	public VisualElement GetObject(string token, string ID, string visualOwnerID)
	{
		return base.Channel.GetObject(token, ID, visualOwnerID);
	}

	public TypeForTree[] GetAllObjectsOfType(string token, Guid type, Guid relationType, string ID, int maxLevels, bool includeRoot)
	{
		return base.Channel.GetAllObjectsOfType(token, type, relationType, ID, maxLevels, includeRoot);
	}

	public VisualElement[] GetAllObjectsOnVisualElement(string token, string ID)
	{
		return base.Channel.GetAllObjectsOnVisualElement(token, ID);
	}

	public SimpleVisualElement[] GetAllObjectsContaining(string token, Guid type, string[] ids)
	{
		return base.Channel.GetAllObjectsContaining(token, type, ids);
	}

	public VisualElement[] GetObjectsWithinGPSArea(string token, Bounds area)
	{
		return base.Channel.GetObjectsWithinGPSArea(token, area);
	}

	public TypeID[] GetAllObjectIDsOnVisualElement(string token, string ID)
	{
		return base.Channel.GetAllObjectIDsOnVisualElement(token, ID);
	}

	public VisualElement[] DeleteObject(string token, VisualElement visualElement)
	{
		return base.Channel.DeleteObject(token, visualElement);
	}

	public VisualElement[] DeleteObjects(string token, VisualElement[] elementsToDelete)
	{
		return base.Channel.DeleteObjects(token, elementsToDelete);
	}

	public void DeleteObjectAndChildObjects(string token, string ID, string visualOwnerID, bool recursive)
	{
		base.Channel.DeleteObjectAndChildObjects(token, ID, visualOwnerID, recursive);
	}

	public MapPlugIn[] GetPlugIns(string token)
	{
		return base.Channel.GetPlugIns(token);
	}

	public MapPlugIn[] GetPlugInsExtended(string token, Guid[] kinds, string language)
	{
		return base.Channel.GetPlugInsExtended(token, kinds, language);
	}

	public MapPlugInItems GetPlugInItems(string token)
	{
		return base.Channel.GetPlugInItems(token);
	}

	public MapPlugInItems GetPlugInItemsExtended(string token, Guid[] kinds, string language)
	{
		return base.Channel.GetPlugInItemsExtended(token, kinds, language);
	}

	public MapPlugInItems GetPlugInItemChanges(string token, DateTime timestamp, int timeout)
	{
		return base.Channel.GetPlugInItemChanges(token, timestamp, timeout);
	}

	public MapPlugInItems GetPlugInItemChangesExtended(string token, DateTime timestamp, int timeout, Guid[] kinds, string language)
	{
		return base.Channel.GetPlugInItemChangesExtended(token, timestamp, timeout, kinds, language);
	}

	public void ActivatePlugInAction(string token, Guid typeID, string itemID, string actionID)
	{
		base.Channel.ActivatePlugInAction(token, typeID, itemID, actionID);
	}

	public byte[] GetIconData(string token, Guid type, Guid iconID)
	{
		return base.Channel.GetIconData(token, type, iconID);
	}

	public TypeChanged GetTypeChanged(string token, Guid[] types, DateTime timestamp, int timeout)
	{
		return base.Channel.GetTypeChanged(token, types, timestamp, timeout);
	}

	public VisualElementChange[] GetVisualElementChanges(string token, bool includeSubElementChanges, bool ignoreCleanup, string ID, DateTime timestamp, int timeout)
	{
		return base.Channel.GetVisualElementChanges(token, includeSubElementChanges, ignoreCleanup, ID, timestamp, timeout);
	}
}
