using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.SCH.Services;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

namespace VideoOS.Management.Server;

[ServiceBehavior(Namespace = "urn:videoos:contracts:servicechannel:main:messagecreator:1", InstanceContextMode = InstanceContextMode.PerCall)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class SCHMessageCreatorManager : ServiceChannelServiceV1, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ISCHMessageCreatorManager
{
	public VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.CreateMessagesMsgOut CreateMessages(VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage[] messages, string identityToken)
	{
		InitiateServiceContext();
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.CreateMessagesMsgOut createMessagesMsgOut = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.CreateMessagesMsgOut();
		try
		{
			Message message = new Message();
			createMessagesMsgOut.NoofSessionMessagesCreated = message.CreateMessages(ServiceChannelServiceV1.ConvertToBusMessages(messages));
			createMessagesMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			return createMessagesMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			createMessagesMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return createMessagesMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}
}
