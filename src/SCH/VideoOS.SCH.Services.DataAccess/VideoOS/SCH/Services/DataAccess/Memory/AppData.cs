using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.DataAccess.Memory;

public class AppData : ISessionDataMgr, IMessageDataMgr, IMessageWatchDataMgr, IBatchJobDataMgr
{
	private class Session
	{
		public Guid ID { get; set; }

		public HashSet<int> MessageIDs { get; set; }

		public string ClientName { get; set; }

		public string ClientUserName { get; set; }

		public DateTime ExpiryTime { get; set; }

		public Session(Guid sessionID, HashSet<int> messageIDs, string clientName, string clientUserName, DateTime expiryTime)
		{
			ID = sessionID;
			MessageIDs = messageIDs;
			ClientName = clientName;
			ClientUserName = clientUserName;
			ExpiryTime = expiryTime;
		}

		public Session(HashSet<int> messageIDs, string clientName, string clientUserName, DateTime expiryTime)
			: this(Guid.NewGuid(), messageIDs, clientName, clientUserName, expiryTime)
		{
		}
	}

	private class Message
	{
		public int ID { get; set; }

		public string MessageTypeName { get; set; }

		public DateTime Created { get; set; }

		public Dictionary<string, MessageData> Data { get; set; }

		public Message(int id, string messageTypeName, DateTime created, Dictionary<string, MessageData> data)
		{
			ID = id;
			MessageTypeName = messageTypeName;
			Created = created;
			Data = data;
		}
	}

	private class MessageData
	{
		public string Key { get; set; }

		public string Value { get; set; }

		public MessageData(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}

	private class MessageWatch
	{
		public Guid SessionID { get; set; }

		public string MessageTypeName { get; set; }

		public List<MatchExpression> Expressions { get; set; }

		public MessageWatch(Guid sessionID, string messageTypeName, List<MatchExpression> data)
		{
			SessionID = sessionID;
			MessageTypeName = messageTypeName;
			if (data == null)
			{
				Expressions = new List<MatchExpression>();
			}
			else
			{
				Expressions = data;
			}
		}

		public MessageWatch(Guid sessionID, string messageTypeName)
			: this(sessionID, messageTypeName, null)
		{
		}
	}

	private class MatchExpression
	{
		public MessageData Data { get; set; }

		public string OperatorName { get; set; }

		public MatchExpression(string key, string value, string operatorName)
		{
			Data = new MessageData(key, value);
			OperatorName = operatorName;
		}
	}

	private class MatchOperator
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public MatchOperator(string name, string description)
		{
			Name = name;
			Description = description;
		}
	}

	private class BatchJobStatus
	{
		public string BatchJobID { get; set; }

		public int Status { get; set; }

		public int NoofFailedStarts { get; set; }

		public DateTime LastSuccess { get; set; }

		public string LastJobMessage { get; set; }

		public BatchJobStatus(string batchJobID, int status, int noofFailedStarts, DateTime lastSuccess, string lastJobMessage)
		{
			BatchJobID = batchJobID;
			Status = status;
			NoofFailedStarts = noofFailedStarts;
			LastSuccess = lastSuccess;
			LastJobMessage = lastJobMessage;
		}
	}

	private static AppData _theData;

	private Mutex _dbLock = new Mutex(initiallyOwned: true);

	private Dictionary<Guid, Session> _sessions = new Dictionary<Guid, Session>();

	private Dictionary<int, Message> _messages = new Dictionary<int, Message>();

	private List<MessageWatch> _watchers = new List<MessageWatch>();

	private Dictionary<string, MatchOperator> _matchOperators = new Dictionary<string, MatchOperator>();

	private Dictionary<string, BatchJobStatus> _batchJobStatus = new Dictionary<string, BatchJobStatus>();

	private int _lastMessageID;

	public static AppData Current => _theData;

