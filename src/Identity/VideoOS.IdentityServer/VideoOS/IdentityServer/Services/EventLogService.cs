using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Services;

public class EventLogService : IEventLogService
{
	private readonly IHttpContextAccessor _context;

	private readonly IEventLogRepository _logRepository;

	private readonly IDateTimeHelper _dateTimeHelper;

	public EventLogService(IHttpContextAccessor context, IEventLogRepository logRepository, IDateTimeHelper dateTimeHelper)
	{
		_context = context;
		_logRepository = logRepository;
		_dateTimeHelper = dateTimeHelper;
	}

	public async Task Log(EventLogModel logModel)
	{
		if (logModel == null)
		{
			throw new ArgumentNullException("logModel");
		}
		EventLog log = new EventLog
		{
			TimeStamp = _dateTimeHelper.UtcNow,
			Event = logModel.GetType().Name,
			UserName = logModel.Username,
			RemoteIpAddress = ((_context.HttpContext.Connection.RemoteIpAddress != null) ? _context.HttpContext.Connection.RemoteIpAddress.ToString() : string.Empty),
			ObjectId = logModel.ObjectId,
			ObjectName = logModel.ObjectName,
			ClientId = logModel.ClientId,
			ClientName = logModel.ClientName
		};
		_logRepository.CreateLog(log);
		await _logRepository.SaveChangesAsync();
	}
}
