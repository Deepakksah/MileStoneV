using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.DataAccess;
using VideoOS.SCH.Services.DataAccess.Generic;

namespace VideoOS.SCH.Services.Business.Components;

public class Message
{
	private bool ValidateMessageData(VideoOS.SCH.Services.Business.Definitions.Custom.Message message)
	{
		if (string.IsNullOrEmpty(message.MessageTypeName))
		{
			ServiceContext.Current.AddErrAndLog(-205000, "Invalid Message fields; null or empty MessageTypeName not allowed.", LogType.Error);
			return false;
		}
		if (message.MessageTypeName.Length > 30)
		{
			ServiceContext.Current.AddErrAndLog(-205004, "Invalid Message fields; Message type name too long, cannot exceed " + 30 + " characters.", LogType.Error);
			return false;
		}
		foreach (MessageData value in message.Data.Values)
		{
			if (string.IsNullOrEmpty(value.Key))
			{
				ServiceContext.Current.AddErrAndLog(-205005, "Invalid MessageData fields; null or empty key not allowed.", LogType.Error);
				return false;
			}
			if (value.Key.Length > 25)
			{
				ServiceContext.Current.AddErrAndLog(-205006, "Invalid MessageData fields; Data key length too long, cannot exceed " + 25 + " characters.", LogType.Error);
				return false;
			}
			if (string.IsNullOrEmpty(value.Value))
			{
				ServiceContext.Current.AddErrAndLog(-205001, "Invalid MessageData fields; null or empty Value not allowed.", LogType.Error);
				return false;
			}
			if (value.Value.Length > 50)
			{
				ServiceContext.Current.AddErrAndLog(-205003, "Invalid MessageData fields; Data value length too long, cannot exceed " + 50 + " characters.", LogType.Error);
				return false;
			}
		}
		return true;
	}

	private static void RemoveTrailingString(string stringToRemove, ref string stringToTrim)
	{
		if (stringToTrim != null && stringToTrim.EndsWith(stringToRemove))
		{
			stringToTrim = stringToTrim.TrimEnd(stringToRemove.ToCharArray());
		}
	}

	private List<string> GetMessageTraceItems(VideoOS.SCH.Services.Business.Definitions.Custom.Message message, string eventType, int noofConnectedSessions, string clientSession)
	{
		string stringToTrim = null;
		if (message.Data == null)
		{
			stringToTrim = "null";
		}
		else
		{
			foreach (MessageData value in message.Data.Values)
			{
				stringToTrim = stringToTrim + value.Key + "=" + value.Value + "; ";
			}
			RemoveTrailingString("; ", ref stringToTrim);
		}
		return new List<string>
		{
			"MessageTypeName=" + message.MessageTypeName,
			"MessageID=" + message.ID,
			"EventType=" + eventType,
			"NoofSessions=" + noofConnectedSessions,
			"clientSession=" + clientSession,
			"Created=" + message.Created.ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo),
			"Data={" + stringToTrim + "}"
		};
	}

	public int CreateMessages(VideoOS.SCH.Services.Business.Definitions.Custom.Message[] messages)
	{
		if (messages == null)
		{
			ServiceContext.Current.AddErrAndLog(-200004, "Argument Message array cannot be null in CreateMessages()", LogType.Error);
			return 0;
		}
		int num = 0;
		IMessageDataMgr messageDataMgr = GenericDataMgr.GetMessageDataMgr();
		MessageWatch messageWatch = new MessageWatch();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Message message in messages)
		{
			if (ValidateMessageData(message))
			{
				int noofCreatedMsgs = messageDataMgr.CreateMessage(message.MessageTypeName, message.Data, messageWatch.GetInterestedSessionIDs(message.MessageTypeName, message.Data));
				num += noofCreatedMsgs;
				ServiceContext.Current.LogToMessageLog(() => GetMessageTraceItems(message, "Created", noofCreatedMsgs, null));
			}
		}
		return num;
	}

	private Dictionary<int, VideoOS.SCH.Services.Business.Definitions.Custom.Message> CheckForMessagesPrivate(Guid sessionID)
	{
		Dictionary<int, VideoOS.SCH.Services.Business.Definitions.Custom.Message> dictionary = null;
		bool moreMsgsToRet = false;
		if (ConfigSettings.SessionMsgIndicatorUsed && !SessionMsgIndicator.Current.HasSessionMsgs(sessionID))
		{
			return null;
		}
		dictionary = GenericDataMgr.GetMessageDataMgr().GetAndDeleteSessionMessages(sessionID, ConfigSettings.MaxResponseDataSize, out moreMsgsToRet);
		if (dictionary != null && dictionary.Count == 0 && moreMsgsToRet)
		{
			ServiceContext.Current.AddErrAndLog(-205002, "Message size in one message is too big. 'MaxResponseDataSize' = " + ConfigSettings.MaxResponseDataSize + ".", LogType.Error);
		}
		if (dictionary != null)
		{
			foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Message message in dictionary.Values)
			{
				ServiceContext.Current.LogToMessageLog(() => GetMessageTraceItems(message, "Read", 1, sessionID.ToString()));
			}
		}
		return dictionary;
	}

	public int GetMillisecsToWaitForMessages(int millisecsRequest)
	{
		return Math.Min(millisecsRequest, ConfigSettings.MaxNoofMillisecsToWaitForMessages);
	}

	public VideoOS.SCH.Services.Business.Definitions.Custom.Message[] CheckForMessages(Guid sessionID, int millisecsToWaitForMessages)
	{
		Session.SetSessionExpireTime(sessionID);
		foreach (ErrorItem error in ServiceContext.Current.Errors)
		{
			if (error.ErrorNumber == -201000)
			{
				return null;
			}
		}
		Dictionary<int, VideoOS.SCH.Services.Business.Definitions.Custom.Message> dictionary = CheckForMessagesPrivate(sessionID);
		if (dictionary != null && dictionary.Count > 0)
		{
			return dictionary.Values.ToArray();
		}
		if (GetMillisecsToWaitForMessages(millisecsToWaitForMessages) > 0)
		{
			if (false)
			{
				dictionary = CheckForMessagesPrivate(sessionID);
				if (dictionary != null && dictionary.Count > 0)
				{
					return dictionary.Values.ToArray();
				}
			}
			return null;
		}
		return null;
	}

	public bool DeleteNonSessionMessages()
	{
		return GenericDataMgr.GetMessageDataMgr().DeleteNonSessionMessages();
	}
}
