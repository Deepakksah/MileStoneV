using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

namespace VideoOS.SCH.Services.Interfaces;

public abstract class ServiceChannelService
{
	protected void AddErrAndLogBusinessException(Exception ex, string exceptionContext)
	{
		ServiceContext.Current.AddErrAndLog(ex, "Exception occurred while executing the " + exceptionContext + " business logic", LogType.Error);
	}

	protected void AddErrAndLogBusinessException(Exception ex)
	{
		AddErrAndLogBusinessException(ex, GetCallingMethodName());
	}

	protected string GetMethodName()
	{
		return new StackFrame(1).GetMethod().Name;
	}

	protected string GetCallingMethodName()
	{
		return new StackFrame(2).GetMethod().Name;
	}

	protected VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem[] ConvertToContractErrors(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem> busErrors)
	{
		List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem busError in busErrors)
		{
			VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem errorItem = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem();
			errorItem.ErrorNumber = busError.ErrorNumber;
			errorItem.ErrorDescription = ErrorUtil.GetErrorDescription(busError.ErrorNumber, busError.ErrorDescription);
			list.Add(errorItem);
		}
		return list.ToArray();
	}

	protected VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem[] CreateContractErrors(int errNumber, string errDescription)
	{
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem errorItem = new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem();
		errorItem.ErrorNumber = errNumber;
		errorItem.ErrorDescription = ErrorUtil.GetErrorDescription(errNumber, errDescription);
		return new VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem[1] { errorItem };
	}

	protected OperationStatus CreateContractStatus(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem> busErrors, OperationStatusEnum statusCode)
	{
		return new OperationStatus
		{
			Errors = ConvertToContractErrors(busErrors),
			Status = statusCode
		};
	}

	protected OperationStatus CreateContractStatus(int errorNumber, string errorDescription, OperationStatusEnum statusCode)
	{
		return new OperationStatus
		{
			Errors = CreateContractErrors(errorNumber, errorDescription),
			Status = statusCode
		};
	}

	protected virtual OperationStatus InitiateServiceContext()
	{
		try
		{
			ServiceContext.Create(clientIsAnonymous: false, ClientAuthenticationType.UserNamePassword, "", "", new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder), Application.GetMessageLogFileName(), 1);
			ServiceContext.Current.MessageLogEnabled = Application.MessageLogEnabled;
			return null;
		}
		catch (Exception ex)
		{
			new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder).Log(ex, "Exception occurred during service initiation [Authorisation + ServiceContext.Create()] in " + GetCallingMethodName(), LogType.Error);
			return CreateContractStatus(-200003, ex.Message, OperationStatusEnum.Failed);
		}
	}
}
