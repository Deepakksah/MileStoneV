using System;
using System.Collections.Generic;
using VideoOS.IM.Server.Localization.Resources;
using VideoOS.IM.Server.Logger.AuditLog;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.IM.Server.WebApi.Utils;

public static class AuditLogHelper
{
	private const string ID_PLACEHOLDER = "{id}";

	private static readonly Dictionary<Guid, string> _auditMessageIdMap = new Dictionary<Guid, string>
	{
		{
			AuditLogMessageIds.TypeCreated,
			"TypeCreated"
		},
		{
			AuditLogMessageIds.StatusCreated,
			"StatusCreated"
		},
		{
			AuditLogMessageIds.CategoryCreated,
			"CategoryCreated"
		},
		{
			AuditLogMessageIds.TypeModified,
			"TypeModified"
		},
		{
			AuditLogMessageIds.StatusModified,
			"StatusModified"
		},
		{
			AuditLogMessageIds.CategoryModified,
			"CategoryModified"
		},
		{
			AuditLogMessageIds.TypeDeleted,
			"TypeDeleted"
		},
		{
			AuditLogMessageIds.StatusDeleted,
			"StatusDeleted"
		},
		{
			AuditLogMessageIds.CategoryDeleted,
			"CategoryDeleted"
		},
		{
			AuditLogMessageIds.IncidentCreated,
			"IncidentCreated"
		},
		{
			AuditLogMessageIds.IncidentDeleted,
			"IncidentDeleted"
		},
		{
			AuditLogMessageIds.IncidentCategoriesModified,
			"IncidentCategoriesModified"
		},
		{
			AuditLogMessageIds.ReportPrinted,
			"ReportPrinted"
		}
	};

	private static readonly Dictionary<Guid, string> _descriptionResourceMap = new Dictionary<Guid, string>
	{
		{
			AuditLogMessageIds.TypeCreated,
			Strings.AuditLogMessageTypeCreated
		},
		{
			AuditLogMessageIds.StatusCreated,
			Strings.AuditLogMessageStatusCreated
		},
		{
			AuditLogMessageIds.CategoryCreated,
			Strings.AuditLogMessageCategoryCreated
		},
		{
			AuditLogMessageIds.TypeModified,
			Strings.AuditLogMessageTypeModified
		},
		{
			AuditLogMessageIds.StatusModified,
			Strings.AuditLogMessageStatusModified
		},
		{
			AuditLogMessageIds.CategoryModified,
			Strings.AuditLogMessageCategoryModified
		},
		{
			AuditLogMessageIds.TypeDeleted,
			Strings.AuditLogMessageTypeDeleted
		},
		{
			AuditLogMessageIds.StatusDeleted,
			Strings.AuditLogMessageStatusDeleted
		},
		{
			AuditLogMessageIds.CategoryDeleted,
			Strings.AuditLogMessageCategoryDeleted
		},
		{
			AuditLogMessageIds.IncidentCreated,
			Strings.AuditLogMessageIncidentCreated
		},
		{
			AuditLogMessageIds.IncidentDeleted,
			Strings.AuditLogMessageIncidentDeleted
		},
		{
			AuditLogMessageIds.IncidentCategoriesModified,
			Strings.AuditLogMessageIncidentCategoriesModified
		},
		{
			AuditLogMessageIds.ReportPrinted,
			Strings.AuditLogMessageReportPrinted
		}
	};

	public static string GetAuditMessageId(Guid messageGuid)
	{
		if (!_auditMessageIdMap.TryGetValue(messageGuid, out var value))
		{
			return $"Could not get audit message ID with message ID: {messageGuid}";
		}
		return value;
	}

	public static string GetDescriptionResource(Guid messageGuid)
	{
		if (!_descriptionResourceMap.TryGetValue(messageGuid, out var value))
		{
			return $"Could not get resource with message ID: {messageGuid}";
		}
		return value;
	}

	public static LogItem CreateLogDescription(string logDescription, string objectId)
	{
		if (logDescription.Contains("{id}") && objectId != null)
		{
			return new LogItem(Strings.Field_Description, logDescription.Replace("{id}", "{0}"), objectId);
		}
		return new LogItem(Strings.Field_Description, logDescription);
	}
}
