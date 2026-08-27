using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.LogServer.Communication.Writers;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Logger.AuditLog;

public sealed class AuditLogger : IAuditLogger
{
	private const string AuditLogMessageCategoryId = "Incidents";

	private readonly ILogger _logger;

	private readonly IStringResourceReader _resourceReader;

	private ILogWriter _logWriter;

	private bool _messagesRegistered;

	public AuditLogger(ILogger logger)
		: this(logger, new StringResourceReader())
	{
	}

	public AuditLogger([NotNull] ILogger logger, [NotNull] IStringResourceReader resourceReader)
	{
		logger.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Logger\\AuditLog\\AuditLogger.cs", 32);
		resourceReader.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Logger\\AuditLog\\AuditLogger.cs", 33);
		_logger = logger;
		_resourceReader = resourceReader;
	}

	public void Initialize([NotNull] Uri logServerUri)
	{
		logServerUri.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Logger\\AuditLog\\AuditLogger.cs", 42);
		Initialize(LogWriterFactory.Create(_logger, logServerUri));
	}

	public void Initialize([NotNull] ILogWriter logWriter)
	{
		logWriter.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Logger\\AuditLog\\AuditLogger.cs", 52);
		_logWriter = logWriter;
	}

	public async Task WriteAuditLogEntryAsync(Guid messageId, IDictionary<string, string> parameters, string source, string user, string userLocation)
	{
		if (_logWriter == null)
		{
			_logger.LogDebug($"Cannot add audit log entry for message {messageId} because Log Server is not registered.");
			return;
		}
		await RegisterMessagesAsync(_logWriter);
		await _logWriter.WriteLoglineAsync(messageId, parameters, DateTime.UtcNow, source, user, userLocation, string.Empty, string.Empty, string.Empty, VideoOS.LogServer.Communication.LogWriterServiceReference.LogLevel.Info, PermissionType.Granted);
	}

	private async Task RegisterMessagesAsync(ILogWriter logWriter)
	{
		if (_messagesRegistered)
		{
			return;
		}
		try
		{
			await logWriter.RegisterCategoriesAsync(GetCategories());
			await logWriter.RegisterSourceTypesAsync(GetSourceTypes());
			await logWriter.RegisterLogMessagesAsync(GetLogMessages());
			_messagesRegistered = true;
		}
		catch (CommunicationServiceException arg)
		{
			_logger.LogError($"Error registering Log Server messages: {arg}");
		}
	}

	private IEnumerable<LogMessageCategory> GetCategories()
	{
		return new LogMessageCategory[1]
		{
			new LogMessageCategory
			{
				Id = "Incidents",
				LogMessageCategoryTexts = (from t in GetStringInAllLanguages("AuditLogMessageCategory")
					select new LogMessageCategoryText
					{
						LanguageCode = t.Key,
						Text = t.Value
					}).ToArray()
			}
		};
	}

	private IEnumerable<LogMessageSourceType> GetSourceTypes()
	{
		return new LogMessageSourceType[1]
		{
			new LogMessageSourceType
			{
				Id = LogType.Audit.ToString(),
				LogMessageSourceTypeTexts = (from t in GetStringInAllLanguages("AuditLogMessageSourceType")
					select new LogMessageSourceTypeText
					{
						LanguageCode = t.Key,
						Text = t.Value
					}).ToArray()
			}
		};
	}

	private IEnumerable<LogMessage> GetLogMessages()
	{
		return new LogMessage[13]
		{
			GetLogMessage(AuditLogMessageIds.IncidentCreated, "IncidentCreated"),
			GetLogMessage(AuditLogMessageIds.IncidentDeleted, "IncidentDeleted"),
			GetLogMessage(AuditLogMessageIds.IncidentCategoriesModified, "IncidentCategoriesModified"),
			GetLogMessage(AuditLogMessageIds.TypeCreated, "TypeCreated"),
			GetLogMessage(AuditLogMessageIds.TypeModified, "TypeModified"),
			GetLogMessage(AuditLogMessageIds.TypeDeleted, "TypeDeleted"),
			GetLogMessage(AuditLogMessageIds.StatusCreated, "StatusCreated"),
			GetLogMessage(AuditLogMessageIds.StatusModified, "StatusModified"),
			GetLogMessage(AuditLogMessageIds.StatusDeleted, "StatusDeleted"),
			GetLogMessage(AuditLogMessageIds.CategoryCreated, "CategoryCreated"),
			GetLogMessage(AuditLogMessageIds.CategoryModified, "CategoryModified"),
			GetLogMessage(AuditLogMessageIds.CategoryDeleted, "CategoryDeleted"),
			GetLogMessage(AuditLogMessageIds.ReportPrinted, "ReportPrinted")
		};
	}

	private LogMessage GetLogMessage(Guid messageId, string stringName)
	{
		return new LogMessage
		{
			Id = messageId,
			LogType = LogType.Audit,
			Category = "Incidents",
			SourceType = LogType.Audit.ToString(),
			MessageTexts = (from t in GetStringInAllLanguages("AuditLogMessage" + stringName)
				select new LogMessageText
				{
					LanguageCode = t.Key,
					MessageText = t.Value
				}).ToArray()
		};
	}

	private IDictionary<string, string> GetStringInAllLanguages(string name)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string> { ["en-US"] = _resourceReader.GetString(name, CultureInfo.GetCultureInfo("en-US")) };
		foreach (CultureInfo item in from c in CultureInfo.GetCultures(CultureTypes.AllCultures)
			where !c.IsNeutralCulture && !string.IsNullOrEmpty(c.Name)
			select c)
		{
			string text = _resourceReader.GetString(name, item);
			if (!string.IsNullOrEmpty(text) && text != dictionary["en-US"])
			{
				dictionary[item.Name] = text;
			}
		}
		return dictionary;
	}
}
