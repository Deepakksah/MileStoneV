using System.Net.Security;
using System.ServiceModel;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[ServiceContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagewatch:1", SessionMode = SessionMode.NotAllowed)]
public interface ISCHMessageWatchManager
{
	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	BeginSessionMsgOut BeginSession(string clientName, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	OperationStatus EndSession(string sessionID, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	OperationStatus AddMessageWatch(string sessionID, string messageTypeName, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	OperationStatus RemoveMessageWatch(string sessionID, string messageTypeName, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	AddMatchExpressionsMsgOut AddMatchExpressions(SCHwatchItem[] watchItems, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	OperationStatus RemoveMatchExpressions(SCHwatchItem[] watchItems, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	OperationStatus ClearMessageWatchers(string sessionID, string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	CheckForMessagesMsgOut CheckForMessages(string sessionID, int millisecsToWaitForMsgs, string identityToken);
}
