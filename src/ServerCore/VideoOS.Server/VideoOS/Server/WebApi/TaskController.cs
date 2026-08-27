using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/tasks")]
public sealed class TaskController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public TaskController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPatch]
	[Route("{taskid:long}/updateprogress")]
	public IHttpActionResult UpdateRecorderTaskProgress(Guid serviceId, long taskId, [FromBody] UpdateRecorderTaskProgressModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			_communicationManager.UpdateRecorderTaskProgress(serviceId, taskId, model.PercentComplete, model.Details, model.Status, model.EndTime);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		catch (DatabaseException)
		{
			return InternalServerError();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("{taskid:long}/success")]
	public IHttpActionResult RecorderTaskSuccess(Guid serviceId, long taskId, [FromBody] RecorderTaskSuccessModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			_communicationManager.UpdateRecorderTaskProgress(serviceId, taskId, 100, model.Result, 2, DateTime.UtcNow);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPatch]
	[Route("{taskid:long}/stopped")]
	public IHttpActionResult RecorderTaskStopped(Guid serviceId, long taskId)
	{
		try
		{
			_communicationManager.StopRecorderTask(serviceId, taskId);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		catch (DatabaseException)
		{
			return InternalServerError();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPatch]
	[Route("{taskid:long}/failed")]
	public IHttpActionResult RecorderTaskFailed(Guid serviceId, long taskId, [FromBody] RecorderTaskFailedModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		try
		{
			_communicationManager.UpdateRecorderTaskProgress(serviceId, taskId, 100, model.ExceptionMessage, 3, DateTime.UtcNow);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPatch]
	[Route("{taskid:long}/remove")]
	public IHttpActionResult RemoveRecorderTask(Guid serviceId, long taskId)
	{
		try
		{
			_communicationManager.RemoveRecorderTask(serviceId, taskId);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		catch (DatabaseException)
		{
			return InternalServerError();
		}
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpGet]
	[Route("{previoussequencenumber:long}/recordertasks")]
	public IHttpActionResult GetRecorderTasks(Guid serviceId, long previousSequenceNumber)
	{
		ServerTaskDto[] array = null;
		try
		{
			array = _communicationManager.GetRecorderTasks(serviceId, previousSequenceNumber);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		List<TaskModel> list = new List<TaskModel>();
		if (array != null)
		{
			ServerTaskDto[] array2 = array;
			foreach (ServerTaskDto serverTaskDto in array2)
			{
				TaskModel item = new TaskModel(serverTaskDto);
				list.Add(item);
			}
		}
		return Ok(new RecorderTaskModel
		{
			Tasks = list
		});
	}

	[HttpGet]
	[Route("recordertaskspendingstop")]
	public IHttpActionResult GetRecorderTasksWithPendingStop(Guid serviceId)
	{
		ServerTaskDto[] array = null;
		try
		{
			array = _communicationManager.GetRecorderTasksWithPendingStop(serviceId);
		}
		catch (RecorderNotFoundException)
		{
			return NotFound();
		}
		List<TaskModel> list = new List<TaskModel>();
		if (array != null)
		{
			ServerTaskDto[] array2 = array;
			foreach (ServerTaskDto serverTaskDto in array2)
			{
				TaskModel item = new TaskModel(serverTaskDto);
				list.Add(item);
			}
		}
		return Ok(new RecorderTaskModel
		{
			Tasks = list
		});
	}
}
