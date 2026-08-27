using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

namespace VideoOS.Management.Server;

[ServiceBehavior(Namespace = "urn:videoos:contracts:servicechannel:main:messagewatch:1", InstanceContextMode = InstanceContextMode.PerCall)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class SCHMessageWatchManager : ServiceChannelServiceV1, ISCHMessageWatchManager
{
	public BeginSessionMsgOut BeginSession(string clientName, string identityToken)
	{
		InitiateServiceContext();
		BeginSessionMsgOut beginSessionMsgOut = new BeginSessionMsgOut();
		try
		{
			if (!new Session().BeginSession(clientName, out var sessionID, out var recommendedMsgChkInterval))
			{
				beginSessionMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
				return beginSessionMsgOut;
			}
			beginSessionMsgOut.SessionID = sessionID.ToString();
			beginSessionMsgOut.RecommendedMsgChkIntervalInMs = recommendedMsgChkInterval;
			beginSessionMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			return beginSessionMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			beginSessionMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return beginSessionMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus EndSession(string sessionID, string identityToken)
	{
		InitiateServiceContext();
		try
		{
			if (GuidUtil.GuidTryParse(sessionID, out var result))
			{
				if (new Session().EndSession(result))
				{
					return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
				}
				return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			}
			ServiceContext.Current.AddErrAndLog(-204001, "Incoming string ('" + sessionID + "') cannot be converted to a guid.", LogType.Error);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus AddMessageWatch(string sessionID, string messageTypeName, string identityToken)
	{
		InitiateServiceContext();
		try
		{
			if (GuidUtil.GuidTryParse(sessionID, out var result))
			{
				new MessageWatch().AddMessageWatch(result, messageTypeName);
				return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			}
			ServiceContext.Current.AddErrAndLog(-204001, "Incoming string ('" + sessionID + "') cannot be converted to a guid.", LogType.Error);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public AddMatchExpressionsMsgOut AddMatchExpressions(SCHwatchItem[] watchItems, string identityToken)
	{
		InitiateServiceContext();
		AddMatchExpressionsMsgOut addMatchExpressionsMsgOut = new AddMatchExpressionsMsgOut();
		try
		{
			MessageWatch messageWatch = new MessageWatch();
			addMatchExpressionsMsgOut.NoofExpressionsCreated = messageWatch.AddMatchExpressions(ServiceChannelServiceV1.ConvertToBusMessageWatchers(watchItems));
			addMatchExpressionsMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			return addMatchExpressionsMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			addMatchExpressionsMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return addMatchExpressionsMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus RemoveMessageWatch(string sessionID, string messageTypeName, string identityToken)
	{
		InitiateServiceContext();
		try
		{
			if (GuidUtil.GuidTryParse(sessionID, out var result))
			{
				new MessageWatch().RemoveMessageWatch(result, messageTypeName);
				return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			}
			ServiceContext.Current.AddErrAndLog(-204001, "Incoming string ('" + sessionID + "') cannot be converted to a guid.", LogType.Error);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus RemoveMatchExpressions(SCHwatchItem[] watchItems, string identityToken)
	{
		InitiateServiceContext();
		try
		{
			new MessageWatch().RemoveMatchExpressions(ServiceChannelServiceV1.ConvertToBusMessageWatchers(watchItems));
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus ClearMessageWatchers(string sessionID, string identityToken)
	{
		InitiateServiceContext();
		try
		{
			if (GuidUtil.GuidTryParse(sessionID, out var result))
			{
				new MessageWatch().RemoveMessageWatchers(result);
				return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
			}
			ServiceContext.Current.AddErrAndLog(-204001, "Incoming string ('" + sessionID + "') cannot be converted to a guid.", LogType.Error);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			return ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public CheckForMessagesMsgOut CheckForMessages(string sessionID, int millisecsToWaitForMsgs, string identityToken)
	{
		InitiateServiceContext();
		CheckForMessagesMsgOut checkForMessagesMsgOut = new CheckForMessagesMsgOut();
		try
		{
			if (GuidUtil.GuidTryParse(sessionID, out var result))
			{
				Message message = new Message();
				checkForMessagesMsgOut.Messages = ServiceChannelServiceV1.ConvertToContractMessages(message.CheckForMessages(result, millisecsToWaitForMsgs));
				checkForMessagesMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Completed);
				return checkForMessagesMsgOut;
			}
			ServiceContext.Current.AddErrAndLog(-204001, "Incoming string ('" + sessionID + "') cannot be converted to a guid.", LogType.Error);
			checkForMessagesMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return checkForMessagesMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			checkForMessagesMsgOut.CurrentStatus = ServiceChannelServiceV1.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
			return checkForMessagesMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}
}
