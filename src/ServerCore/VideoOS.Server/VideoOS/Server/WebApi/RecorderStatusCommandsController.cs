using System;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/commands/recorderstatus")]
public sealed class RecorderStatusCommandsController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public RecorderStatusCommandsController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("update")]
	public IHttpActionResult UpdateRecorderStatus(Guid serviceId, [FromBody] RecorderStatusModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		UpdateRecorderStatusCommand updateRecorderStatusCommand = new UpdateRecorderStatusCommand(serviceId, DtoConvert.ToRecorderStatuses(model.Statuses));
		updateRecorderStatusCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, updateRecorderStatusCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}
}
