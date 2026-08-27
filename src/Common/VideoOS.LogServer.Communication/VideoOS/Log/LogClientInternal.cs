using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication;
using VideoOS.LogServer.Communication.Extensions;
using VideoOS.LogServer.Communication.Legacy.Registration;
using VideoOS.LogServer.Communication.Legacy.Registration.Data;
using VideoOS.LogServer.Communication.Log;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.LogServer.Communication.MIP;
using VideoOS.LogServer.Communication.Writers;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Log;

public class LogClientInternal : IDisposable
{
	private ILogWriter _logWriter;

	private readonly ILogger _logger;

	private Uri _logServerUri;

	private readonly object _logWriterLock = new object();

	private bool disposedValue;

	protected string UserId { get; set; }

	protected string UserName { get; set; }

	protected string UserLocation { get; set; }

	private ILogWriter LogWriter
	{
		get
		{
			lock (_logWriterLock)
			{
				if (_logWriter == null)
				{
					if (_logServerUri != null)
					{
						_logWriter = LogWriterFactory.Create(_logger, _logServerUri);
					}
					else
					{
						_logWriter = LogWriterFactory.Create(_logger);
					}
				}
				return _logWriter;
			}
		}
	}

	public LogClientInternal()
	{
		_logger = CreateDefaultLogger();
		UserId = string.Empty;
		UserName = string.Empty;
		UserLocation = string.Empty;
	}

	public LogClientInternal(ILogWriter logWriter, ILogger logger)
	{
		_logger = logger ?? throw new ArgumentException("logger cannot be null");
		_logWriter = logWriter;
	}

	private static ILogger CreateDefaultLogger()
	{
		try
		{
			return Telemetry.GetILogger("LegacyLogClient");
		}
		catch (Exception)
		{
			return (ILogger)(object)new TraceLogger();
		}
	}

	public void DoSetServer(string server)
	{
		if (string.IsNullOrEmpty(server))
		{
			LoggerExtensions.LogError(_logger, "LogClientInternal.DoSetServer() called with null or empty server parameter", Array.Empty<object>());
			return;
		}
		if (server.ToUpperInvariant().Contains("SOAPEXT.DLL?LOGSERVER") || server.ToUpperInvariant().Contains("LOGSERVICE.ASMX"))
		{
			LoggerExtensions.LogError(_logger, "LogClientInternal.DoSetServer() called with no longer supported legacy endpoint server parameter: {0}", new object[1] { server });
			return;
		}
		try
		{
			_logServerUri = new Uri(server);
		}
		catch (UriFormatException ex)
		{
			LoggerExtensions.LogError(_logger, "LogClientInternal.DoSetServer() failed: server={0}, exception={1}", new object[2] { server, ex });
		}
	}

	public bool DoUploadCultureFile(string file)
	{
		if (_logServerUri == null && !EnvironmentWrapper.IsInitialized)
		{
			LoggerExtensions.LogError(_logger, "UploadCultureFile() called without a valid server address or initialized MIP environment.", Array.Empty<object>());
			return false;
		}
		if (string.IsNullOrEmpty(file) || !File.Exists(file))
		{
			return false;
		}
		Task.Run(async delegate
		{
			await DoUploadCultureFileAsync(file);
		});
		return true;
	}

	public Task<bool> DoUploadCultureFileAsync(string file)
	{
		return DoUploadCultureFilesAsync(new string[1] { file });
	}

	public bool DoUploadCultureFiles(string directory, bool recursive)
	{
		if (_logServerUri == null && !EnvironmentWrapper.IsInitialized)
		{
			LoggerExtensions.LogError(_logger, "UploadCultureFiles() called without a valid server address or initialized MIP environment.", Array.Empty<object>());
			return false;
		}
		if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
		{
			return false;
		}
		Task.Run(async delegate
		{
			await DoUploadCultureFilesAsync(directory, recursive);
		});
		return true;
	}

