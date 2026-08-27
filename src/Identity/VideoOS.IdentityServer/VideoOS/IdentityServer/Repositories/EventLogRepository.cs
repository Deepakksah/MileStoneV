using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Repositories;

public class EventLogRepository : IEventLogRepository, IRepository
{
	private readonly IApplicationDbContext _dbContext;

	public EventLogRepository(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void CreateLog(EventLog log)
	{
		_dbContext.EventLogs.Add(log);
	}

	public Task<IEnumerable<EventLog>> GetLogs(int skip, int take)
	{
		return FilterLogs(null, skip, take);
	}

	public async Task<IEnumerable<EventLog>> FilterLogs(Expression<Func<EventLog, bool>> filter, int skip, int take)
	{
		if (skip < 0)
		{
			throw new ArgumentOutOfRangeException("skip");
		}
		if (take < 0)
		{
			throw new ArgumentOutOfRangeException("take");
		}
		if (filter == null)
		{
			return await _dbContext.EventLogs.OrderBy((EventLog e) => e.Id).Paginate(skip, take).ToListAsync();
		}
		return await (from e in _dbContext.EventLogs.Where(filter)
			orderby e.Id
			select e).Paginate(skip, take).ToListAsync();
	}

	public void DeleteLogsOlderThan(DateTimeOffset timeOffset)
	{
		IQueryable<EventLog> entities = _dbContext.EventLogs.Where((EventLog l) => l.TimeStamp < timeOffset);
		_dbContext.EventLogs.RemoveRange(entities);
	}

	public async Task SaveChangesAsync()
	{
		if (_dbContext is ApplicationDbContext applicationDbContext)
		{
			await applicationDbContext.SaveChangesAsync();
			return;
		}
		throw new NotSupportedException("SaveChangesAsync is only supported for ApplicationDbContext");
	}
}
