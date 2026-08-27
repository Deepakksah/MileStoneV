using System;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/drivers")]
public sealed class DriverController : ApiController
{
	private readonly IRecorderCommManager _communicationManager;

	public DriverController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("updatedrivernumbers")]
	public IHttpActionResult UpdateDriverNumbers(Guid serviceId, [FromBody] DriverListModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		Dictionary<Guid, int> dictionary = new Dictionary<Guid, int>();
		DriverModel[] drivers = model.Drivers;
		foreach (DriverModel driverModel in drivers)
		{
			if (dictionary.ContainsKey(driverModel.DriverId))
			{
				RecorderCommManager.Logger.LogWarning("Duplicate driver ID received from Recorder - second instance will be ignored: {DriverId}", driverModel.DriverId);
			}
			else
			{
				dictionary.Add(driverModel.DriverId, driverModel.DriverNumber);
			}
		}
		IDictionary<Guid, int> dictionary2 = null;
		try
		{
			dictionary2 = _communicationManager.UpdateDrivers(serviceId, dictionary);
		}
		catch (DatabaseConcurrencyException exception)
		{
			return InternalServerError(exception);
		}
		catch (ServerServiceException exception2)
		{
			return InternalServerError(exception2);
		}
		return Ok(dictionary2);
	}
}
