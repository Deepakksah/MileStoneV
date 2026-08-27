using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.DataAccess;
using VideoOS.SCH.Services.DataAccess.Generic;

namespace VideoOS.SCH.Services.Business.Components;

public class MessageWatch
{
	public List<MatchOperator> GetMatchOperators()
	{
		return GenericDataMgr.GetMessageWatchDataMgr().GetAllMatchOperators(includeDescriptions: true).Values.ToList();
	}

	public bool ValidateMessageWatch(VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatch, Dictionary<string, MatchOperator> allOperators)
	{
		if (string.IsNullOrEmpty(messageWatch.MessageTypeName))
		{
			ServiceContext.Current.AddErrAndLog(-204005, "Invalid MessageWatch fields; null or empty MessageTypeName not allowed.", LogType.Error);
			return false;
		}
		if (messageWatch.MessageTypeName.Length > 30)
		{
			ServiceContext.Current.AddErrAndLog(-204006, "Invalid MessageWatch fields; Message type name too long, cannot exceed " + 30 + " characters.", LogType.Error);
			return false;
		}
		foreach (MatchExpression expression in messageWatch.Expressions)
		{
			if (string.IsNullOrEmpty(expression.OperatorName))
			{
				ServiceContext.Current.AddErrAndLog(-203002, "Invalid MessageWatch fields; null or empty Operator Name not allowed.", LogType.Error);
				return false;
			}
			if (!allOperators.Keys.Contains(expression.OperatorName, StringComparer.OrdinalIgnoreCase))
			{
				ServiceContext.Current.AddErrAndLog(-203003, null, LogType.Error);
				return false;
			}
			if (string.IsNullOrEmpty(expression.Data.Key))
			{
				ServiceContext.Current.AddErrAndLog(-204007, "Invalid MessageWatch fields; null or empty Data Key not allowed.", LogType.Error);
				return false;
			}
			if (expression.Data.Key.Length > 25)
			{
				ServiceContext.Current.AddErrAndLog(-204008, "Invalid MessageWatch fields; Message Data Key too long, cannot exceed " + 25 + " characters.", LogType.Error);
				return false;
			}
			if (string.IsNullOrEmpty(expression.Data.Value))
			{
				ServiceContext.Current.AddErrAndLog(-204009, "Invalid MessageWatch fields; null or empty Data Value not allowed.", LogType.Error);
				return false;
			}
			if (expression.Data.Value.Length > 50)
			{
				ServiceContext.Current.AddErrAndLog(-204010, "Invalid MessageWatch fields; Message Data Value too long, cannot exceed " + 50 + " characters.", LogType.Error);
				return false;
			}
		}
		return true;
	}

	public void AddMessageWatch(Guid sessionID, string messageTypeName)
	{
		GenericDataMgr.GetMessageWatchDataMgr().CreateMessageWatch(sessionID, messageTypeName);
	}

	public int AddMatchExpressions(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> watchItems)
	{
		int num = 0;
		if (watchItems == null)
		{
			ServiceContext.Current.AddErrAndLog(-200004, "Argument MessageWatch array cannot be null in AddMessageWatchItems()", LogType.Error);
			return 0;
		}
		IMessageWatchDataMgr messageWatchDataMgr = GenericDataMgr.GetMessageWatchDataMgr();
		Dictionary<string, MatchOperator> allMatchOperators = messageWatchDataMgr.GetAllMatchOperators(includeDescriptions: false);
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch watchItem in watchItems)
		{
			if (ValidateMessageWatch(watchItem, allMatchOperators) && messageWatchDataMgr.CreateMatchExpressions(watchItem) > 0)
			{
				hashSet.Add(watchItem.SessionID);
				num++;
			}
		}
		Session.SetSessionExpireTime(hashSet);
		return num;
	}

	public bool RemoveMessageWatch(Guid sessionID, string messageTypeName)
	{
		return GenericDataMgr.GetMessageWatchDataMgr().DeleteMessageWatch(sessionID, messageTypeName);
	}

	public void RemoveMatchExpressions(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> messageWatchList)
	{
		if (messageWatchList == null)
		{
			ServiceContext.Current.AddErrAndLog(-200004, "Argument MessageWatch array cannot be null in RemoveMatchExpressions()", LogType.Error);
			return;
		}
		IMessageWatchDataMgr messageWatchDataMgr = GenericDataMgr.GetMessageWatchDataMgr();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatch in messageWatchList)
		{
			messageWatchDataMgr.DeleteMatchExpressions(messageWatch);
			Session.SetSessionExpireTime(messageWatch.SessionID);
		}
	}

	public void RemoveMessageWatchers(Guid sessionID)
	{
		GenericDataMgr.GetMessageWatchDataMgr().DeleteMessageWatchers(sessionID);
		Session.SetSessionExpireTime(sessionID);
	}

	private bool BreakingMatchFound(MatchExpression matchExpr, Dictionary<string, MessageData> messageData)
	{
		foreach (MessageData value in messageData.Values)
		{
			if (!matchExpr.Data.Key.Equals(value.Key, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			if (matchExpr.OperatorName.Equals("equals", StringComparison.OrdinalIgnoreCase))
			{
				if (!matchExpr.Data.Value.Equals(value.Value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				return false;
			}
			if (matchExpr.OperatorName.Equals("startsWith", StringComparison.OrdinalIgnoreCase))
			{
				if (!value.Value.StartsWith(matchExpr.Data.Value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				return false;
			}
			if (matchExpr.OperatorName.Equals("endsWith", StringComparison.OrdinalIgnoreCase))
			{
				if (!value.Value.EndsWith(matchExpr.Data.Value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				return false;
			}
			if (matchExpr.OperatorName.Equals("like", StringComparison.OrdinalIgnoreCase))
			{
				if (value.Value.IndexOf(matchExpr.Data.Value, StringComparison.OrdinalIgnoreCase) < 0)
				{
					return true;
				}
				return false;
			}
			ServiceContext.Current.AddErrAndLog(-203003, "Invalid operator name found in message watch data: '" + matchExpr.OperatorName + "'.", LogType.Error);
			return true;
		}
		if (ConfigSettings.MessageTypeEnforced)
		{
			throw new ServiceChannelException(-204004, "Message watch data key not found in message type.");
		}
		return true;
	}

	public List<Guid> GetInterestedSessionIDs(string messageTypeName, Dictionary<string, MessageData> messageData)
	{
		List<Guid> list = new List<Guid>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatcher in GenericDataMgr.GetMessageWatchDataMgr().GetMessageWatchers(messageTypeName))
		{
			bool flag = false;
			foreach (MatchExpression expression in messageWatcher.Expressions)
			{
				if (BreakingMatchFound(expression, messageData))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(messageWatcher.SessionID);
			}
		}
		return list.Distinct().ToList();
	}

	public bool DeleteMessageWatchOrphans()
	{
		return GenericDataMgr.GetMessageWatchDataMgr().DeleteMessageWatchOrphans();
	}
}
