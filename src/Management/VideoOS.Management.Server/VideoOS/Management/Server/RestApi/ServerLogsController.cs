using System;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using VideoOS.Common.Integration.Properties;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Management.Server.RestApi;

[Route("api/serverLogs")]
public class ServerLogsController : ApiController
{
	public const string JsonField_LogType = "logType";

	public const string JsonField_LogLevel = "logLevel";

	public const string JsonField_LogEntryID = "logEntryID";

	public const string JsonField_ResourceID = "resourceID";

	public const string JsonField_ResourceName = "resourceName";

	public const string JsonField_ResourceLocation = "resourceLocation";

	public const string JsonField_ResourceType = "resourceType";

	public const string JsonField_AuditType = "auditType";

	public const string JsonField_AccessGranted = "accessGranted";

	public const string JsonField_Description = "description";

	private readonly IRestApiRequestContextFactory _requestContextFactory;

	public ServerLogsController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public ServerLogsController(IRestApiRequestContextFactory requestContextFactory)
	{
		_requestContextFactory = requestContextFactory;
	}

	[HttpPost]
	public IHttpActionResult Post([FromBody] JObject postContent)
	{
		using IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request);
		ILocalizedLogger logger = restApiRequestContext.GetService<AuditAndSystemLoggingService>().Logger;
		string userName = ((!(base.User is ClaimsPrincipal)) ? string.Empty : ((ClaimsPrincipal)base.User).FindFirst("name")?.Value);
		string userHostAddress = HttpContext.Current.Request.UserHostAddress;
		DeserializeAndLog(logger, postContent, userName, userHostAddress);
		return Ok();
	}

	public static void DeserializeAndLog(ILocalizedLogger serverLogger, JObject content, string userName, string locationIPAddress)
	{
		string stringValue = JObjectUtil.GetStringValue(content, "logLevel", allowDefault: true);
		LogLevel result = LogLevel.Information;
		if (!string.IsNullOrEmpty(stringValue) && !Enum.TryParse<LogLevel>(stringValue, ignoreCase: true, out result))
		{
			throw new ArgumentException("Invalid value specified for logLevel, must be one of Trace, Debug, Information, Warning, Error, Critical, None");
		}
		serverLogger.Log(result, (CultureInfo c) => new LogItem[11]
		{
			new LogItem(LogDefinitions.Field_LogType, JObjectUtil.GetStringValue(content, "logType", allowDefault: false)),
			new LogItem(LogDefinitions.Field_LogEntryId, JObjectUtil.GetStringValue(content, "logEntryID", allowDefault: false)),
			new LogItem(LogDefinitions.Field_ResourceId, JObjectUtil.GetStringValue(content, "resourceID", allowDefault: true) ?? string.Empty),
			new LogItem(LogDefinitions.Field_ResourceName, JObjectUtil.GetStringValue(content, "resourceName", allowDefault: true) ?? string.Empty),
			new LogItem(LogDefinitions.Field_ResourceLocation, JObjectUtil.GetStringValue(content, "resourceLocation", allowDefault: true) ?? string.Empty),
			new LogItem(LogDefinitions.Field_ResourceType, JObjectUtil.GetStringValue(content, "resourceType", allowDefault: true) ?? string.Empty),
			new LogItem(LogDefinitions.Field_LocationIPAddress, locationIPAddress ?? string.Empty),
			new LogItem(LogDefinitions.Field_UserName, userName),
			new LogItem(LogDefinitions.Field_AuditType, JObjectUtil.GetStringValue(content, "auditType", allowDefault: true) ?? string.Empty),
			new LogItem(LogDefinitions.Field_AccessGranted, JObjectUtil.GetStringValue(content, "accessGranted", allowDefault: true) ?? string.Empty),
			new LogItem(LogDefinitions.Field_Description, JObjectUtil.GetStringValue(content, "description", allowDefault: false))
		});
	}
}
