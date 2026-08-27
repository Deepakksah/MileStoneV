using System.Collections.Generic;
using System.Web.Http;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Management.Server.RestApi;

[Route("api/serverLogsSettings")]
public class ServerLogsSettingsController : ApiController
{
	public const string JsonField_LogLevels = "logLevel";

	public const string JsonField_Culture = "culture";

	private readonly IRestApiRequestContextFactory _requestContextFactory;

	public ServerLogsSettingsController()
		: this(new RestApiRequestContextFactory())
	{
	}

	private ServerLogsSettingsController(IRestApiRequestContextFactory requestContextFactory)
	{
		_requestContextFactory = requestContextFactory;
	}

	[HttpGet]
	public IHttpActionResult Get(string pathId = null)
	{
		new Dictionary<string, object>();
		using IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request);
		ILocalizedLogger logger = restApiRequestContext.GetService<AuditAndSystemLoggingService>().Logger;
		var content = new
		{
			logLevel = logger.BaseLogger.HighestEnabledLogLevel().ToString(),
			culture = logger.Culture.Name
		};
		return Ok(content);
	}
}
