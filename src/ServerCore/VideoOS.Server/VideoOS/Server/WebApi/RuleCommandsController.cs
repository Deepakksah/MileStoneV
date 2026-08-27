using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/commands/rule")]
public sealed class RuleCommandsController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public RuleCommandsController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("executeruleactions")]
	public IHttpActionResult ExecuteRuleActionsCommand(Guid serviceId, [FromBody] ExecuteRuleActionsModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		RuleActionExecutionRequestDto[] array = model.RuleActionExecutionRequest.ToArray();
		RuleActionExecutionRequest[] array2 = new RuleActionExecutionRequest[array.Length];
		for (int i = 0; i < model.RuleActionExecutionRequest.Count(); i++)
		{
			RuleActionExecutionRequestDto dto = array[i];
			array2[i] = RuleActionExecutionRequestDto.CreateRuleActionExecutionRequest(dto);
		}
		ExecuteRuleActionsCommand command = new ExecuteRuleActionsCommand(model.SourceId, array2)
		{
			Token = model.Token,
			SenderUtcTime = model.SenderUtcTime
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
	[Route("executemipaction")]
	public IHttpActionResult ExecuteMipActionCommand(Guid serviceId, [FromBody] MipActionModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		MIPActionExecuteCommand command = new MIPActionExecuteCommand(model.ActionId, model.ActionName, model.Targets.ToArray(), TriggerEventDto.CreateTriggerEvent(model.TriggerEvent))
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
	[Route("executemipaction2")]
	public IHttpActionResult ExecuteMipAction2Command(Guid serviceId, [FromBody] MipAction2Model model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		MIPAction2ExecuteCommand command = new MIPAction2ExecuteCommand(model.ActionId, model.ActionName, model.Targets.ToArray(), model.Targets2.ToArray(), TriggerEventDto.CreateTriggerEvent(model.TriggerEvent))
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
