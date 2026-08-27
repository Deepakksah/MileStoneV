using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.ResourceParameters;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdpAuditEventPolicy")]
[Route("api/[controller]")]
[ApiController]
public class EventLogsController : BaseController
{
	private readonly IEventLogRepository _eventLogRepository;

	public EventLogsController(IEventLogRepository eventLogRepository)
	{
		_eventLogRepository = eventLogRepository;
	}

	[HttpGet]
	public async Task<IActionResult> Get([ModelBinder(BinderType = typeof(FilterModelBinder))] Expression<Func<EventLog, bool>> filter, [FromQuery] PaginationParameters paginationParameters)
	{
		return Ok(await _eventLogRepository.FilterLogs(filter, paginationParameters.Skip.Value, paginationParameters.Top.Value));
	}
}
