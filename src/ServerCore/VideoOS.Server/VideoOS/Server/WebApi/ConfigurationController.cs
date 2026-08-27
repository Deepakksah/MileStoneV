using System;
using System.Web.Http;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/configuration")]
public sealed class ConfigurationController : ApiController
{
	private IRecorderCommManager _communicationManager;

	private readonly IWebApiConfig _webApiConfig;

	public ConfigurationController(IRecorderCommManager communicationManager, IWebApiConfig config)
	{
		_communicationManager = communicationManager;
		_webApiConfig = config;
	}

	[HttpGet]
	[Route("complete/{ConfigDto}/{SecurityDto}")]
	public IHttpActionResult Get(Guid serviceId, [FromUri] GetCompleteConfigurationModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		if (!_communicationManager.IsConfigurationCompatible(serviceId))
		{
			return BadRequest();
		}
		if (!_communicationManager.RequestGetCompleteConfigurationWork())
		{
			return new ServiceUnavailableStatusCodeResult((int)_webApiConfig.SleepBetweenGetCompleteConfigurationReconnection.TotalSeconds, base.Request);
		}
		try
		{
			return Ok(_communicationManager.GetCompleteConfigurationDto(serviceId, model.ConfigDto, model.SecurityDto, tokensDto: false));
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		finally
		{
			_communicationManager.ReleaseGetCompleteConfigurationWork();
		}
	}

	[HttpGet]
	[Route("currentversions")]
	public IHttpActionResult GetCurrentConfigurationVersions(Guid serviceId)
	{
		ConfigurationVersionDto currentConfigurationVersions;
		try
		{
			currentConfigurationVersions = _communicationManager.GetCurrentConfigurationVersions(serviceId);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok(new CurrentConfigurationVersionsModel
		{
			ConfigVersion = currentConfigurationVersions.ConfigVersion,
			SecurityVersion = currentConfigurationVersions.SecurityVersion,
			TokenVersion = currentConfigurationVersions.TokenVersion
		});
	}

	[HttpPost]
	[Route("setrecorderspecificconfiguration")]
	public IHttpActionResult SetRecorderSpecificConfiguration(Guid serviceId, [FromBody] RecorderSpecificConfigurationModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			SystemConfigurationDto systemConfigurationDto = new SystemConfigurationDto(model.CompileDataSet());
			systemConfigurationDto.LastUpdate = model.LastUpdate;
			systemConfigurationDto.NextUpdate = model.NextUpdate;
			RecorderSpecificConfigurationDto recorderSpecificConfiguration = new RecorderSpecificConfigurationDto(systemConfigurationDto, model.AdditionalConfiguration);
			ServerResponseStatusDto content = _communicationManager.SetRecorderSpecificConfiguration(serviceId, model.RecorderId, recorderSpecificConfiguration, forceUpdate: false);
			return Ok(content);
		}
		catch (RecorderNotFoundException)
		{
			return Ok(ServerResponseStatusDto.Unassigned);
		}
	}

	[HttpGet]
	[Route("compatible")]
	public IHttpActionResult IsConfigurationCompatible(Guid serviceId)
	{
		bool content;
		try
		{
			content = _communicationManager.IsConfigurationCompatible(serviceId);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok(content);
	}
}
