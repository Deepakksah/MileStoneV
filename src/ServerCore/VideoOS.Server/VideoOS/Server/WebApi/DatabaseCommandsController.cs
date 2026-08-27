using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/commands/database")]
public sealed class DatabaseCommandsController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public DatabaseCommandsController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("reportmediastatus")]
	public IHttpActionResult ReportMediaStatusCommand(Guid serviceId, [FromBody] ReportMediaStatusModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		MediaStatusEntry2[] source = ((model.MediaStatus == null) ? new MediaStatusEntry2[0] : model.MediaStatus.Select((MediaStatusEntryModel mediaStatus) => new MediaStatusEntry2(mediaStatus.DeviceId, mediaStatus.MediaRetentionTime)).ToArray());
		ReportMediaStatusCommand reportMediaStatusCommand = new ReportMediaStatusCommand(trackStatus: ((model.TrackStatus == null) ? new TrackStatusEntry[0] : model.TrackStatus.Select((TrackStatusEntryModel trackStatus) => new TrackStatusEntry(trackStatus.TrackId, trackStatus.MediaTime)).ToArray()).ToArray(), mediaStatus: source.ToArray());
		reportMediaStatusCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, reportMediaStatusCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("reportmediacommittedtodatabase")]
	public IHttpActionResult ReportMediaCommittedToDatabaseCommand(Guid serviceId, [FromBody] ReportMediaCommittedToDatabaseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ReportMediaCommittedToDatabaseCommand reportMediaCommittedToDatabaseCommand = new ReportMediaCommittedToDatabaseCommand(model.DeviceId, model.StartTime, model.EndTime);
		reportMediaCommittedToDatabaseCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, reportMediaCommittedToDatabaseCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("reportmanualrecordingstate")]
	public IHttpActionResult ReportManualRecordingStateCommand(Guid serviceId, [FromBody] ReportManualRecordingStateModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ReportManualRecordingStateCommand reportManualRecordingStateCommand = new ReportManualRecordingStateCommand(model.DeviceId, model.IsManualRecording);
		reportManualRecordingStateCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, reportManualRecordingStateCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("addbookmark")]
	public IHttpActionResult AddBookmarkCommand(Guid serviceId, [FromBody] AddBookmarkModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		AddBookmarkCommand addBookmarkCommand = new AddBookmarkCommand(model.DeviceIds?.ToArray(), model.Header, model.Description, model.TimeTrigged, model.TimeBefore, model.TimeAfter);
		addBookmarkCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, addBookmarkCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("activatearchiving")]
	public IHttpActionResult ActivateArchivingCommand(Guid serviceId, [FromBody] ActivateArchivingModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ActivateArchivingCommand activateArchivingCommand = new ActivateArchivingCommand(model.ArchiveStorageId);
		activateArchivingCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, activateArchivingCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("edgejobupdated")]
	public IHttpActionResult EdgeJobUpdatedCommand(Guid serviceId, [FromBody] EdgeJobUpdatedModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		EdgeJobUpdatedCommand edgeJobUpdatedCommand = new EdgeJobUpdatedCommand(model.DeviceId, model.StartTime, model.EndTime);
		edgeJobUpdatedCommand.Token = model.Token;
		try
		{
			_communicationManager.ProcessReceivedCommand(serviceId, edgeJobUpdatedCommand);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}
}
