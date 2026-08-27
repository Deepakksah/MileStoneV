using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Repositories.Interfaces;

public interface IEventLogRepository : IRepository
{
	void CreateLog(EventLog log);

	Task<IEnumerable<EventLog>> FilterLogs(Expression<Func<EventLog, bool>> filter, int skip, int take);

	Task<IEnumerable<EventLog>> GetLogs(int skip, int take);

	void DeleteLogsOlderThan(DateTimeOffset timeOffset);
}
