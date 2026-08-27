using System.Net.Security;
using System.ServiceModel;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[ServiceContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagecreator:1", SessionMode = SessionMode.NotAllowed)]
public interface ISCHMessageCreatorManager
{
	[OperationContract(ProtectionLevel = ProtectionLevel.None)]
	CreateMessagesMsgOut CreateMessages(SCHmessage[] messages, string identityToken);
}
