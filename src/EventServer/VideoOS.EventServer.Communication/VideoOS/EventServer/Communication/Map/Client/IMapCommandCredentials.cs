using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.EventServer.Communication.Map.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerMapCommand", ConfigurationName = "VideoOS.EventServer.Communication.Map.Client.IMapCommandCredentials", SessionMode = SessionMode.NotAllowed)]
public interface IMapCommandCredentials
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/CreateObject", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/CreateObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/CreateObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement CreateObject(VisualElement visualElement);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/CreateObjects", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/CreateObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/CreateObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] CreateObjects(VisualElement[] visualElements);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateObject", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateObject(VisualElement visualElement, bool force);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateObjects", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime[] UpdateObjects(VisualElement[] visualElements, bool force);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateName", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateNameResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateNameMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateName(string ID, string visualOwnerID, string name, DateTime timestamp, bool force);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateBounds", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateBoundsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateBoundsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateBounds(string ID, string visualOwnerID, Bounds bounds, DateTime timestamp, bool force);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateDisplayXML", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateDisplayXMLResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/UpdateDisplayXMLMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime UpdateDisplayXML(string ID, string visualOwnerID, string displayXML, DateTime timestamp, bool force);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetObject", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement GetObject(string ID, string visualOwnerID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsOfType", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsOfTypeResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsOfTypeMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	TypeForTree[] GetAllObjectsOfType(Guid type, Guid relationType, string ID, int maxLevels, bool includeRoot);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsOnVisualElement", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsOnVisualElementResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsOnVisualElementMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] GetAllObjectsOnVisualElement(string ID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsContaining", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsContainingResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectsContainingMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	SimpleVisualElement[] GetAllObjectsContaining(Guid type, string[] ids);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetObjectsWithinGPSArea", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetObjectsWithinGPSAreaResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetObjectsWithinGPSAreaMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] GetObjectsWithinGPSArea(Bounds area);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectIDsOnVisualElement", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectIDsOnVisualElementResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetAllObjectIDsOnVisualElementMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	TypeID[] GetAllObjectIDsOnVisualElement(string ID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObject", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] DeleteObject(VisualElement visualElement);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjects", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElement[] DeleteObjects(VisualElement[] elementsToDelete);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectAndChildObjects", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectAndChildObjectsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/DeleteObjectAndChildObjectsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	void DeleteObjectAndChildObjects(string ID, string visualOwnerID, bool recursive);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugIns", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapPlugIn[] GetPlugIns();

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInsExtended", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInsExtendedResponse")]
	[FaultContract(typeof(MapServiceFault2), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInsExtendedMapServiceFault2Fault", Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
	MapPlugIn[] GetPlugInsExtended(Guid[] kinds, string language);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItems", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapPlugInItems GetPlugInItems();

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemsExtended", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemsExtendedResponse")]
	[FaultContract(typeof(MapServiceFault2), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemsExtendedMapServiceFault2Fault", Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
	MapPlugInItems GetPlugInItemsExtended(Guid[] kinds, string language);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemChanges", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemChangesResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemChangesMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapPlugInItems GetPlugInItemChanges(DateTime timestamp, int timeout);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemChangesExtended", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemChangesExtendedResponse")]
	[FaultContract(typeof(MapServiceFault2), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetPlugInItemChangesExtendedMapServiceFault2Fault", Name = "MapServiceFault2", Namespace = "http://videoos.net/2/EventServerMapCommand")]
	MapPlugInItems GetPlugInItemChangesExtended(DateTime timestamp, int timeout, Guid[] kinds, string language);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/ActivatePlugInAction", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/ActivatePlugInActionResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/ActivatePlugInActionMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	void ActivatePlugInAction(Guid typeID, string itemID, string actionID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetIconData", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetIconDataResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetIconDataMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	byte[] GetIconData(Guid type, Guid iconID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetTypeChanged", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetTypeChangedResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetTypeChangedMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	TypeChanged GetTypeChanged(Guid[] types, DateTime timestamp, int timeout);

	[OperationContract(Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetVisualElementChanges", ReplyAction = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetVisualElementChangesResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerMapCommand/IMapCommandCredentials/GetVisualElementChangesMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualElementChange[] GetVisualElementChanges(bool includeSubElementChanges, bool ignoreCleanup, string ID, DateTime timestamp, int timeout);
}
