using System;
using System.Linq;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public static class LogEntryModelConverter
{
	public static LogEntrySystem ToLogEntry(this LogEntrySystemModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		LogEntrySystem logEntry = ((model.LogEntryDefinition == null) ? new LogEntrySystem((LogEntryId)model.LogId) : new LogEntrySystem(model.SourceId, (SourceType)Enum.Parse(typeof(SourceType), model.SourceTypeValue), model.SourceName, model.EventTypeId, model.EventType, model.LogEntryDefinition));
		logEntry = SetLogEntryFields(logEntry, model);
		logEntry.EventType = model.EventType;
		logEntry.EventTypeId = model.EventTypeId;
		return logEntry;
	}

	public static LogEntrySystemModel ToModel(this LogEntrySystem entry)
	{
		if (entry == null)
		{
			throw new ArgumentNullException("entry");
		}
		LogEntrySystemModel model = new LogEntrySystemModel();
		model = SetLogEntryModelFields(entry, model);
		model.EventType = entry.EventType;
		model.EventTypeId = entry.EventTypeId;
		return model;
	}

	public static LogEntryRule ToLogEntry(this LogEntryRuleModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		LogEntryRule logEntryRule = SetLogEntryFields(new LogEntryRule((LogEntryId)model.LogId), model);
		logEntryRule.EventType = model.EventType;
		logEntryRule.EventTypeId = model.EventTypeId;
		logEntryRule.ServiceName = model.ServiceName;
		logEntryRule.RuleName = model.RuleName;
		logEntryRule.GeneratorId = model.GeneratorId;
		logEntryRule.GeneratorName = model.GeneratorName;
		logEntryRule.GeneratorTypeValue = model.GeneratorTypeValue;
		return logEntryRule;
	}

	public static LogEntryRuleModel ToModel(this LogEntryRule entry)
	{
		if (entry == null)
		{
			throw new ArgumentNullException("entry");
		}
		LogEntryRuleModel model = new LogEntryRuleModel();
		model = SetLogEntryModelFields(entry, model);
		model.EventType = entry.EventType;
		model.EventTypeId = entry.EventTypeId;
		model.ServiceName = entry.ServiceName;
		model.RuleName = entry.RuleName;
		model.GeneratorId = entry.GeneratorId;
		model.GeneratorName = entry.GeneratorName;
		model.GeneratorTypeValue = entry.GeneratorTypeValue;
		return model;
	}

	public static LogEntryAudit ToLogEntry(this LogEntryAuditModel model)
	{
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		LogEntryAudit logEntry = ((model.LogEntryDefinition == null) ? new LogEntryAudit((LogEntryId)model.LogId) : new LogEntryAudit(model.Location, (SourceType)Enum.Parse(typeof(SourceType), model.SourceTypeValue), model.SourceName, model.UserCredentials, (ResourceType)Enum.Parse(typeof(ResourceType), model.ResourceTypeValue), model.ResourceName, model.ResourceLocation, model.ResourceID, (AuditType)Enum.Parse(typeof(AuditType), model.AuditTypeValue), (AccessGranted)Enum.Parse(typeof(AccessGranted), model.AccessGrantedValue), model.LogEntryDefinition));
		logEntry = SetLogEntryFields(logEntry, model);
		logEntry.AccessGrantedValue = model.AccessGrantedValue;
		logEntry.AuditTypeValue = model.AuditTypeValue;
		logEntry.UserCredentials = model.UserCredentials;
		logEntry.Location = model.Location;
		logEntry.ResourceID = model.ResourceID;
		logEntry.ResourceTypeValue = model.ResourceTypeValue;
		logEntry.ResourceName = model.ResourceName;
		logEntry.ResourceLocation = model.ResourceLocation;
		return logEntry;
	}

	public static LogEntryAuditModel ToModel(this LogEntryAudit entry)
	{
		if (entry == null)
		{
			throw new ArgumentNullException("entry");
		}
		LogEntryAuditModel model = new LogEntryAuditModel();
		model = SetLogEntryModelFields(entry, model);
		model.AccessGrantedValue = entry.AccessGrantedValue;
		model.AuditTypeValue = entry.AuditTypeValue;
		model.UserCredentials = entry.UserCredentials;
		model.Location = entry.Location;
		model.ResourceID = entry.ResourceID;
		model.ResourceTypeValue = entry.ResourceTypeValue;
		model.ResourceName = entry.ResourceName;
		model.ResourceLocation = entry.ResourceLocation;
		return model;
	}

	private static T SetLogEntryFields<T>(T logEntry, LogEntryModel model) where T : LogEntry
	{
		logEntry.Arguments = model.Arguments.ToArray();
		logEntry.Description = model.Description;
		logEntry.LogId = model.LogId;
		logEntry.LogLevel = model.LogLevel;
		logEntry.LogLevelValue = model.LogLevelValue;
		logEntry.MigratedLogDescription = model.MigratedLogDescription;
		logEntry.SourceId = model.SourceId;
		logEntry.SourceName = model.SourceName;
		logEntry.SourceTypeValue = model.SourceTypeValue;
		logEntry.TimeStampLocal = model.TimestampLocal;
		logEntry.TimeStampUtc = model.TimestampUtc;
		return logEntry;
	}

	private static T SetLogEntryModelFields<T>(LogEntry logEntry, T model) where T : LogEntryModel
	{
		model.Arguments = logEntry.Arguments;
		model.Description = logEntry.Description;
		model.LogId = logEntry.LogId;
		model.LogIdString = null;
		model.LogLevel = logEntry.LogLevel;
		model.LogLevelValue = logEntry.LogLevelValue;
		model.MigratedLogDescription = logEntry.MigratedLogDescription;
		model.SourceId = logEntry.SourceId;
		model.SourceName = logEntry.SourceName;
		model.SourceTypeValue = logEntry.SourceTypeValue;
		model.TimestampLocal = logEntry.TimeStampLocal;
		model.TimestampUtc = logEntry.TimeStampUtc;
		model.LogEntryDefinition = logEntry.LogEntryDefinition;
		return model;
	}
}
