using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.SCH.Services;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

namespace VideoOS.Management.Server;

[ServiceBehavior(Namespace = "urn:videoos:contracts:servicechannel:diagnostics", InstanceContextMode = InstanceContextMode.PerCall)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class SCHDiagnosticsManager : ServiceChannelService, ISCHDiagnosticsManager
{
	public PingMsgOut Ping(string pingText)
	{
		InitiateServiceContext();
		PingMsgOut pingMsgOut = new PingMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			pingMsgOut.PingText = diagnostics.Ping(pingText);
			pingMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return pingMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			pingMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return pingMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetServerInfoMsgOut GetServerInfo()
	{
		InitiateServiceContext();
		GetServerInfoMsgOut getServerInfoMsgOut = new GetServerInfoMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getServerInfoMsgOut.InfoDetails = diagnostics.GetServerInfo(out getServerInfoMsgOut.ServerUtcDateTime, out getServerInfoMsgOut.ServerTimeZone, out getServerInfoMsgOut.ServerName, out getServerInfoMsgOut.osVersion, out getServerInfoMsgOut.clrRuntimeVersion);
			getServerInfoMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getServerInfoMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getServerInfoMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getServerInfoMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetServerAppDataMsgOut GetServerAppData()
	{
		InitiateServiceContext();
		GetServerAppDataMsgOut getServerAppDataMsgOut = new GetServerAppDataMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getServerAppDataMsgOut.AppData = diagnostics.GetServerAppData();
			getServerAppDataMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getServerAppDataMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getServerAppDataMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getServerAppDataMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetIdentitiesMsgOut GetIdentities()
	{
		InitiateServiceContext();
		GetIdentitiesMsgOut getIdentitiesMsgOut = new GetIdentitiesMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getIdentitiesMsgOut.Identities = diagnostics.GetIdentities();
			getIdentitiesMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getIdentitiesMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getIdentitiesMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getIdentitiesMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetProcessDetailsMsgOut GetProcessDetails()
	{
		InitiateServiceContext();
		GetProcessDetailsMsgOut getProcessDetailsMsgOut = new GetProcessDetailsMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getProcessDetailsMsgOut.ProcessDetails = diagnostics.GetProcessDetails();
			getProcessDetailsMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getProcessDetailsMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getProcessDetailsMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getProcessDetailsMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetAssemblyInfoMsgOut GetAssemblyInfo()
	{
		InitiateServiceContext();
		GetAssemblyInfoMsgOut getAssemblyInfoMsgOut = new GetAssemblyInfoMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getAssemblyInfoMsgOut.AssemblyInfoArray = diagnostics.GetAssemblyInfo();
			getAssemblyInfoMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getAssemblyInfoMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getAssemblyInfoMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getAssemblyInfoMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetConfigSettingsMsgOut GetConfigSettings()
	{
		InitiateServiceContext();
		GetConfigSettingsMsgOut getConfigSettingsMsgOut = new GetConfigSettingsMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getConfigSettingsMsgOut.ConfigSettings = diagnostics.GetConfigSettings();
			getConfigSettingsMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getConfigSettingsMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getConfigSettingsMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getConfigSettingsMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public TestTextLoggerMsgOut TestTextLogger()
	{
		InitiateServiceContext();
		TestTextLoggerMsgOut testTextLoggerMsgOut = new TestTextLoggerMsgOut();
		try
		{
			testTextLoggerMsgOut.Success = true;
			testTextLoggerMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return testTextLoggerMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			testTextLoggerMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return testTextLoggerMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public TestWindowsEventLoggerMsgOut TestWindowsEventLogger()
	{
		InitiateServiceContext();
		TestWindowsEventLoggerMsgOut testWindowsEventLoggerMsgOut = new TestWindowsEventLoggerMsgOut();
		try
		{
			List<string> list = new List<string>();
			testWindowsEventLoggerMsgOut.Success = true;
			if (list != null)
			{
				testWindowsEventLoggerMsgOut.ResultDetails = list.ToArray();
			}
			testWindowsEventLoggerMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return testWindowsEventLoggerMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			testWindowsEventLoggerMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return testWindowsEventLoggerMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public GetSessionMsgIndicatorDataMsgOut GetSessionMsgIndicatorData()
	{
		InitiateServiceContext();
		GetSessionMsgIndicatorDataMsgOut getSessionMsgIndicatorDataMsgOut = new GetSessionMsgIndicatorDataMsgOut();
		try
		{
			VideoOS.SCH.Services.Business.Components.Diagnostics diagnostics = new VideoOS.SCH.Services.Business.Components.Diagnostics();
			getSessionMsgIndicatorDataMsgOut.CacheData = diagnostics.GetSessionMsgIndicatorData();
			getSessionMsgIndicatorDataMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed);
			return getSessionMsgIndicatorDataMsgOut;
		}
		catch (Exception ex)
		{
			ServiceChannelService.AddErrAndLogBusinessException(ex);
			getSessionMsgIndicatorDataMsgOut.CurrentStatus = ServiceChannelService.CreateContractStatus(ServiceContext.Current.Errors, VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Failed);
			return getSessionMsgIndicatorDataMsgOut;
		}
		finally
		{
			ServiceContext.Delete();
		}
	}
}
