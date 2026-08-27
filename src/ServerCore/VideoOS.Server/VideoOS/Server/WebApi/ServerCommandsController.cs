using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/commands")]
public sealed class ServerCommandsController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public ServerCommandsController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("videowall")]
	public IHttpActionResult VideoWallCommand(Guid serviceId, [FromBody] VideoWallModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		VideoWallCommand videoWallCommand = new VideoWallCommand(DtoConvert.ToVideoWallOperation(model.OperationType), model.VideoWallId, model.MonitorId, model.PresetId, model.ViewId, model.LayoutId, model.ViewPosition, model.DeviceIds?.ToArray(), model.DeviceGroupIds?.ToArray(), model.MonitorText);
		videoWallCommand.Token = model.Token;
		_communicationManager.ProcessReceivedCommand(serviceId, videoWallCommand);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("setmatrixview")]
	public IHttpActionResult SetMatrixView(Guid serviceId, [FromBody] SetMatrixViewModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		SetMatrixViewCommand setMatrixViewCommand = new SetMatrixViewCommand(model.MatrixId, model.DeviceIds?.ToArray());
		setMatrixViewCommand.Token = model.Token;
		_communicationManager.ProcessReceivedCommand(serviceId, setMatrixViewCommand);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("sendmultinotification")]
	public IHttpActionResult SendMultiNotification(Guid serviceId, [FromBody] SendMultiNotificationModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		SendMultiNotificationCommand sendMultiNotificationCommand = new SendMultiNotificationCommand(model.IDNotificationProfile, model.RuleId, model.EventTypeId, model.TriggerId, model.RecorderId, model.SourceIds?.ToArray(), model.SourceGroupIds?.ToArray(), model.Time);
		sendMultiNotificationCommand.Token = model.Token;
		_communicationManager.ProcessReceivedCommand(serviceId, sendMultiNotificationCommand);
		return StatusCode(HttpStatusCode.NoContent);
	}
}
