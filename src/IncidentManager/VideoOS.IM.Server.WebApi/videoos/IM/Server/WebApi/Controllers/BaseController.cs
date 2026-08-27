using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Common.Service.Interfaces;
using VideoOS.IM.Server.Localization.Resources;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Service;
using VideoOS.IM.Server.WebApi.Utils;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.IM.Server.WebApi.Controllers;

public abstract class BaseController : ApiController
{
	protected IDtoDAL DataAccessLayer;

	protected BaseController()
		: this(GlobalWebApiAppDALInstanceSelector.GetDtoDALImplementation())
	{
	}

	protected BaseController(IDtoDAL dataAccessLayer)
	{
		DataAccessLayer = dataAccessLayer;
	}

	protected async Task WriteAuditLogEntryAsync(Guid messageId, string auditMessageId, string objectId = null, string objectName = null)
	{
		try
		{
			string source = base.RequestContext.Url.Request.RequestUri.Host.ToUpperInvariant();
			string user = (base.RequestContext.Principal as ClaimsPrincipal)?.FindFirst("name")?.Value.ToUpperInvariant();
			string userHostAddress = HttpContext.Current.Request.UserHostAddress;
			string userLocation = userHostAddress;
			if (!string.IsNullOrEmpty(userLocation))
			{
				try
				{
					userLocation = (await Dns.GetHostEntryAsync(userLocation).ConfigureAwait(continueOnCapturedContext: false)).HostName.ToUpperInvariant();
				}
				catch (Exception ex)
				{
					LoggerProvider.Instance.Logger.LogWarning("Error resolving DNS host name for {UserLocation}: {Exception}", userLocation, ex);
				}
			}
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			if (objectId != null)
			{
				parameters.Add("id", objectId);
			}
			HostingEnvironment.QueueBackgroundWorkItem(async delegate
			{
				await LoggerProvider.Instance.AuditLogger.WriteAuditLogEntryAsync(messageId, parameters, source, user, userLocation).ConfigureAwait(continueOnCapturedContext: false);
			});
			LoggerProvider.Instance.AuditAndSystemLogger.Log(Microsoft.Extensions.Logging.LogLevel.Information, (CultureInfo culture) => new LogItem[10]
			{
				new LogItem(Strings.Field_LogType, ServerLogType.Audit, Strings.ResourceManager),
				new LogItem(Strings.Field_LogEntryId, auditMessageId),
				AuditLogHelper.CreateLogDescription(AuditLogHelper.GetDescriptionResource(messageId), objectId),
				new LogItem(Strings.Field_ResourceName, objectName),
				new LogItem(Strings.Field_SourceType, Strings.AuditLogMessageSourceType),
				new LogItem(Strings.Field_LocationIPAddress, userHostAddress),
				new LogItem(Strings.Field_UserName, user),
				new LogItem(Strings.Field_AuditType, Strings.AuditLogMessageCategory),
				new LogItem(Strings.Field_AccessGranted, PermissionType.Granted, Strings.ResourceManager),
				new LogItem(Strings.Field_ResourceLocation, source)
			});
		}
		catch (Exception ex2)
		{
			LoggerProvider.Instance.Logger.LogError("Audit logging error: {Exception}", ex2);
		}
	}
}
