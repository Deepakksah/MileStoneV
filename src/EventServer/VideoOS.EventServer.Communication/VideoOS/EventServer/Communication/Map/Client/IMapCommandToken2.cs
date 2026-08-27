using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.EventServer.Communication.Map.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/MapServerMapCommand", ConfigurationName = "VideoOS.EventServer.Communication.Map.Client.IMapCommandToken2", SessionMode = SessionMode.NotAllowed)]
public interface IMapCommandToken2
{
	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/CreateObject", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/CreateObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/CreateObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement CreateObject(string token, VisualElement visualElement);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/CreateObjects", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/CreateObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/CreateObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] CreateObjects(string token, VisualElement[] visualElements);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateObject", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateObject(string token, VisualElement visualElement, bool force);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateObjects", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime[] UpdateObjects(string token, VisualElement[] visualElements, bool force);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateName", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateNameResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateNameMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateName(string token, string ID, string visualOwnerID, string name, DateTime timestamp, bool force);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateBounds", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateBoundsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateBoundsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateBounds(string token, string ID, string visualOwnerID, Bounds bounds, DateTime timestamp, bool force);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateDisplayXML", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateDisplayXMLResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/UpdateDisplayXMLMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateDisplayXML(string token, string ID, string visualOwnerID, string displayXML, DateTime timestamp, bool force);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetObject", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement GetObject(string token, string ID, string visualOwnerID);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsOfType", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsOfTypeResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsOfTypeMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	TypeForTree[] GetAllObjectsOfType(string token, Guid type, Guid relationType, string ID, int maxLevels, bool includeRoot);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsOnVisualElement", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsOnVisualElementResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsOnVisualElementMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] GetAllObjectsOnVisualElement(string token, string ID);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsContaining", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsContainingResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectsContainingMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	SimpleVisualElement[] GetAllObjectsContaining(string token, Guid type, string[] ids);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetObjectsWithinGPSArea", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetObjectsWithinGPSAreaResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetObjectsWithinGPSAreaMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] GetObjectsWithinGPSArea(string token, Bounds area);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectIDsOnVisualElement", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectIDsOnVisualElementResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetAllObjectIDsOnVisualElementMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	TypeID[] GetAllObjectIDsOnVisualElement(string token, string ID);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObject", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] DeleteObject(string token, VisualElement visualElement);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjects", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] DeleteObjects(string token, VisualElement[] elementsToDelete);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectAndChildObjects", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectAndChildObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/DeleteObjectAndChildObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	void DeleteObjectAndChildObjects(string token, string ID, string visualOwnerID, bool recursive);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugIns", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapPlugIn[] GetPlugIns(string token);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInsExtended", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInsExtendedResponse")]
	[FaultContract(typeof(MapServiceFault2), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInsExtendedMapServiceFault2Fault", Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
	MapPlugIn[] GetPlugInsExtended(string token, Guid[] kinds, string language);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItems", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapPlugInItems GetPlugInItems(string token);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemsExtended", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemsExtendedResponse")]
	[FaultContract(typeof(MapServiceFault2), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemsExtendedMapServiceFault2Fault", Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
	MapPlugInItems GetPlugInItemsExtended(string token, Guid[] kinds, string language);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemChanges", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemChangesResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemChangesMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapPlugInItems GetPlugInItemChanges(string token, DateTime timestamp, int timeout);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemChangesExtended", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemChangesExtendedResponse")]
	[FaultContract(typeof(MapServiceFault2), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetPlugInItemChangesExtendedMapServiceFault2Fault", Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
	MapPlugInItems GetPlugInItemChangesExtended(string token, DateTime timestamp, int timeout, Guid[] kinds, string language);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/ActivatePlugInAction", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/ActivatePlugInActionResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/ActivatePlugInActionMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	void ActivatePlugInAction(string token, Guid typeID, string itemID, string actionID);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetIconData", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetIconDataResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetIconDataMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	byte[] GetIconData(string token, Guid type, Guid iconID);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetTypeChanged", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetTypeChangedResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetTypeChangedMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	TypeChanged GetTypeChanged(string token, Guid[] types, DateTime timestamp, int timeout);

	[OperationContract(Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetVisualElementChanges", ReplyAction = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetVisualElementChangesResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/MapServerMapCommand/IMapCommandToken2/GetVisualElementChangesMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElementChange[] GetVisualElementChanges(string token, bool includeSubElementChanges, bool ignoreCleanup, string ID, DateTime timestamp, int timeout);
}
