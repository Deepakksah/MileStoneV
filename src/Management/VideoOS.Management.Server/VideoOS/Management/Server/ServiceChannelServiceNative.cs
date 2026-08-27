using System;
using System.Collections.Generic;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

namespace VideoOS.Management.Server;

public class ServiceChannelServiceNative : ServiceChannelService
{
	protected static List<ServiceChannelException.ErrorItem> ConvertToExceptionErrors(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem> busErrors)
	{
		List<ServiceChannelException.ErrorItem> list = new List<ServiceChannelException.ErrorItem>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.ErrorItem busError in busErrors)
		{
			list.Add(new ServiceChannelException.ErrorItem(busError.ErrorNumber, busError.ErrorDescription));
		}
		return list;
	}

	protected static List<ServiceChannelException.ErrorItem> ConvertToExceptionErrors(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem> contractErrors)
	{
		List<ServiceChannelException.ErrorItem> list = new List<ServiceChannelException.ErrorItem>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.ErrorItem contractError in contractErrors)
		{
			list.Add(new ServiceChannelException.ErrorItem(contractError.ErrorNumber, contractError.ErrorDescription));
		}
		return list;
	}

	protected override VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatus InitiateServiceContext()
	{
		try
		{
			ServiceContext.Create(clientIsAnonymous: false, ClientAuthenticationType.UserNamePassword, "", "", new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder), Application.GetMessageLogFileName(), 1);
			ServiceContext.Current.MessageLogEnabled = Application.MessageLogEnabled;
			return null;
		}
		catch (Exception ex)
		{
			new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder).Log(ex, "Exception occurred during service initiation [Authorisation + ServiceContext.Create()] in " + ServiceChannelService.GetCallingMethodName(), LogType.Error);
			return ServiceChannelService.CreateContractStatus(-200003, ex.Message, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
		}
	}
}
