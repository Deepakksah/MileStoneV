using System;
using System.Collections.Generic;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.DataAccess;

public interface IMessageDataMgr
{
	int CreateMessage(string messageTypeName, Dictionary<string, MessageData> messageData, List<Guid> interestedSessions);

	Dictionary<int, Message> GetAndDeleteSessionMessages(Guid sessionID, int returnMaxSize, out bool moreMsgsToRet);

	Dictionary<int, CreatedSessionMessageItem> GetSessionMessagesLaterThan(DateTime createTime);

	bool DeleteNonSessionMessages();

	int GetLastMessageID();

	List<Message> GetMessages(int returnMaxSize, out bool moreMsgsToRet);

	bool GetTotalSessionMsgInfo(out int totalMsgCount, out DateTime latestMsgCreateTime);

	List<SessionMsgInfo> GetSessionMsgInfoList();
}
