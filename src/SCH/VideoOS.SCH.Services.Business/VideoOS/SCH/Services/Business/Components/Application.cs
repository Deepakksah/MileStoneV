using System;
using System.IO;
using VideoOS.SCH.Services.DataAccess;

namespace VideoOS.SCH.Services.Business.Components;

public class Application
{
	public enum ServiceFeature
	{
		NoPermissionFeature,
		CreateMessages,
		BeginSession,
		EndSession,
		AddMessageWatch,
		AddMatchExpressions,
		RemoveMessageWatch,
		RemoveMatchExpressions,
		ClearMessageWatchers,
		CheckForMessages,
		GetConnectedClients,
		GetAllMatchOperators,
		DiagnosisPing,
		DiagnosisGetServerInfo,
		DiagnosisGetServerAppData,
		DiagnosisGetIdentityTokenCacheData,
		DiagnosisGetIdentities,
		DiagnosisGetProcessDetails,
		DiagnosisGetAssemblyInfo,
		DiagnosisGetConfigSettings,
		DiagnosisTestTextLogger,
		DiagnosisTestWindowsEventLogger,
		DiagnosisClearIdentityTokenCache,
		BatchSessionCleanup,
		BatchMessageCleanup,
		BatchGetBatchJobDefinitions,
		BatchRegisterThisServiceChannel,
		DiagnosisGetSessionMsgIndicatorData
	}

	public const string ServerLogConfigName = "ServiceChannelServerLog";

	public const string MessageLogFileNameDefault = "~\\Logs\\ServiceChannelMessages_%yyMMdd_HH%.log";

	public const int MaxMsgTypeNameLength = 30;

	public const int MaxMsgDataValueLength = 50;

	public const int MaxMsgDataKeyLength = 25;

	public const int MaxSessionClientNameLength = 50;

	public const int MaxSessionClientUserNameLength = 50;

	public static string ServerLogFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ConfigSettings.CompanyName), ConfigSettings.InstallDirectory);

	public static bool MessageLogEnabled => ConfigSettings.MessageLogEnabled;

	public static string EventSourceName => ConfigSettings.WinEventLogSourceName;

	private static string ReplaceFolderToken(string fileName)
	{
		string text = fileName;
		if (text.IndexOf("~") == 0)
		{
			text = text.TrimStart('~').TrimStart('\\');
			text = Path.Combine(ServerLogFolder, text);
		}
		return text;
	}

	public static string GetMessageLogFileName()
	{
		string text = ConfigSettings.MessageLogFileName;
		if (string.IsNullOrEmpty(text))
		{
			text = "~\\Logs\\ServiceChannelMessages_%yyMMdd_HH%.log";
		}
		return ReplaceFolderToken(text);
	}

	public static string GetServerName()
	{
		return Environment.MachineName;
	}
}
