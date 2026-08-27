using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace VideoOS.EventServer.Communication.Status.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/MapServerStatusCommand", ConfigurationName = "VideoOS.EventServer.Communication.Status.Client.IStatusCommandToken2", SessionMode = SessionMode.NotAllowed)]
public interface IStatusCommandToken2
{
	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatus", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusStatusServiceFaultFault", Name = "StatusServiceFault")]
	MapObjectState[] GetStatus(string token, string ID);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusTranslated", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusTranslatedResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusTranslatedStatusServiceFaultFault", Name = "StatusServiceFault")]
	MapObjectState[] GetStatusTranslated(string token, string ID, string language);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusByUserIds", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusByUserIdsResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusByUserIdsStatusServiceFaultFault", Name = "StatusServiceFault")]
	MapObjectState[] GetStatusByUserIds(string token, string ID, MapUserId[] userIds);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusForAllObjectsOfType", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusForAllObjectsOfTypeResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusForAllObjectsOfTypeStatusServiceFaultFault", Name = "StatusServiceFault")]
	MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string token, string ID, Guid type, Guid relationType, int maxLevels);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusDetails", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusDetailsResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetStatusDetailsStatusServiceFaultFault", Name = "StatusServiceFault")]
	string GetStatusDetails(string token, string ID, Guid type, string language);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/SetVisualization", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/SetVisualizationResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/SetVisualizationStatusServiceFaultFault", Name = "StatusServiceFault")]
	DateTime SetVisualization(string token, VisualizationState visualizationState);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetVisualization", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetVisualizationResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetVisualizationStatusServiceFaultFault", Name = "StatusServiceFault")]
	VisualizationState GetVisualization(string token);

	[OperationContract(Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetVisualizationChanges", ReplyAction = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetVisualizationChangesResponse")]
	[FaultContract(typeof(StatusServiceFault), Action = "http://videoos.net/2/MapServerStatusCommand/IStatusCommandToken2/GetVisualizationChangesStatusServiceFaultFault", Name = "StatusServiceFault")]
	VisualizationState GetVisualizationChanges(string token, DateTime timestamp, int timeout);
}
