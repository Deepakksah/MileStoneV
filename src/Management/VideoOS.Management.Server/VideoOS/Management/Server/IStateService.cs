using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://schemas.videoos/StateService/2008/06/Vmo/01")]
public interface IStateService
{
	[OperationContract]
	int StartStateSession();

	[OperationContract]
	void StopStateSession(int sessionId);

	[OperationContract]
	CurrentState[] SubscribeServerStates(int sessionId, int[] eventTypes, Guid[] itemGuids);

	[OperationContract]
	CurrentState[] SubscribeCameraStates(int sessionId, int[] eventTypes, Guid[] itemGuids);

	[OperationContract]
	void UnsubscribeStates(int sessionId, int[] eventTypes, Guid[] itemGuids);

	[OperationContract]
	Task<CurrentState[]> GetStates(int sessionId, int timeout);

	[OperationContract]
	CurrentState[] GetCurrentServerStates(int[] eventTypes, Guid[] itemGuids);

	[OperationContract]
	CurrentState[] GetCurrentCameraStates(int[] eventTypes, Guid[] itemGuids);
}
