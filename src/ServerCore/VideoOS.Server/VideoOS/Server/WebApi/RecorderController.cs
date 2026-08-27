using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}")]
public sealed class RecorderController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public RecorderController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPatch]
	[Route("patchrecorder")]
	public IHttpActionResult PatchRecorder(Guid serviceId, [FromBody] PatchRecorderModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			if (model.EventSubscription.HasValue)
			{
				_communicationManager.UpdateEventSubscriptionState(serviceId, model.EventSubscription.Value);
			}
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok();
	}

	[HttpPost]
	[Route("{storageid:guid}/promotenewencryptionpassword")]
	public IHttpActionResult PromoteNewEncryptionPassword(Guid serviceId, Guid storageId)
	{
		try
		{
			_communicationManager.PromoteNewEncryptionPassword(serviceId, storageId);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok();
	}

	[HttpPatch]
	[Route("updaterecorderstatusdata")]
	public IHttpActionResult UpdateRecorderStatusData(Guid serviceId, [FromBody] UpdateRecorderStatusDataModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			_communicationManager.UpdateRecorderStatusData(serviceId, model.InfoType, model.Data);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok();
	}

	[HttpPost]
	[Route("log")]
	public IHttpActionResult Log(Guid serviceId, [FromBody] LogEntriesModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			List<LogEntry> list = new List<LogEntry>();
			if (model.AuditLogEntries != null)
			{
				list.AddRange(model.AuditLogEntries.Select((LogEntryAuditModel l) => l.ToLogEntry()));
			}
			if (model.RuleLogEntries != null)
			{
				list.AddRange(model.RuleLogEntries.Select((LogEntryRuleModel l) => l.ToLogEntry()));
			}
			if (model.SystemLogEntries != null)
			{
				list.AddRange(model.SystemLogEntries.Select((LogEntrySystemModel l) => l.ToLogEntry()));
			}
			_communicationManager.Log(serviceId, list.ToArray());
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return Ok();
	}

	[HttpPost]
	[Route("filtertrackidstothosethatshouldnothavemediaonrecorder")]
	public IHttpActionResult FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(Guid serviceId, [FromBody] FilterTrackIdsModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			return Ok(_communicationManager.FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(serviceId, model.TargetRecorderId, model.TrackIds.ToArray()));
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
	}
}
