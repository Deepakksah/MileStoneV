using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.EventServer.Communication.Status.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerStatusCommand", ConfigurationName = "VideoOS.EventServer.Communication.Status.Client.IStatusCommandCredentials", SessionMode = SessionMode.NotAllowed)]
public interface IStatusCommandCredentials
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatus", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapObjectState[] GetStatus(string ID);

	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusTranslated", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusTranslatedResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusTranslatedMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapObjectState[] GetStatusTranslated(string ID, string language);

	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusForAllObjectsOfType", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusForAllObjectsOfTypeResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusForAllObjectsOfTypeMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string ID, Guid type, Guid relationType, int maxLevels);

	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusDetails", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusDetailsResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetStatusDetailsMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	string GetStatusDetails(string ID, Guid type, string language);

	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/SetVisualization", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/SetVisualizationResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/SetVisualizationMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	DateTime SetVisualization(VisualizationState visualizationState);

	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetVisualization", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetVisualizationResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetVisualizationMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualizationState GetVisualization();

	[OperationContract(Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetVisualizationChanges", ReplyAction = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetVisualizationChangesResponse")]
	[FaultContract(typeof(MapServiceFault), Action = "http://videoos.net/2/CentralServerStatusCommand/IStatusCommandCredentials/GetVisualizationChangesMapServiceFaultFault", Name = "MapServiceFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
	VisualizationState GetVisualizationChanges(DateTime timestamp, int timeout);
}
