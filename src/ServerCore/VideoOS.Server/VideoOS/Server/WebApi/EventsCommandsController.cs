using System;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/commands/events")]
public sealed class EventsCommandsController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public EventsCommandsController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("triggerglobalevent")]
	public IHttpActionResult TriggerEventAsGlobalCommand(Guid serviceId, [FromBody] GlobalEventModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		TriggerEventAsGlobalCommand triggerEventAsGlobalCommand = new TriggerEventAsGlobalCommand(model.SourceId, TriggerEventDto.CreateTriggerEvent(model.EventCommand));
		triggerEventAsGlobalCommand.Token = model.Token;
		triggerEventAsGlobalCommand.SourceId = model.SourceId;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, triggerEventAsGlobalCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("activateuserdefinedeventonremotesite")]
	public IHttpActionResult ActivateUserDefinedEventOnRemoteSiteCommand(Guid serviceId, [FromBody] ActivateUserDefinedEventOnRemoteSiteModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ActivateUserDefinedEventOnRemoteSiteCommand command = new ActivateUserDefinedEventOnRemoteSiteCommand(model.SiteId, model.UserDefinedEventNameOrId)
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
}
