using System;
using System.Collections.Generic;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.DataAccess;

public interface IMessageWatchDataMgr
{
	void CreateMessageWatch(Guid sessionID, string messageTypeName);

	bool DeleteMessageWatch(Guid sessionID, string messageTypeName);

	int CreateMatchExpressions(MessageWatch messageWatch);

	void DeleteMatchExpressions(MessageWatch messageWatch);

	bool DeleteMessageWatchers(Guid sessionID);

	bool DeleteMessageWatchOrphans();

	Dictionary<string, MatchOperator> GetAllMatchOperators(bool includeDescriptions);

	List<MessageWatch> GetMessageWatchers();

	List<MessageWatch> GetMessageWatchers(string messageTypeName);
}
