using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Management.VmoClient;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/registration")]
public sealed class RecorderRegistrationController : ApiController
{
	private readonly IRecorderCommManager _communicationManager;

	private readonly TimeSpan _serviceDiscoveryTimeout;

	private readonly IServiceDiscoveryEngine _serviceDiscoveryEngine;

	private readonly IWebApiConfig _webApiConfig;

	private readonly ITokenCache _tokenCache;

	private readonly ILogger _logger;

	public RecorderRegistrationController(IRecorderCommManager communicationManager, TimeSpan serviceDiscoveryTimeout, IServiceDiscoveryEngine serviceDiscoveryEngine, IWebApiConfig config, ITokenCache tokenCache, ILogger logger)
	{
		_communicationManager = communicationManager;
		_serviceDiscoveryTimeout = serviceDiscoveryTimeout;
		_serviceDiscoveryEngine = serviceDiscoveryEngine;
		_webApiConfig = config;
		_tokenCache = tokenCache;
		_logger = logger;
	}

	[HttpPost]
	[Route("registerrecorder")]
	public IHttpActionResult RegisterRecorder(Guid serviceId, [FromBody] RegisterRecorderModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		if (!_webApiConfig.Scheme.Equals(model.ServerCommunicationWebApiUri.Scheme, StringComparison.OrdinalIgnoreCase))
		{
			return BadRequest(string.Format(CultureInfo.InvariantCulture, "Recorder must use {0} scheme", _webApiConfig.Scheme));
		}
		if (!_communicationManager.RequestAttachRecorderWork())
		{
			return new ServiceUnavailableStatusCodeResult((int)_webApiConfig.SleepBetweenRecorderReconnection.TotalSeconds, base.Request);
		}
		try
		{
			WebApiRecorderData webApiRecorderData = new WebApiRecorderData(model.RecorderId, model.ServerCommunicationWebApiUri, serviceId, (RecordingServerServiceType)model.RecorderServiceType, RecorderConnectionStatus.NotConnected, RecorderAttachStatus.NotAttached, RecorderStatuses.None, model.Version);
			if (webApiRecorderData.ServiceType == RecordingServerServiceType.FailoverServer && !_communicationManager.RecorderServerServiceExists(webApiRecorderData.RecorderId))
			{
				VideoOS.Common.Integration.WebApi.Models.Server.ErrorResponseModel content = new VideoOS.Common.Integration.WebApi.Models.Server.ErrorResponseModel
				{
					ErrorCode = 1,
					Message = string.Format(CultureInfo.InvariantCulture, "Cannot find failover recorder recording service {0} in the database.", model.RecorderId)
				};
				return new ContentStatusCodeResult(HttpStatusCode.Forbidden, base.Request, content);
			}
			bool compatible = _communicationManager.RegisterRecorder(webApiRecorderData, new WebApiRecorderEntryFactory(), model.RecorderName, model.HostName, model.WebServiceUri.AbsoluteUri, model.FailoverPort) || webApiRecorderData.RecorderVersion == RecorderVersion.Version132 || webApiRecorderData.RecorderVersion == RecorderVersion.Version201;
			return Ok(new RegisterRecorderResponseModel
			{
				Compatible = compatible,
				ServiceDiscoveryTimeout = _serviceDiscoveryTimeout
			});
		}
		catch (RecordingServerUnlicensedException)
		{
			RegisterRecorderResponseModel content2 = new RegisterRecorderResponseModel
			{
				ErrorCode = ApplicationErrorCode.Unlicensed,
				Message = "Recorder is not licensed to register"
			};
			return new ContentStatusCodeResult(HttpStatusCode.Forbidden, base.Request, content2);
		}
		finally
		{
			_communicationManager.ReleaseAttachRecorderWork();
		}
	}

	[HttpPost]
	[Route("unregisterrecorder")]
	public IHttpActionResult UnregisterRecorder(Guid serviceId)
	{
		_communicationManager.UnregisterRecorder(serviceId);
		return Ok();
	}

	[HttpPost]
	[Route("setrecorderinformation")]
	public IHttpActionResult SetRecorderInformation(Guid serviceId, [FromBody] RecorderInformationModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		if (_communicationManager.IsRecorderHotStandByFailoverType(serviceId) && !_communicationManager.RecorderServerServiceExists(serviceId))
		{
			VideoOS.Common.Integration.WebApi.Models.Server.ErrorResponseModel content = new VideoOS.Common.Integration.WebApi.Models.Server.ErrorResponseModel
			{
				ErrorCode = 1,
				Message = string.Format(CultureInfo.InvariantCulture, "Cannot find failover recorder recording service {0} in the database.", serviceId)
			};
			return new ContentStatusCodeResult(HttpStatusCode.Forbidden, base.Request, content);
		}
		_communicationManager.SetRecorderInformation(serviceId, model.DisplayName, model.HostName, model.WebserverUri, model.ScanRanges, model.TimeZone, model.DevicePackInfoXml, model.FailoverServerPort, model.DatabasePort, model.FailoverServerId, model.Version);
		return Ok();
	}

