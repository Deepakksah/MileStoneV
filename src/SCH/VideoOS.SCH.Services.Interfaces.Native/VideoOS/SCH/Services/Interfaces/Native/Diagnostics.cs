using System;
using System.Collections.Generic;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

namespace VideoOS.SCH.Services.Interfaces.Native;

public sealed class Diagnostics : ServiceChannelServiceNative
{
	public Diagnostics()
	{
		InitiateServiceContext();
	}

	public DiagSimpleKVPair[] GetServerInfo(string identityToken)
	{
		try
		{
			DateTime serverUtcDateTime;
			string serverTimeZone;
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetServerInfo(out serverUtcDateTime, out serverTimeZone, out serverTimeZone, out serverTimeZone, out serverTimeZone);
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public DiagSimpleKVPair[] GetIdentities(string identityToken)
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetIdentities();
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public DiagServerAppData GetServerAppData(string identityToken)
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetServerAppData();
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public DiagSessionMsgIndicatorData GetSessionMsgIndicatorData()
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetSessionMsgIndicatorData();
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public DiagSimpleKVPair[] GetProcessDetails()
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetProcessDetails();
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public DiagAssemblyInfo[] GetAssemblyInfo()
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetAssemblyInfo();
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public DiagSimpleKVPair[] GetConfigSettings()
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().GetConfigSettings();
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public bool TestTextLogger(out List<string> resultDetails)
	{
		try
		{
			return new VideoOS.SCH.Services.Business.Components.Diagnostics().TestTextLogger(out resultDetails);
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			throw new ServiceChannelException("Exception occurred in service method " + GetMethodName(), ConvertToExceptionErrors(ServiceContext.Current.Errors), ex);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}
}
