using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public static class StatusMessageProvider
{
	public static string GetMessage(RegistrationResult registrationResult, object successResult, ILogger logger, MessageSource tabPage)
	{
		if (registrationResult == null)
		{
			throw new ArgumentNullException("registrationResult");
		}
		string message = GetMessage(registrationResult, successResult, registrationResult.ResultType, logger, tabPage);
		if (string.IsNullOrEmpty(message))
		{
			string entry = FormattableString.Invariant($"Unexpected registration results: {Environment.NewLine} {string.Join(Environment.NewLine, registrationResult.Results.Select((KeyValuePair<Type, object> x) => FormattableString.Invariant($"Status Type : {x.Key}, Status Value{x.Value}")))}");
			logger.LogError(entry);
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_UnexpectedError, logger.LogFilePath);
		}
		return message;
	}

	public static string GetMessage(RegistrationResult registrationResult, object successResult, RegistrationResultType resultType, ILogger logger, MessageSource tabPage)
	{
		if (registrationResult == null)
		{
			throw new ArgumentNullException("registrationResult");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (resultType == RegistrationResultType.Success)
		{
			return GetSuccessMessage(registrationResult, successResult, logger, tabPage);
		}
		ApplyStatus? result = registrationResult.GetResult<ApplyStatus>();
		if (resultType == result?.ResultType())
		{
			return GetApplyCertificateMessage(result.Value, logger);
		}
		SystemConfigurationStatus? result2 = registrationResult.GetResult<SystemConfigurationStatus>();
		if (resultType == result2?.ResultType())
		{
			return GetConfigureSystemMessage(result2.Value, logger);
		}
		RegisterClientStatus? result3 = registrationResult.GetResult<RegisterClientStatus>();
		if (resultType == result3?.ResultType())
		{
			return GetRegisterClientMessage(result3.Value, logger, tabPage);
		}
		RegisterServiceStatus? result4 = registrationResult.GetResult<RegisterServiceStatus>();
		if (resultType == result4?.ResultType())
		{
			return GetRegisterServiceMessage(result4.Value, logger, tabPage);
		}
		SetServerAddressStatus? result5 = registrationResult.GetResult<SetServerAddressStatus>();
		if (resultType == result5?.ResultType())
		{
			return GetSetServerAddressMessage(result5.Value, logger, tabPage);
		}
		ConsistencyStatus? result6 = registrationResult.GetResult<ConsistencyStatus>();
		if (resultType == result6?.ResultType())
		{
			return GetConsistencyStatusMessage(result6.Value, logger);
		}
		if (resultType == registrationResult.GetResult<SetSqlServerStatus>()?.ResultType())
		{
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_UnexpectedError, logger.LogFilePath);
		}
		return string.Empty;
	}

	private static string GetSuccessMessage(RegistrationResult registrationResult, object successResult, ILogger logger, MessageSource tabPage)
	{
		if (registrationResult.ResultType.SuccessOrWarning())
		{
			if (successResult != null)
			{
				if (successResult is ApplyStatus)
				{
					if ((ApplyStatus)successResult == ApplyStatus.Success)
					{
						return GetApplyCertificateMessage(ApplyStatus.Success, logger);
					}
				}
				else if (successResult is SystemConfigurationStatus)
				{
					if ((SystemConfigurationStatus)successResult == SystemConfigurationStatus.Success)
					{
						return GetConfigureSystemMessage(SystemConfigurationStatus.Success, logger);
					}
				}
				else if (successResult is RegisterClientStatus)
				{
					if ((RegisterClientStatus)successResult == RegisterClientStatus.Success)
					{
						return GetRegisterClientMessage(RegisterClientStatus.Success, logger, tabPage);
					}
				}
				else if (successResult is RegisterServiceStatus)
				{
					if ((RegisterServiceStatus)successResult == RegisterServiceStatus.Success)
					{
						return GetRegisterServiceMessage(RegisterServiceStatus.Success, logger, tabPage);
					}
				}
				else if (successResult is SetServerAddressStatus)
				{
					if ((SetServerAddressStatus)successResult == SetServerAddressStatus.Success)
					{
						return GetSetServerAddressMessage(SetServerAddressStatus.Success, logger, tabPage);
					}
				}
				else if (successResult is ConsistencyStatus)
				{
					if ((ConsistencyStatus)successResult == ConsistencyStatus.Success)
					{
						return GetConsistencyStatusMessage(ConsistencyStatus.Success, logger);
					}
				}
				else if (successResult is SetSqlServerStatus && (SetSqlServerStatus)successResult == SetSqlServerStatus.Success)
				{
					return Resources.GeneralStatus_Success;
				}
				return string.Empty;
			}
			return Resources.GeneralStatus_Success;
		}
		return string.Empty;
	}

	private static string GetConsistencyStatusMessage(ConsistencyStatus consistencyStatus, ILogger logger)
	{
		return consistencyStatus switch
		{
			ConsistencyStatus.Success => Resources.RegisterClientStatus_Success, 
			ConsistencyStatus.ClientEndpointSecuritySchemeDoesNotMatchServerEndpoint => Resources.ConsistencyStatus_SecuritySchemeDoesNotMatch, 
			ConsistencyStatus.ServerCertificateIsNotTrusted => Resources.ConsistencyStatus_ServerCertificateIsNotTrusted, 
			_ => string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath), 
		};
	}

	private static string GetConfigureSystemMessage(SystemConfigurationStatus systemConfigurationStatus, ILogger logger)
	{
		return systemConfigurationStatus switch
		{
			SystemConfigurationStatus.Success => Resources.RegisterClientStatus_Success, 
			SystemConfigurationStatus.UnspecifiedError => string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath), 
			SystemConfigurationStatus.InsufficientCredentials => Resources.RegisterClientStatus_InsufficientCredentials, 
			SystemConfigurationStatus.AuthorizationServerNotFound => Resources.RegisterClientStatus_ManagementServerNotFound, 
			_ => string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath), 
		};
	}

	private static string GetSetServerAddressMessage(SetServerAddressStatus setServerAddressStatus, ILogger logger, MessageSource tabPage)
	{
		switch (setServerAddressStatus)
		{
		case SetServerAddressStatus.Success:
			return Resources.RegisterClientStatus_Success;
		case SetServerAddressStatus.UnspecifiedError:
			return string.Format(CultureInfo.CurrentCulture, Resources.SetServerAddress_UnspecifiedError, logger.LogFilePath);
		case SetServerAddressStatus.CouldNotStopServer:
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStopServer, logger.LogFilePath);
		case SetServerAddressStatus.CouldNotStartServer:
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStartServer, logger.LogFilePath);
		case SetServerAddressStatus.ManagementServerNotFound:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessRegistrationServerNotFound;
			}
			return Resources.RegisterClientStatus_ManagementServerNotFound;
		case SetServerAddressStatus.InsufficientCredentials:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessInsufficientCredentials;
			}
			return Resources.RegisterClientStatus_InsufficientCredentials;
		case SetServerAddressStatus.SetServerAddressNotAllowedWhenManagementServerIsLocal:
			return Resources.SetServerAddress_NotAllowedWhenManagementServerIsLocal;
		default:
			return string.Format(CultureInfo.CurrentCulture, Resources.SetServerAddress_UnspecifiedError, logger.LogFilePath);
		}
	}

	private static string GetRegisterServiceMessage(RegisterServiceStatus registerServiceResult, ILogger logger, MessageSource tabPage)
	{
		switch (registerServiceResult)
		{
		case RegisterServiceStatus.Success:
			return Resources.RegisterClientStatus_Success;
		case RegisterServiceStatus.UnspecifiedError:
			if (tabPage == MessageSource.Encryption)
			{
				return string.Format(CultureInfo.CurrentCulture, Resources.ApplyStatus_SuccessUnableToRegister, logger.LogFilePath);
			}
			return string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath);
		case RegisterServiceStatus.InsufficientCredentials:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessInsufficientCredentials;
			}
			return Resources.RegisterClientStatus_InsufficientCredentials;
		case RegisterServiceStatus.ManagementServerNotFound:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessRegistrationServerNotFound;
			}
			return Resources.RegisterClientStatus_ManagementServerNotFound;
		case RegisterServiceStatus.CouldNotStopServer:
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStopServer, logger.LogFilePath);
		case RegisterServiceStatus.CouldNotStartServer:
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStartServer, logger.LogFilePath);
		case RegisterServiceStatus.ServiceRegistrationNotFound:
			return string.Format(CultureInfo.CurrentCulture, Resources.RegisterService_ServiceRegistrationNotFound, logger.LogFilePath);
		default:
			if (tabPage == MessageSource.Encryption)
			{
				return string.Format(CultureInfo.CurrentCulture, Resources.ApplyStatus_SuccessUnableToRegister, logger.LogFilePath);
			}
			return string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath);
		}
	}

	private static string GetRegisterClientMessage(RegisterClientStatus registerClientResult, ILogger logger, MessageSource tabPage)
	{
		switch (registerClientResult)
		{
		case RegisterClientStatus.Success:
			return Resources.RegisterClientStatus_Success;
		case RegisterClientStatus.UnspecifiedError:
			if (tabPage == MessageSource.Encryption)
			{
				return string.Format(CultureInfo.CurrentCulture, Resources.ApplyStatus_SuccessUnableToRegister, logger.LogFilePath);
			}
			return string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath);
		case RegisterClientStatus.InsufficientCredentials:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessInsufficientCredentials;
			}
			return Resources.RegisterClientStatus_InsufficientCredentials;
		case RegisterClientStatus.InvalidCredentials:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessInvalidCredentials;
			}
			return Resources.RegisterClientStatus_InvalidCredentials;
		case RegisterClientStatus.AuthorizationServerNotFound:
			if (tabPage == MessageSource.Encryption)
			{
				return Resources.ApplyStatus_SuccessRegistrationServerNotFound;
			}
			return Resources.RegisterClientStatus_ManagementServerNotFound;
		case RegisterClientStatus.CouldNotStopServer:
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStopServer, logger.LogFilePath);
		case RegisterClientStatus.CouldNotStartServer:
			return string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStartServer, logger.LogFilePath);
		default:
			if (tabPage == MessageSource.Encryption)
			{
				return string.Format(CultureInfo.CurrentCulture, Resources.ApplyStatus_SuccessUnableToRegister, logger.LogFilePath);
			}
			return string.Format(CultureInfo.CurrentCulture, Resources.RegisterClientStatus_UnspecifiedError, logger.LogFilePath);
		}
	}

	private static string GetApplyCertificateMessage(ApplyStatus applyResult, ILogger logger)
	{
		return applyResult switch
		{
			ApplyStatus.Success => Resources.ApplyStatus_Success, 
			ApplyStatus.UnspecifiedError => string.Format(CultureInfo.CurrentCulture, Resources.ApplyStatus_UnspecifiedError, logger.LogFilePath), 
			ApplyStatus.CouldNotStopServer => string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStopServer, logger.LogFilePath), 
			ApplyStatus.CouldNotStartServer => string.Format(CultureInfo.CurrentCulture, Resources.GeneralStatus_CouldNotStartServer, logger.LogFilePath), 
			_ => string.Format(CultureInfo.CurrentCulture, Resources.ApplyStatus_UnspecifiedError, logger.LogFilePath), 
		};
	}
}