	[HttpPost]
	[Route("keepalive")]
	public IHttpActionResult KeepAlive(Guid serviceId)
	{
		if (!_communicationManager.IsRecorderServiceAvailable(serviceId))
		{
			return Ok(content: false);
		}
		if (_tokenCache.IsTokenExpired())
		{
			_logger.LogError("The token used for communication with the recorder service has expired.");
			return InternalServerError();
		}
		_serviceDiscoveryEngine?.CommunicationConfirmed(serviceId);
		return Ok(content: true);
	}

	[HttpPost]
	[Route("validatepublicaddress")]
	public async Task<IHttpActionResult> ValidatePublicAddress(Guid serviceId, [FromBody] Uri validationAddress)
	{
		if (validationAddress == null)
		{
			return BadRequest();
		}
		AddressValidationResponse response = new AddressValidationResponse
		{
			WebExceptionStatus = WebExceptionStatus.UnknownError,
			Message = "Address could not be validated because of an unknown error."
		};
		try
		{
			if (!_webApiConfig.Scheme.Equals(validationAddress.Scheme, StringComparison.OrdinalIgnoreCase))
			{
				response.Message = FormattableString.Invariant($"There was a mismatch between the expected scheme {_webApiConfig.Scheme} and what was received {validationAddress.Scheme}.");
				return new ServiceUnavailableStatusCodeResult(120, base.Request, response);
			}
			if (!(await ValidateHttpEndpointAsync(validationAddress, "api/recorder/serviceid", serviceId).ConfigureAwait(continueOnCapturedContext: false)))
			{
				response.Message = FormattableString.Invariant($"The received service id does not match what was returned from the validation address.");
				return new ServiceUnavailableStatusCodeResult(120, base.Request, response);
			}
		}
		catch (CommunicationException ex)
		{
			if (ex.InnerException is HttpRequestException && ex.InnerException.InnerException is WebException)
			{
				WebException ex2 = (WebException)ex.InnerException.InnerException;
				response = new AddressValidationResponse(ex2);
				if (ex2.Status == WebExceptionStatus.ConnectFailure && ex2.InnerException is SocketException)
				{
					response.Message = ex2.InnerException.Message;
				}
				if (ex2.Status == WebExceptionStatus.SendFailure && ex2.InnerException is IOException)
				{
					response.Message = ex2.InnerException.Message;
				}
				return new ServiceUnavailableStatusCodeResult(120, base.Request, response);
			}
			response.Message = ex.Message;
			return new ServiceUnavailableStatusCodeResult(30, base.Request, response);
		}
		catch (VideoOS.Common.Integration.WebApi.TimeoutException)
		{
			response.Message = "Tried to validate address, but the request timed out.";
			return new ServiceUnavailableStatusCodeResult(30, base.Request, response);
		}
		catch (AccessTokenException)
		{
			_logger.LogError("ValidatePublicAddress was called, but a token could not be retrieved.");
			response.Message = "Internal token error.";
			return new ServiceUnavailableStatusCodeResult(30, base.Request, response);
		}
		catch (ResponseException ex5)
		{
			_logger.LogError("Recorder responded with status code {ExceptionStatusCode} during ValidatePublicAddress.", ex5.StatusCode);
			response.Message = $"Tried to validate address, but the recorder responded with status code {ex5.StatusCode}";
			return new ServiceUnavailableStatusCodeResult(120, base.Request, response);
		}
		catch (TrustFailureException)
		{
			_logger.LogError("The Recorder at {ValidationAddress} has an invalid certificate.", validationAddress);
			response.Message = "Server cannot validate the certificate on the recorder. Please ensure you are using a valid certificate, and that the certificate is trusted on the Management Server.";
			return new ServiceUnavailableStatusCodeResult(120, base.Request, response);
		}
		response.Message = "Validation was successful.";
		response.WebExceptionStatus = WebExceptionStatus.Success;
		return Ok(response);
	}

	private async Task<bool> ValidateHttpEndpointAsync(Uri baseAddress, string endpoint, Guid serviceId)
	{
		Guid guid = Guid.Empty;
		using (HttpClientCommunicator communicator = new HttpClientCommunicator(baseAddress, _logger, new ServerAccessTokenHandler(_tokenCache), null))
		{
			guid = await communicator.ReceiveAsync<Guid>(endpoint);
		}
		return guid.Equals(serviceId);
	}
}
