using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

namespace VideoOS.Management.Server;

public abstract class ServiceChannelServiceV1 : ServiceChannelService
{
	protected new static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem[] ConvertToContractErrors(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem> busErrors)
	{
		List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem busError in busErrors)
		{
			VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem errorItem = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem();
			errorItem.ErrorNumber = busError.ErrorNumber;
			errorItem.ErrorDescription = busError.ErrorDescription;
			list.Add(errorItem);
		}
		return list.ToArray();
	}

	protected new static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem[] CreateContractErrors(int errNumber, string errDescription)
	{
		VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem errorItem = ServiceContext.CreateError(errNumber, errDescription);
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem errorItem2 = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem();
		errorItem2.ErrorNumber = errorItem.ErrorNumber;
		errorItem2.ErrorDescription = errorItem.ErrorDescription;
		return new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.ErrorItem[1] { errorItem2 };
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus CreateContractStatus(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem> busErrors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum statusCode)
	{
		return new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus
		{
			Errors = ConvertToContractErrors(busErrors),
			Status = statusCode
		};
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus CreateContractStatus(int errorNumber, string errorDescription, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum statusCode)
	{
		return new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus
		{
			Errors = CreateContractErrors(errorNumber, errorDescription),
			Status = statusCode
		};
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Message ConvertToBusMessage(VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage contractMessage)
	{
		Dictionary<string, MessageData> dictionary = new Dictionary<string, MessageData>();
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessageDataItem[] dataItems = contractMessage.DataItems;
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessageDataItem sCHmessageDataItem in dataItems)
		{
			if (dictionary.ContainsKey(sCHmessageDataItem.Key))
			{
				throw new ServiceChannelException(-204000);
			}
			dictionary.Add(sCHmessageDataItem.Key, new MessageData(sCHmessageDataItem.Key, sCHmessageDataItem.Value));
		}
		return new VideoOS.SCH.Services.Business.Definitions.Custom.Message(contractMessage.MessageID, contractMessage.MsgTypeName, contractMessage.Created, dictionary);
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Message[] ConvertToBusMessages(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage> contractMessages)
	{
		if (contractMessages == null)
		{
			return null;
		}
		List<VideoOS.SCH.Services.Business.Definitions.Custom.Message> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Message>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage contractMessage in contractMessages)
		{
			list.Add(ConvertToBusMessage(contractMessage));
		}
		return list.ToArray();
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage ConvertToContractMessage(VideoOS.SCH.Services.Business.Definitions.Custom.Message busMessage)
	{
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage sCHmessage = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage();
		List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessageDataItem> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessageDataItem>();
		foreach (MessageData value in busMessage.Data.Values)
		{
			VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessageDataItem sCHmessageDataItem = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessageDataItem();
			sCHmessageDataItem.Key = value.Key;
			sCHmessageDataItem.Value = value.Value;
			list.Add(sCHmessageDataItem);
		}
		sCHmessage.MessageID = busMessage.ID;
		sCHmessage.MsgTypeName = busMessage.MessageTypeName;
		sCHmessage.Created = busMessage.Created;
		sCHmessage.DataItems = list.ToArray();
		return sCHmessage;
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage[] ConvertToContractMessages(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.Message> busMessages)
	{
		if (busMessages != null && busMessages.Count() > 0)
		{
			List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.SCHmessage>();
			foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Message busMessage in busMessages)
			{
				list.Add(ConvertToContractMessage(busMessage));
			}
			return list.ToArray();
		}
		return null;
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch ConvertToBusMessageWatch(SCHwatchItem contractWatchItem)
	{
		List<MatchExpression> list = new List<MatchExpression>();
		SCHmatchExpression[] matchExpressions = contractWatchItem.MatchExpressions;
		foreach (SCHmatchExpression sCHmatchExpression in matchExpressions)
		{
			list.Add(new MatchExpression(new MessageData(sCHmatchExpression.Data.Key, sCHmatchExpression.Data.Value), sCHmatchExpression.OperatorName));
		}
		if (GuidUtil.GuidTryParse(contractWatchItem.SessionID, out var result))
		{
			return new VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch(result, contractWatchItem.MsgTypeName, list);
		}
		ServiceContext.Current.AddErrAndLog(-204001, "Incoming string ('" + contractWatchItem.SessionID + "') cannot be converted to a guid.", LogType.Error);
		return null;
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch[] ConvertToBusMessageWatchers(IEnumerable<SCHwatchItem> contractWatchItems)
	{
		if (contractWatchItems == null)
		{
			return null;
		}
		List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch>();
		foreach (SCHwatchItem contractWatchItem in contractWatchItems)
		{
			VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatch = ConvertToBusMessageWatch(contractWatchItem);
			if (messageWatch != null)
			{
				list.Add(messageWatch);
			}
		}
		return list.ToArray();
	}

	protected static SCHclient ConvertToContractClient(ServiceChannelClient busClient)
	{
		SCHclient sCHclient = new SCHclient();
		sCHclient.ClientName = busClient.Name;
		if (busClient.MemberGroups != null)
		{
			sCHclient.MemberGroups = busClient.MemberGroups.ToArray();
		}
		return sCHclient;
	}

	protected static SCHclient[] ConvertToContractClients(IEnumerable<ServiceChannelClient> busClients)
	{
		if (busClients == null)
		{
			return null;
		}
		List<SCHclient> list = new List<SCHclient>();
		foreach (ServiceChannelClient busClient in busClients)
		{
			list.Add(ConvertToContractClient(busClient));
		}
		return list.ToArray();
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.MatchOperator ConvertToContractMatchOperator(VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator busOperator)
	{
		return new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.MatchOperator
		{
			Name = busOperator.Name,
			Description = busOperator.Description
		};
	}

	protected static VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.MatchOperator[] ConvertToContractMatchOperators(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator> busOperators)
	{
		if (busOperators == null)
		{
			return null;
		}
		List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.MatchOperator> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.MatchOperator>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator busOperator in busOperators)
		{
			list.Add(ConvertToContractMatchOperator(busOperator));
		}
		return list.ToArray();
	}

	protected new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatus InitiateServiceContext()
	{
		try
		{
			ServiceContext.Create(clientIsAnonymous: false, ClientAuthenticationType.None, "", "", new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder), Application.GetMessageLogFileName(), 1);
			ServiceContext.Current.MessageLogEnabled = Application.MessageLogEnabled;
			return null;
		}
		catch (Exception ex)
		{
			new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder).Log(ex, "Exception occurred during service initiation [Authorisation + ServiceContext.Create()] in " + ServiceChannelService.GetCallingMethodName(), LogType.Error);
			return CreateContractStatus(-200003, ex.Message, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1.OperationStatusEnum.Failed);
		}
	}
}
