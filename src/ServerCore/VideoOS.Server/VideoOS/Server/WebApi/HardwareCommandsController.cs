using System;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/commands/hardware")]
public sealed class HardwareCommandsController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public HardwareCommandsController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("updatereadonlysettings")]
	public IHttpActionResult UpdateReadOnlySettings(Guid serviceId, [FromBody] UpdateHardwareReadOnlySettingsModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		UpdateHardwareReadOnlySettingsCommand command = new UpdateHardwareReadOnlySettingsCommand(model.HardwareId, model.UpdatedSettingsXml)
		{
			Token = model.Token
		};
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, command);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("updatedriver")]
	public IHttpActionResult UpdateDriver(Guid serviceId, [FromBody] UpdateHardwareDriverModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		SystemConfigurationDto systemConfigurationDto = new SystemConfigurationDto(model.CompileDataSet());
		systemConfigurationDto.LastUpdate = model.LastUpdate;
		systemConfigurationDto.NextUpdate = model.NextUpdate;
		UpdateHardwareDriverCommand updateHardwareDriverCommand = new UpdateHardwareDriverCommand(systemConfigurationDto);
		updateHardwareDriverCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, updateHardwareDriverCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}
}
