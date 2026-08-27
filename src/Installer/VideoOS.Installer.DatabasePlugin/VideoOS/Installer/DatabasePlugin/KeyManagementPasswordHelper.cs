using System;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.Security;

namespace VideoOS.Installer.DatabasePlugin;

public static class KeyManagementPasswordHelper
{
	public static SystemPasswordMode GetSystemPasswordMode(IEncryptionKeyReader encryptionKeyReader, IGlobalData globalData, ILogger logger)
	{
		if (encryptionKeyReader == null)
		{
			throw new ArgumentNullException("encryptionKeyReader");
		}
		if (globalData == null)
		{
			throw new ArgumentNullException("globalData");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		bool flag = true;
		bool flag2 = false;
		if (globalData.TryGetValue("SQL-KEEP-DATA", out var value))
		{
			flag = string.Equals("yes", value, StringComparison.OrdinalIgnoreCase);
		}
		if (globalData.TryGetValue("SQL-CREATE-DATABASE", out value))
		{
			flag2 = string.Equals("yes", value, StringComparison.OrdinalIgnoreCase);
		}
		if (!flag || flag2)
		{
			return SystemPasswordMode.CreateNewPassword;
		}
		switch (encryptionKeyReader.GetKeyManagementState())
		{
		case KeyManagementState.PasswordConsistent:
			LoggerExtensions.LogInformation(logger, FormattableString.Invariant($"System password mode: {SystemPasswordMode.NoPasswordRequired}"), Array.Empty<object>());
			return SystemPasswordMode.NoPasswordRequired;
		case KeyManagementState.PasswordInconsistent:
			LoggerExtensions.LogInformation(logger, FormattableString.Invariant($"System password mode: {SystemPasswordMode.EnterExistingPassword}"), Array.Empty<object>());
			return SystemPasswordMode.EnterExistingPassword;
		case KeyManagementState.KeyMissing:
			LoggerExtensions.LogInformation(logger, FormattableString.Invariant($"System password mode: {SystemPasswordMode.CreateNewPassword}"), Array.Empty<object>());
			return SystemPasswordMode.CreateNewPassword;
		default:
			throw new KeyStorageException(KeyStorageErrorCode.UnknownError, "KeyManager returned unknown state.");
		}
	}

	public static string VerifyExistingPassword(IEncryptionKeyReader encryptionKeyReader, SecureString password, bool acceptEmptyPassword, ILogger logger)
	{
		if (encryptionKeyReader == null)
		{
			throw new ArgumentNullException("encryptionKeyReader");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (password == null)
		{
			LoggerExtensions.LogError(logger, FormattableString.Invariant($"Password is null. Returning KeyManagementErrorIncorrectPassword error message."), Array.Empty<object>());
			return Resources.KeyManagementErrorIncorrectPassword;
		}
		if (password.Length == 0 && !acceptEmptyPassword)
		{
			LoggerExtensions.LogError(logger, FormattableString.Invariant($"Password is 0 length where not allowed. Returning KeyManagementErrorIncorrectPassword error message."), Array.Empty<object>());
			return Resources.KeyManagementErrorIncorrectPassword;
		}
		bool flag;
		try
		{
			flag = encryptionKeyReader.VerifyPassword(password.DecryptSecureString());
		}
		catch (KeyStorageException ex) when (ex.ErrorType == KeyStorageErrorCode.KeyIsMissingFromDatabaseOnRead)
		{
			LoggerExtensions.LogError(logger, FormattableString.Invariant($"Key is missing. Returning KeyManagementErrorIncorrectPassword error message."), Array.Empty<object>());
			throw;
		}
		catch (KeyStorageException ex2)
		{
			LoggerExtensions.LogError(logger, FormattableString.Invariant(FormattableStringFactory.Create("{0} failed. KeyStorage error code = {1}, exception was {2}", "VerifyExistingPassword", ex2.ErrorType, ex2)), Array.Empty<object>());
			throw;
		}
		if (!flag)
		{
			return Resources.KeyManagementErrorIncorrectPassword;
		}
		return string.Empty;
	}

	public static IEncryptionKeyReader CreateKeyManager(ILogger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (!GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var value))
		{
			LoggerExtensions.LogWarning(logger, "Connection string for MS is not provided", Array.Empty<object>());
		}
		if (string.IsNullOrEmpty(value) && !GlobalData.TryGetValue("SQL_CONNECTION_STRING_ES", out value))
		{
			LoggerExtensions.LogWarning(logger, "Connection string for ES is not provided", Array.Empty<object>());
		}
		if (string.IsNullOrEmpty(value) && !GlobalData.TryGetValue("SQL_CONNECTION_STRING_LOG", out value))
		{
			LoggerExtensions.LogWarning(logger, "Connection string for Log Server is not provided", Array.Empty<object>());
			throw new InvalidOperationException("No SQL connection string defined for Management Server, Event Server or Log Server");
		}
		IDatabaseConnectionInfo databaseConnectionInfo = new DatabaseConnectionInfo(value);
		return KeyManagerFactory.GetEncryptionKeyReader(GlobalData.OemHelper.CompanyName, new SqlConnectionManager(databaseConnectionInfo, logger), logger);
	}
}
