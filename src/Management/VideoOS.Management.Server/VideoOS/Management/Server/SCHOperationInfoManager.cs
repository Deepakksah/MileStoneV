using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.SCH.Services;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

namespace VideoOS.Management.Server;

[ServiceBehavior(Namespace = "urn:videoos:contracts:servicechannel:main:operationinfo:1", InstanceContextMode = InstanceContextMode.PerCall)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class SCHOperationInfoManager : ServiceChannelServiceV1, ISCHOperationInfoManager
{
	public GetConnectedClientsMsgOut GetConnectedClients(string identityToken)
	{
		InitiateServiceContext();
		GetConnectedClientsMsgOut getConnectedClientsMsgOut = new GetConnectedClientsMsgOut();
		try
		{
			Session session = new Session();
			getConnectedClientsMsgOut.Clients = ServiceChannelServiceV1.ConvertToContractClients(session.GetAllClients());
			getConnectedClientsMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			return getConnectedClientsMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getConnectedClientsMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return getConnectedClientsMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetAllMatchOperatorsMsgOut GetAllMatchOperators(string identityToken)
	{
		InitiateServiceContext();
		GetAllMatchOperatorsMsgOut getAllMatchOperatorsMsgOut = new GetAllMatchOperatorsMsgOut();
		try
		{
			MessageWatch messageWatch = new MessageWatch();
			getAllMatchOperatorsMsgOut.MatchOperators = ServiceChannelServiceV1.ConvertToContractMatchOperators(messageWatch.GetMatchOperators());
			getAllMatchOperatorsMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			return getAllMatchOperatorsMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getAllMatchOperatorsMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return getAllMatchOperatorsMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}
}
