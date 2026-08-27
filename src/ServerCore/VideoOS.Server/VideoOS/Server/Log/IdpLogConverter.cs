using System;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.Server.Log;

public sealed class IdpLogConverter : IIdpLogConverter
{
	public LogEntry ConvertIdpLog(EventLogModel idpLog, string idpServerName)
	{
		if (idpLog == null)
		{
			throw new ArgumentNullException("idpLog");
		}
		if (idpServerName == null)
		{
			throw new ArgumentNullException("idpServerName");
		}
		if (!Guid.TryParse(idpLog.ObjectId, out var result))
		{
			result = Guid.Empty;
		}
		LogEntry logEntry = null;
		string credentials = GetCredentials(idpLog);
		switch (idpLog.Event)
		{
		case "RegistrationSuccess":
			logEntry = LogEntryAuditFactory.AuditIdentityProviderRegistrationSuccess(idpServerName, idpLog.ObjectId, idpLog.ObjectName, credentials, idpLog.RemoteIpAddress);
			break;
		case "RegistrationChanged":
			logEntry = LogEntryAuditFactory.AuditIdentityProviderRegistrationChanged(idpServerName, idpLog.ObjectId, idpLog.ObjectName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserNotAdministrator":
			logEntry = LogEntryAuditFactory.AuditIdentityProviderAdministratorLoginFailed(result, idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserChangePasswordSuccess":
			logEntry = LogEntryAuditFactory.AuditBasicUserChangeOwnPassword(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLoginSuccess":
			logEntry = LogEntryAuditFactory.AuditUserLoggedIn(result, idpServerName, idpLog.UserName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLogoutSuccess":
			logEntry = LogEntryAuditFactory.AuditUserLoggedOut(result, idpServerName, idpLog.UserName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLoginFailedNeedToChangePassword":
			logEntry = LogEntryAuditFactory.AuditUserLoginFailedPasswordChange(result.ToString(), idpLog.UserName, idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLockedOut":
			logEntry = LogEntryAuditFactory.AuditBasicUserSystemLockedOut(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLockedOutCleared":
			logEntry = LogEntryAuditFactory.AuditBasicUserLockedOutCleared(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserNeedPasswordChange":
			logEntry = LogEntryAuditFactory.AuditBasicUserForcePasswordChangeIdentityProvider(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress.ToString());
			break;
		case "UserNeedPasswordChangeCleared":
			logEntry = LogEntryAuditFactory.AuditBasicUserForcePasswordChangeCleared(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress.ToString());
			break;
		case "UserLoginFailed":
			logEntry = LogEntryAuditFactory.AuditUserLoginFailed(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLoginFailedUserNotActive":
			logEntry = LogEntryAuditFactory.AuditUserLoginFailedLockedOutByAdmin(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserLoginFailedUnknownUser":
			logEntry = LogEntryAuditFactory.AuditUserLoginFailedUnknownUser(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserDescriptionChanged":
			logEntry = LogEntryAuditFactory.AuditBasicUserDescriptionChanged(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserClaimsUpdated":
			logEntry = new LogEntrySystem(LogEntryId.AutoProvisionedUserUpdated, result, SourceType.IdentityProvider, idpServerName, idpLog.ObjectName.ToUpperInvariant());
			if (logEntry.LogLevel == LogLevel.Undefined)
			{
				logEntry.LogLevel = LogManagerBase.GetLogEntryDefinition((LogEntryId)logEntry.LogId).LogLevel;
			}
			break;
		case "UserCreated":
			logEntry = LogEntryAuditFactory.AuditBasicUserCreatedIdentityProvider(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			if (logEntry.LogLevel == LogLevel.Undefined)
			{
				logEntry.LogLevel = LogManagerBase.GetLogEntryDefinition((LogEntryId)logEntry.LogId).LogLevel;
			}
			break;
		case "UserDeleted":
			logEntry = LogEntryAuditFactory.AuditBasicUserDeletedIdentityProvider(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserEnabled":
			logEntry = LogEntryAuditFactory.AuditBasicUserEnabledIdentityProvider(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		case "UserDisabled":
			logEntry = LogEntryAuditFactory.AuditBasicUserDisabled(result.ToString(), idpLog.ObjectName.ToUpperInvariant(), idpServerName, credentials, idpLog.RemoteIpAddress);
			break;
		default:
			throw new ArgumentException("Unknown idp event type.", "idpLog");
		}
		if (logEntry != null)
		{
			logEntry.TimeStampLocal = idpLog.TimeStamp.DateTime.ToLocalTime();
			logEntry.TimeStampUtc = logEntry.TimeStampLocal.ToUniversalTime();
		}
		return logEntry;
	}

	private static string GetCredentials(EventLogModel idpLog)
	{
		if (!string.IsNullOrEmpty(idpLog.UserName))
		{
			return FormatUserName(idpLog.UserName.Trim());
		}
		if (!string.IsNullOrEmpty(idpLog.ClientId))
		{
			return FormatClientName(idpLog.ClientId.Trim(), idpLog.ClientName?.Trim());
		}
		return "[System]";
	}

	private static string FormatUserName(string value)
	{
		string[] array = value?.Split('\\');
		if (array != null && array.Length > 1)
		{
			return value?.ToUpperInvariant();
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", "[BASIC]", value?.ToUpperInvariant());
	}

	private static string FormatClientName(string clientId, string clientName)
	{
		if (string.IsNullOrEmpty(clientName))
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", "[Client]", clientId);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}\\{1}@{2}", "[Client]", clientName, clientId);
	}
}