	private static void InitData()
	{
		_theData = new AppData();
		try
		{
			string text = "equals";
			_theData._matchOperators.Add(text, new MatchOperator(text, "'Equals' operator. Returns true when the data value equals the match expression value."));
			text = "startsWith";
			_theData._matchOperators.Add(text, new MatchOperator(text, "'Starts with' operator. Returns true when the data value starts with the match expression value."));
			text = "endsWith";
			_theData._matchOperators.Add(text, new MatchOperator(text, "'Ends with' operator. Returns true when the data value ends with the match expression value."));
			text = "like";
			_theData._matchOperators.Add(text, new MatchOperator(text, "'Like' operator. Returns true when the match expression value is found somewhere in the data value."));
		}
		finally
		{
			_theData._dbLock.ReleaseMutex();
		}
	}

	static AppData()
	{
		InitData();
	}

	public void ClearData()
	{
		_dbLock.WaitOne(-1);
		try
		{
			InitData();
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	private bool ValidateSessionData(string clientName, string clientUserName)
	{
		if (string.IsNullOrEmpty(clientName))
		{
			ServiceContext.Current.AddErrAndLog(-206008, "Invalid Session fields; null or empty ClientName not allowed.", LogType.Error);
			return false;
		}
		return true;
	}

	public bool CreateSession(string clientName, string clientUserName, DateTime expiryTime, out Guid sessionID)
	{
		if (!ValidateSessionData(clientName, clientUserName))
		{
			sessionID = Guid.Empty;
			return false;
		}
		_dbLock.WaitOne(-1);
		try
		{
			Session session = new Session(new HashSet<int>(), clientName, clientUserName, expiryTime);
			_sessions.Add(session.ID, session);
			sessionID = session.ID;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Session GetSession(Guid sessionID)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (_sessions.ContainsKey(sessionID))
			{
				Session session = _sessions[sessionID];
				return new VideoOS.SCH.Services.Business.Definitions.Custom.Session(session.ID, session.MessageIDs.ToList(), session.ClientName, session.ClientUserName, session.ExpiryTime);
			}
			return null;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool DeleteSession(Guid sessionID)
	{
		bool flag = false;
		_dbLock.WaitOne(-1);
		try
		{
			bool num = DeleteMessageWatchersPrivate(sessionID, null);
			flag = _sessions.Remove(sessionID);
			if (!flag)
			{
				ServiceContext current = ServiceContext.Current;
				Guid guid = sessionID;
				current.AddErrAndLog(-201002, "Failed to remove session " + guid.ToString() + ".", LogType.Error);
			}
			return num && flag;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool SessionExists(Guid sessionID)
	{
		_dbLock.WaitOne(-1);
		try
		{
			return _sessions.ContainsKey(sessionID);
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public List<Guid> GetSessionIDsWithExpiryTimeLessThan(DateTime expireTime)
	{
		_dbLock.WaitOne(-1);
		try
		{
			return (from session in _sessions.Values
				where session.ExpiryTime < expireTime
				select session.ID).ToList();
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public Dictionary<Guid, VideoOS.SCH.Services.Business.Definitions.Custom.Session> GetAllSessions()
	{
		_dbLock.WaitOne(-1);
		try
		{
			Dictionary<Guid, VideoOS.SCH.Services.Business.Definitions.Custom.Session> dictionary = new Dictionary<Guid, VideoOS.SCH.Services.Business.Definitions.Custom.Session>();
			foreach (Session value in _sessions.Values)
			{
				dictionary.Add(value.ID, new VideoOS.SCH.Services.Business.Definitions.Custom.Session(value.ID, value.MessageIDs.ToList(), value.ClientName, value.ClientUserName, value.ExpiryTime));
			}
			return dictionary;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public void SetSessionExpireTime(IEnumerable<Guid> sessionIDs, DateTime expiryTime)
	{
		if (sessionIDs == null)
		{
			ServiceContext.Current.AddErrAndLog(-200004, "Invalid session ID array; null reference not allowed in SetSessionExpireTime().", LogType.Error);
			return;
		}
		_dbLock.WaitOne(-1);
		try
		{
			foreach (Guid sessionID in sessionIDs)
			{
				if (_sessions.ContainsKey(sessionID))
				{
					_sessions[sessionID].ExpiryTime = expiryTime;
				}
				else
				{
					ServiceContext.Current.AddErrAndLog(-201000, "Session not found in SetSessionExpireTime().", LogType.Error);
				}
			}
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public List<ServiceChannelClient> GetAllClients()
	{
		_dbLock.WaitOne(-1);
		try
		{
			List<ServiceChannelClient> list = new List<ServiceChannelClient>();
			foreach (Session value in _sessions.Values)
			{
				list.Add(new ServiceChannelClient(value.ClientName, null));
			}
			return list;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	private bool ValidateMessageData(string messageTypeName, Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> messageData)
	{
		if (string.IsNullOrEmpty(messageTypeName))
		{
			ServiceContext.Current.AddErrAndLog(-206008, "Invalid Message fields; null or empty MessageTypeName not allowed.", LogType.Error);
			return false;
		}
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageData value in messageData.Values)
		{
			if (string.IsNullOrEmpty(value.Value))
			{
				ServiceContext.Current.AddErrAndLog(-206008, "Invalid MessageData fields; null or empty Value not allowed.", LogType.Error);
				return false;
			}
		}
		return true;
	}

	public int CreateMessage(string messageTypeName, Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> messageData, List<Guid> interestedSessions)
	{
		int num = 0;
		if (!ValidateMessageData(messageTypeName, messageData))
		{
			return 0;
		}
		_dbLock.WaitOne(-1);
		try
		{
			if (interestedSessions.Count > 0)
			{
				int num2 = ++_lastMessageID;
				Dictionary<string, MessageData> dictionary = new Dictionary<string, MessageData>();
				foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageData value in messageData.Values)
				{
					dictionary.Add(value.Key, new MessageData(value.Key, value.Value));
				}
				_messages.Add(num2, new Message(num2, messageTypeName, DateTime.UtcNow, dictionary));
				foreach (Guid interestedSession in interestedSessions)
				{
					if (_sessions.ContainsKey(interestedSession))
					{
						if (_sessions[interestedSession].MessageIDs.Contains(num2))
						{
							ServiceContext.Current.AddErrAndLog(-206003, "Newly created message ID (" + num2 + ") already exist in Session.MessageIDs.", LogType.Warning);
						}
						_sessions[interestedSession].MessageIDs.Add(num2);
						num++;
					}
					else
					{
						ServiceContext.Current.AddErrAndLog(-206009, "Referential integrity error: Session reference between AppData.Session and AppData.Message invalid.", LogType.Error);
					}
				}
			}
			return num;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public Dictionary<int, VideoOS.SCH.Services.Business.Definitions.Custom.Message> GetAndDeleteSessionMessages(Guid sessionID, int returnMaxSize, out bool moreMsgsToRet)
	{
		Dictionary<int, VideoOS.SCH.Services.Business.Definitions.Custom.Message> dictionary = null;
		int num = 0;
		_dbLock.WaitOne(-1);
		try
		{
			if (_sessions.ContainsKey(sessionID))
			{
				Session session = _sessions[sessionID];
				if (session.MessageIDs.Count > 0)
				{
					Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> dictionary2 = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData>();
					dictionary = new Dictionary<int, VideoOS.SCH.Services.Business.Definitions.Custom.Message>();
					while (session.MessageIDs.Count > 0)
					{
						int num2 = session.MessageIDs.ElementAt(0);
						if (_messages.ContainsKey(num2))
						{
							Message message = _messages[num2];
							dictionary2.Clear();
							foreach (MessageData value in message.Data.Values)
							{
								num += value.Key.Length;
								num += value.Value.Length;
								dictionary2.Add(value.Key, new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(value.Key, value.Value));
							}
							num += 42;
							num += message.MessageTypeName.Length;
							if (num > returnMaxSize)
							{
								moreMsgsToRet = true;
								return dictionary;
							}
							dictionary.Add(message.ID, new VideoOS.SCH.Services.Business.Definitions.Custom.Message(message.ID, message.MessageTypeName, message.Created, dictionary2));
						}
						else
						{
							ServiceContext.Current.AddErrAndLog(-206000, "Referential integrity error: Message referenced in AppData.Sessions not found in AppData.Messages.", LogType.Error);
						}
						session.MessageIDs.Remove(num2);
					}
				}
			}
			else
			{
				ServiceContext.Current.AddErrAndLog(-201000, "Unknown session in GetAndDeleteSessionMessages(); Session " + sessionID.ToString() + " not found in memory data.", LogType.Error);
			}
			moreMsgsToRet = false;
			return dictionary;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public Dictionary<int, CreatedSessionMessageItem> GetSessionMessagesLaterThan(DateTime createTime)
	{
		throw new NotImplementedException();
	}

	public bool DeleteNonSessionMessages()
	{
		bool result = true;
		_dbLock.WaitOne(-1);
		try
		{
			Dictionary<int, string> sessMessages = new Dictionary<int, string>();
			foreach (Session value in _sessions.Values)
			{
				foreach (int messageID in value.MessageIDs)
				{
					if (!sessMessages.ContainsKey(messageID))
					{
						sessMessages.Add(messageID, null);
					}
				}
			}
			foreach (int item in (from msg in _messages.Values
				where !sessMessages.ContainsKey(msg.ID)
				select msg.ID).ToList())
			{
				if (!_messages.Remove(item))
				{
					result = false;
				}
			}
			return result;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public int GetLastMessageID()
	{
		_dbLock.WaitOne(-1);
		try
		{
			return _lastMessageID;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public List<VideoOS.SCH.Services.Business.Definitions.Custom.Message> GetMessages(int returnMaxSize, out bool moreMsgsToRet)
	{
		int num = 0;
		_dbLock.WaitOne(-1);
		try
		{
			List<VideoOS.SCH.Services.Business.Definitions.Custom.Message> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.Message>();
			foreach (Message item in _messages.Values.Select((Message message) => message))
			{
				if (num >= returnMaxSize)
				{
					moreMsgsToRet = true;
					return list;
				}
				Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> dictionary = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData>();
				foreach (MessageData value in item.Data.Values)
				{
					num += value.Key.Length;
					num += value.Value.Length;
					dictionary.Add(value.Key, new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(value.Key, value.Value));
				}
				num += 42;
				num += item.MessageTypeName.Length;
				list.Add(new VideoOS.SCH.Services.Business.Definitions.Custom.Message(item.ID, item.MessageTypeName, item.Created, dictionary));
			}
			moreMsgsToRet = false;
			return list;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool GetTotalSessionMsgInfo(out int totalMsgCount, out DateTime latestMsgCreateTime)
	{
		totalMsgCount = 0;
		latestMsgCreateTime = DateTime.MinValue;
		foreach (Session value in _sessions.Values)
		{
			totalMsgCount += value.MessageIDs.Count;
			foreach (int messageID in value.MessageIDs)
			{
				if (_messages[messageID].Created > latestMsgCreateTime)
				{
					latestMsgCreateTime = _messages[messageID].Created;
				}
			}
		}
		return true;
	}

	public List<SessionMsgInfo> GetSessionMsgInfoList()
	{
		List<SessionMsgInfo> list = new List<SessionMsgInfo>();
		foreach (Session value in _sessions.Values)
		{
			DateTime dateTime = DateTime.MinValue;
			foreach (int messageID in value.MessageIDs)
			{
				if (_messages[messageID].Created > dateTime)
				{
					dateTime = _messages[messageID].Created;
				}
			}
			list.Add(new SessionMsgInfo(value.ID, value.MessageIDs.Count, dateTime));
		}
		return list;
	}

	private bool EqualExpressions(VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression expr1, VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression expr2)
	{
		if (expr1.Data.Key == expr2.Data.Key && expr1.OperatorName == expr2.OperatorName && expr1.Data.Value == expr2.Data.Value)
		{
			return true;
		}
		return false;
	}

	private bool EqualExpressions(MatchExpression expr1, MatchExpression expr2)
	{
		if (expr1.Data.Key == expr2.Data.Key && expr1.OperatorName == expr2.OperatorName && expr1.Data.Value == expr2.Data.Value)
		{
			return true;
		}
		return false;
	}

	private bool EqualExpressions(MatchExpression expr1, VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression expr2)
	{
		if (expr1.Data.Key == expr2.Data.Key && expr1.OperatorName == expr2.OperatorName && expr1.Data.Value == expr2.Data.Value)
		{
			return true;
		}
		return false;
	}

	public bool CheckForDuplicateExpressions(List<VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression> busExpr)
	{
		List<VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression item in busExpr)
		{
			foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression item2 in list)
			{
				if (EqualExpressions(item, item2))
				{
					return true;
				}
			}
			list.Add(item);
		}
		return false;
	}

	private bool ExpressionExistInList(MatchExpression matchExpr, List<MatchExpression> memExpressions)
	{
		foreach (MatchExpression memExpression in memExpressions)
		{
			if (EqualExpressions(matchExpr, memExpression))
			{
				return true;
			}
		}
		return false;
	}

	private bool ExpressionExistInList(MatchExpression memExpr, List<VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression> busExpressions)
	{
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression busExpression in busExpressions)
		{
			if (EqualExpressions(memExpr, busExpression))
			{
				return true;
			}
		}
		return false;
	}

	private bool ValidateMessageWatch(VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatch)
	{
		if (string.IsNullOrEmpty(messageWatch.MessageTypeName))
		{
			ServiceContext.Current.AddErrAndLog(-206008, "Invalid MessageWatch fields; null or empty MessageTypeName not allowed.", LogType.Error);
			return false;
		}
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression expression in messageWatch.Expressions)
		{
			if (string.IsNullOrEmpty(expression.OperatorName))
			{
				ServiceContext.Current.AddErrAndLog(-206008, "Invalid MatchExpression fields; null or empty OperatorName not allowed.", LogType.Error);
				return false;
			}
			if (expression.Data == null)
			{
				ServiceContext.Current.AddErrAndLog(-206008, "Invalid MatchExpression fields; null MessageData not allowed.", LogType.Error);
				return false;
			}
			if (string.IsNullOrEmpty(expression.Data.Key) || string.IsNullOrEmpty(expression.Data.Value))
			{
				ServiceContext.Current.AddErrAndLog(-206008, "Invalid MatchExpression fields; null or empty MessageData Key or Value not allowed.", LogType.Error);
				return false;
			}
		}
		if (CheckForDuplicateExpressions(messageWatch.Expressions))
		{
			ServiceContext.Current.AddErrAndLog(-206005, "Invalid MatchExpressions; duplicate expressions are not allowed.", LogType.Error);
			return false;
		}
		return true;
	}

	public void CreateMessageWatch(Guid sessionID, string messageTypeName)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (_sessions.ContainsKey(sessionID))
			{
				_watchers.Add(new MessageWatch(sessionID, messageTypeName, null));
			}
			else
			{
				ServiceContext.Current.AddErrAndLog(-206002, "Invalid session reference in MessageWatch; Session " + sessionID.ToString() + " not found in memory data when creating a message watch.", LogType.Error);
			}
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public int CreateMatchExpressions(VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatch)
	{
		_dbLock.WaitOne(-1);
		try
		{
			int num = 0;
			if (_sessions.ContainsKey(messageWatch.SessionID))
			{
				if (ValidateMessageWatch(messageWatch))
				{
					List<MessageWatch> messageWatchersPrivate = GetMessageWatchersPrivate(messageWatch.SessionID, messageWatch.MessageTypeName);
					List<MatchExpression> list = new List<MatchExpression>();
					foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression expression in messageWatch.Expressions)
					{
						list.Add(new MatchExpression(expression.Data.Key, expression.Data.Value, expression.OperatorName));
					}
					if (messageWatchersPrivate.Count == 0)
					{
						_watchers.Add(new MessageWatch(messageWatch.SessionID, messageWatch.MessageTypeName, list));
						return list.Count;
					}
					if (messageWatchersPrivate.Count > 1)
					{
						ServiceContext.Current.AddErrAndLog(-206005, "Invalid MatchExpressions; duplicate expressions are not allowed when creating a MessageWatch.", LogType.Error);
					}
					List<MatchExpression> list2 = new List<MatchExpression>();
					foreach (MessageWatch item in messageWatchersPrivate)
					{
						foreach (MatchExpression item2 in list)
						{
							if (ExpressionExistInList(item2, item.Expressions))
							{
								ServiceContext.Current.AddErrAndLog(-206005, "Invalid MatchExpression; duplicate expressions are not allowed when creating a MatchExpression.", LogType.Error);
							}
							else
							{
								list2.Add(item2);
							}
						}
						foreach (MatchExpression item3 in list2)
						{
							item.Expressions.Add(item3);
							num++;
						}
					}
				}
			}
			else
			{
				ServiceContext.Current.AddErrAndLog(-206002, "Invalid session reference in MessageWatch; Session " + messageWatch.SessionID.ToString() + " not found in memory data when creating match expressions.", LogType.Error);
			}
			return num;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	private string CreateExprListForLog(MessageWatch watchItem)
	{
		string text = string.Empty;
		foreach (MatchExpression expression in watchItem.Expressions)
		{
			text = text + "'" + expression.Data.Key + "' " + expression.OperatorName + " '" + expression.Data.Value + "';";
		}
		return text;
	}

	private bool DeleteMessageWatchersPrivate(Guid sessionID, string messageTypeName)
	{
		bool result = true;
		IEnumerable<MessageWatch> source = ((messageTypeName != null) ? _watchers.Where((MessageWatch messageWatch) => messageWatch.SessionID == sessionID && messageWatch.MessageTypeName == messageTypeName) : _watchers.Where((MessageWatch messageWatch) => messageWatch.SessionID == sessionID));
		foreach (MessageWatch item in source.ToList())
		{
			if (!_watchers.Remove(item))
			{
				result = false;
				ServiceContext current2 = ServiceContext.Current;
				Guid guid = sessionID;
				current2.LogToServerLog(-203001, "Failed to remove a watch item for session " + guid.ToString() + ". Expressions: " + CreateExprListForLog(item), LogType.Error);
			}
		}
		return result;
	}

	public bool DeleteMessageWatch(Guid sessionID, string messageTypeName)
	{
		_dbLock.WaitOne(-1);
		try
		{
			return DeleteMessageWatchersPrivate(sessionID, messageTypeName);
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool DeleteMessageWatchers(Guid sessionID)
	{
		_dbLock.WaitOne(-1);
		try
		{
			return DeleteMessageWatchersPrivate(sessionID, null);
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public void DeleteMatchExpressions(VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch messageWatch)
	{
		_dbLock.WaitOne(-1);
		try
		{
			foreach (MessageWatch item in GetMessageWatchersPrivate(messageWatch.SessionID, messageWatch.MessageTypeName))
			{
				List<MatchExpression> list = new List<MatchExpression>();
				foreach (MatchExpression expression in item.Expressions)
				{
					if (ExpressionExistInList(expression, messageWatch.Expressions))
					{
						list.Add(expression);
					}
				}
				foreach (MatchExpression item2 in list)
				{
					if (!item.Expressions.Remove(item2))
					{
						ServiceContext.Current.AddErrAndLog(-203001, "Failed to remove a watch item for session " + item.SessionID.ToString() + " and msgTypeName '" + item.MessageTypeName + "'. Expressions: " + CreateExprListForLog(item), LogType.Error);
					}
				}
			}
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool DeleteMessageWatchOrphans()
	{
		bool result = true;
		_dbLock.WaitOne(-1);
		try
		{
			foreach (MessageWatch item in _watchers.Where((MessageWatch msgWatch) => !_sessions.ContainsKey(msgWatch.SessionID)).ToList())
			{
				if (!_watchers.Remove(item))
				{
					result = false;
				}
			}
			return result;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator> GetAllMatchOperators(bool includeDescriptions)
	{
		_dbLock.WaitOne(-1);
		try
		{
			Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator> dictionary = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator>();
			foreach (MatchOperator value in _matchOperators.Values)
			{
				VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator matchOperator = ((!includeDescriptions) ? new VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator(value.Name, null) : new VideoOS.SCH.Services.Business.Definitions.Custom.MatchOperator(value.Name, value.Description));
				dictionary.Add(matchOperator.Name, matchOperator);
			}
			return dictionary;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	private List<MessageWatch> GetMessageWatchersPrivate(Guid sessionID, string messageTypeName)
	{
		if (messageTypeName == null)
		{
			throw new ArgumentNullException("messageTypeName", "messageTypeName cannot be null when looking for MessageWatch'ers for a SessionID-MessageTypeName combination.");
		}
		return _watchers.Where((MessageWatch watch) => watch.SessionID == sessionID && watch.MessageTypeName == messageTypeName).ToList();
	}

	private List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> GetMessageWatchersPrivate(string messageTypeName)
	{
		List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch>();
		IEnumerable<MessageWatch> enumerable = ((messageTypeName != null) ? _watchers.Where((MessageWatch watch) => watch.MessageTypeName.Equals(messageTypeName, StringComparison.OrdinalIgnoreCase)) : _watchers.Select((MessageWatch watch) => watch));
		foreach (MessageWatch item in enumerable)
		{
			List<VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression> list2 = new List<VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression>();
			foreach (MatchExpression expression in item.Expressions)
			{
				list2.Add(new VideoOS.SCH.Services.Business.Definitions.Custom.MatchExpression(new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(expression.Data.Key, expression.Data.Value), expression.OperatorName));
			}
			list.Add(new VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch(item.SessionID, item.MessageTypeName, list2));
		}
		return list;
	}

	public List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> GetMessageWatchers(string messageTypeName)
	{
		_dbLock.WaitOne(-1);
		try
		{
			return GetMessageWatchersPrivate(messageTypeName);
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public List<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> GetMessageWatchers()
	{
		_dbLock.WaitOne(-1);
		try
		{
			return GetMessageWatchersPrivate(null);
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	private bool ValidateJobStatus(VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus batchJobStatus)
	{
		if (batchJobStatus == null)
		{
			ServiceContext.Current.AddErrAndLog(-200004, "Invalid batch job object; null reference not allowed.", LogType.Error);
			return false;
		}
		if (string.IsNullOrEmpty(batchJobStatus.BatchJobID))
		{
			ServiceContext.Current.AddErrAndLog(-206004, "Invalid batch job object; null or empty BatchJobID not allowed.", LogType.Error);
			return false;
		}
		return true;
	}

	public bool CreateJobStatus(VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus batchJobStatus)
	{
		if (!ValidateJobStatus(batchJobStatus))
		{
			return false;
		}
		_dbLock.WaitOne(-1);
		try
		{
			if (_batchJobStatus.ContainsKey(batchJobStatus.BatchJobID))
			{
				ServiceContext.Current.AddErrAndLog(-206005, "Cannot add BatchJobStatus, BatchJobID '" + batchJobStatus.BatchJobID + "' would not be unique.", LogType.Error);
				return false;
			}
			BatchJobStatus batchJobStatus2 = new BatchJobStatus(batchJobStatus.BatchJobID, batchJobStatus.Status, batchJobStatus.NoofFailedStarts, batchJobStatus.LastSuccess, batchJobStatus.LastJobMessage);
			_batchJobStatus.Add(batchJobStatus2.BatchJobID, batchJobStatus2);
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public List<VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus> GetBatchJobStatus(string batchJobID)
	{
		_dbLock.WaitOne(-1);
		try
		{
			List<VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus> list = new List<VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus>();
			if (string.IsNullOrEmpty(batchJobID))
			{
				foreach (BatchJobStatus value in _batchJobStatus.Values)
				{
					list.Add(new VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus(value.BatchJobID, value.Status, value.NoofFailedStarts, value.LastSuccess, value.LastJobMessage));
				}
			}
			else if (_batchJobStatus.ContainsKey(batchJobID))
			{
				BatchJobStatus batchJobStatus = _batchJobStatus[batchJobID];
				list.Add(new VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus(batchJobStatus.BatchJobID, batchJobStatus.Status, batchJobStatus.NoofFailedStarts, batchJobStatus.LastSuccess, batchJobStatus.LastJobMessage));
			}
			return list;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool UpdateJobStatus(VideoOS.SCH.Services.Business.Definitions.Custom.BatchJobStatus batchJobStatus)
	{
		if (!ValidateJobStatus(batchJobStatus))
		{
			return false;
		}
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(batchJobStatus.BatchJobID))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot update BatchJobStatus; BatchJobID '" + batchJobStatus.BatchJobID + "' not found.", LogType.Error);
				return false;
			}
			BatchJobStatus batchJobStatus2 = _batchJobStatus[batchJobStatus.BatchJobID];
			batchJobStatus2.Status = batchJobStatus.Status;
			batchJobStatus2.NoofFailedStarts = batchJobStatus.NoofFailedStarts;
			batchJobStatus2.LastSuccess = batchJobStatus.LastSuccess;
			batchJobStatus2.LastJobMessage = batchJobStatus.LastJobMessage;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusOfUpdatedRecord, int statusToUpdate)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(jobIDOfUpdatedRecord))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot update BatchJobStatus; BatchJobID '" + jobIDOfUpdatedRecord + "' not found.", LogType.Error);
				return false;
			}
			BatchJobStatus batchJobStatus = _batchJobStatus[jobIDOfUpdatedRecord];
			if (batchJobStatus.Status != statusOfUpdatedRecord)
			{
				ServiceContext.Current.AddErrAndLog(-206007, "Cannot update BatchJobStatus; BatchJobID '" + jobIDOfUpdatedRecord + "' and Status = " + statusOfUpdatedRecord + " not found.", LogType.Error);
				return false;
			}
			batchJobStatus.Status = statusToUpdate;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusToUpdate)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(jobIDOfUpdatedRecord))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot update BatchJobStatus; BatchJobID '" + jobIDOfUpdatedRecord + "' not found.", LogType.Error);
				return false;
			}
			_batchJobStatus[jobIDOfUpdatedRecord].Status = statusToUpdate;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool SetLastSuccessInfo(string batchJobID, DateTime lastSuccess, string lastJobMessage)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(batchJobID))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot set last success info; BatchJobID '" + batchJobID + "' not found.", LogType.Error);
				return false;
			}
			_batchJobStatus[batchJobID].LastSuccess = lastSuccess;
			_batchJobStatus[batchJobID].LastJobMessage = lastJobMessage;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool SetLastJobMessage(string batchJobID, string lastJobMessage)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(batchJobID))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot set last job message; BatchJobID '" + batchJobID + "' not found.", LogType.Error);
				return false;
			}
			_batchJobStatus[batchJobID].LastJobMessage = lastJobMessage;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool IncreaseNoofFailedStarts(string batchJobID)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(batchJobID))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot increase NoofFailedStarts; BatchJobID '" + batchJobID + "' not found.", LogType.Error);
				return false;
			}
			_batchJobStatus[batchJobID].NoofFailedStarts++;
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool DeleteJobStatus(string batchJobID)
	{
		if (string.IsNullOrEmpty(batchJobID))
		{
			ServiceContext.Current.AddErrAndLog(-200005, "Invalid batch job ID; null or empty reference not allowed in DeleteJobStatus().", LogType.Error);
			return false;
		}
		_dbLock.WaitOne(-1);
		try
		{
			if (!_batchJobStatus.ContainsKey(batchJobID))
			{
				ServiceContext.Current.AddErrAndLog(-206006, "Cannot delete BatchJobStatus; BatchJobID '" + batchJobID + "' not found.", LogType.Error);
				return false;
			}
			_batchJobStatus.Remove(batchJobID);
			return true;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}

	public bool JobStatusExist(string batchJobID)
	{
		_dbLock.WaitOne(-1);
		try
		{
			if (_batchJobStatus.ContainsKey(batchJobID))
			{
				return true;
			}
			return false;
		}
		finally
		{
			_dbLock.ReleaseMutex();
		}
	}
}
