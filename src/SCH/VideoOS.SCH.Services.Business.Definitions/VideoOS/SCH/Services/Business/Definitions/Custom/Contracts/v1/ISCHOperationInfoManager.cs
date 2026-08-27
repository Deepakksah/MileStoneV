using System.Net.Security;
using System.ServiceModel;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[ServiceContract(Namespace = "urn:videoos:contracts:servicechannel:main:operationinfo:1", SessionMode = SessionMode.NotAllowed)]
public interface ISCHOperationInfoManager
{
	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetConnectedClientsMsgOut GetConnectedClients(string identityToken);

	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	GetAllMatchOperatorsMsgOut GetAllMatchOperators(string identityToken);
}
