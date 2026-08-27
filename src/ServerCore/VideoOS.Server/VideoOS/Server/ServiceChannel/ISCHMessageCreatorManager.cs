using System.CodeDom.Compiler;
using System.Net.Security;
using System.ServiceModel;

namespace VideoOS.Server.ServiceChannel;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[ServiceContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagecreator:1", ConfigurationName = "VideoOS.Server.ServiceChannel.ISCHMessageCreatorManager", SessionMode = SessionMode.NotAllowed)]
public interface ISCHMessageCreatorManager
{
	[OperationContract(ProtectionLevel = ProtectionLevel.None, Action = "urn:videoos:contracts:servicechannel:main:messagecreator:1/ISCHMessageCreatorManager/CreateMessages", ReplyAction = "urn:videoos:contracts:servicechannel:main:messagecreator:1/ISCHMessageCreatorManager/CreateMessagesResponse")]
	CreateMessagesMsgOut CreateMessages(SCHmessage[] messages);
}