	public Task<bool> DoUploadCultureFilesAsync(string directory, bool recursive)
	{
		string[] files = Directory.GetFiles(directory, "*.lmsg", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		return DoUploadCultureFilesAsync(files);
	}

	private async Task<bool> DoUploadCultureFilesAsync(IEnumerable<string> files)
	{
		List<LegacyMessageXml> list = new List<LegacyMessageXml>();
		foreach (string file in files)
		{
			LegacyMessageXml legacyMessageXml = ReadFile(file);
			if (legacyMessageXml == null)
			{
				return false;
			}
			list.Add(legacyMessageXml);
		}
		try
		{
			return await RegisterMessagesAsync(list);
		}
		catch (CommunicationServiceException)
		{
			return false;
		}
	}

	private LegacyMessageXml ReadFile(string file)
	{
		try
		{
			using FileStream xmlStream = File.OpenRead(file);
			return LegacyMessageXmlParser.ExtractMessagesFromXml(xmlStream);
		}
		catch (FileNotFoundException)
		{
		}
		catch (PathTooLongException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (NotSupportedException)
		{
		}
		catch (IOException)
		{
		}
		catch (LegacyMessageXmlParserException ex6)
		{
			LoggerExtensions.LogError(_logger, "VideoOS.Log.LogClient ReadFile() failed: {0}", new object[1] { ex6 });
		}
		return null;
	}

	private async Task<bool> RegisterMessagesAsync(IEnumerable<LegacyMessageXml> messageXmls)
	{
		bool success = true;
		foreach (LegacyMessageXml messageXml in messageXmls)
		{
			try
			{
				if (!EnvironmentWrapper.IsInitialized)
				{
					await LogWriter.RegisterSourceTypesAsync(messageXml.ToSourceTypes());
					await LogWriter.RegisterCategoriesAsync(messageXml.ToCategories());
					await LogWriter.RegisterLegacyLogMessagesAsync(messageXml.ToLogMessages());
					continue;
				}
				EnvironmentWrapper environmentWrapper = new EnvironmentWrapper();
				if (!environmentWrapper.LoginSettings.IsBasicUser)
				{
					await LogWriter.RegisterSourceTypesAsync(messageXml.ToSourceTypes());
					await LogWriter.RegisterCategoriesAsync(messageXml.ToCategories());
					await LogWriter.RegisterLegacyLogMessagesAsync(messageXml.ToLogMessages());
				}
				else
				{
					await LogWriter.RegisterSourceTypesTokenAsync(environmentWrapper.LoginSettings.Token, messageXml.ToSourceTypes());
					await LogWriter.RegisterCategoriesTokenAsync(environmentWrapper.LoginSettings.Token, messageXml.ToCategories());
					await LogWriter.RegisterLegacyLogMessagesTokenAsync(environmentWrapper.LoginSettings.Token, messageXml.ToLogMessages());
				}
			}
			catch (CommunicationServiceException)
			{
				success = false;
			}
		}
		return success;
	}

	protected void DoLogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string version, string application, string component, string module, string group, string category, Status status, Severity severity, string messageId, string messageText, string messageCulture, Hashtable variables, List<LogMessageUserField> fields)
	{
		if (_logServerUri == null && !EnvironmentWrapper.IsInitialized)
		{
			LoggerExtensions.LogError(_logger, "LogWrite() called without a valid server address or initialized MIP environment.", Array.Empty<object>());
			return;
		}
		Dictionary<string, string> parameters = variables?.Cast<DictionaryEntry>().ToDictionary((DictionaryEntry d) => (string)d.Key, (DictionaryEntry d) => (d.Value as string) ?? string.Empty);
		DoLogWrite(token, messageId, parameters, timestamp, objectName, UserId ?? UserName, UserLocation, severity, fields);
	}

	public void DoLogWrite(string token, string messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, Severity severity, List<LogMessageUserField> fields)
	{
		if (_logServerUri == null && !EnvironmentWrapper.IsInitialized)
		{
			LoggerExtensions.LogError(_logger, "LogWrite() called without a valid server address or initialized MIP environment.", Array.Empty<object>());
			return;
		}
		string ruleName = null;
		string serviceName = null;
		string eventType = null;
		string permissionType = null;
		if (fields != null)
		{
			foreach (LogMessageUserField field in fields)
			{
				switch (field.FieldName?.ToUpperInvariant())
				{
				case "RULENAME":
					ruleName = field.Value;
					break;
				case "SERVICENAME":
					serviceName = field.Value;
					break;
				case "EVENTTYPE":
					eventType = field.Value;
					break;
				case "ACCESSGRANTED":
					permissionType = field.Value;
					break;
				}
			}
		}
		if (string.IsNullOrEmpty(token))
		{
			Task.Run(async delegate
			{
				try
				{
					await LogWriter.WriteLegacyLoglineAsync(messageId, parameters, clientTimestamp.ToUniversalTime(), source, GetUserOrDefault(user), userLocation, serviceName, ruleName, eventType, (LogLevel)severity, StringToPermissionType(permissionType));
				}
				catch (Exception ex)
				{
					LoggerExtensions.LogWarning(_logger, "Exception thrown while trying to write legacy log line. Error: {0}", new object[1] { ex });
				}
			}).ConfigureAwait(continueOnCapturedContext: false);
			return;
		}
		Task.Run(async delegate
		{
			try
			{
				await LogWriter.WriteLegacyLoglineTokenAsync(token, messageId, parameters, clientTimestamp.ToUniversalTime(), source, GetUserOrDefault(user), userLocation, serviceName, ruleName, eventType, (LogLevel)severity, StringToPermissionType(permissionType));
			}
			catch (Exception ex)
			{
				LoggerExtensions.LogWarning(_logger, "Exception thrown while trying to write legacy log line. Error: {0}", new object[1] { ex });
			}
		}).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static PermissionType StringToPermissionType(string permissionString)
	{
		PermissionType result = PermissionType.Granted;
		Enum.TryParse<PermissionType>(permissionString, out result);
		return result;
	}

	private static string GetUserOrDefault(string user)
	{
		if (string.IsNullOrEmpty(user))
		{
			return Environment.UserDomainName + "\\" + Environment.UserName;
		}
		return user;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_logWriter?.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
