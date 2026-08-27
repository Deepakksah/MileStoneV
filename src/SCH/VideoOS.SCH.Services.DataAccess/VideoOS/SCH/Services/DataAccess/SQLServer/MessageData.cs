using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.Utilities.Database;

namespace VideoOS.SCH.Services.DataAccess.SQLServer;

public class MessageData : IMessageDataMgr
{
	public int CreateMessage(string messageTypeName, Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> messageData, List<Guid> interestedSessions)
	{
		int num = 0;
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "insert into Messages (MsgTypeName, Created) values (@MsgTypeName, @Created) SET @MsgID = SCOPE_IDENTITY()";
			int num2;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
				databaseParameter.Value = messageTypeName;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@Created", SqlDbType.DateTime);
				databaseParameter.Value = DateTime.UtcNow;
				databaseCommand.AddParameter(databaseParameter);
				DatabaseParameter databaseParameter2 = new DatabaseParameter("@MsgID", SqlDbType.Int);
				databaseParameter2.Direction = ParameterDirection.Output;
				databaseCommand.AddParameter(databaseParameter2);
				databaseCommand.ExecuteNonQuery();
				num2 = (int)databaseParameter2.Value;
			}
			commandText = "insert into MessageData (MsgID, MsgDataKey, MsgDataValue) values (@MsgID, @MsgDataKey, @MsgDataValue)";
			foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageData value in messageData.Values)
			{
				using DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText);
				DatabaseParameter databaseParameter = new DatabaseParameter("@MsgID", SqlDbType.Int);
				databaseParameter.Value = num2;
				databaseCommand2.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgDataKey", SqlDbType.Char, 20);
				databaseParameter.Value = value.Key;
				databaseCommand2.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgDataValue", SqlDbType.NVarChar, 50);
				databaseParameter.Value = value.Value;
				databaseCommand2.AddParameter(databaseParameter);
				databaseCommand2.ExecuteNonQuery();
			}
			commandText = "insert into SessionMsgs (SessionID, MsgID) values (@SessionID, @MsgID)";
			foreach (Guid interestedSession in interestedSessions)
			{
				using DatabaseCommand databaseCommand3 = databaseConnection.CreateCommand(commandText);
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = interestedSession;
				databaseCommand3.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgID", SqlDbType.Int);
				databaseParameter.Value = num2;
				databaseCommand3.AddParameter(databaseParameter);
				try
				{
					databaseCommand3.ExecuteNonQuery();
					num++;
				}
				catch (DatabaseException ex)
				{
					ServiceContext.Current.AddErrAndLog(-206009, "Referential integrity error: DatabaseException occurred during SessionMsgs insert.", ex, LogType.Error);
				}
			}
			databaseTransaction.Commit();
			return num;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public Dictionary<int, Message> GetAndDeleteSessionMessages(Guid sessionID, int returnMaxSize, out bool moreMsgsToRet)
	{
		Dictionary<int, Message> dictionary = new Dictionary<int, Message>();
		Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> data = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData>();
		Message message = null;
		int num = 0;
		moreMsgsToRet = false;
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "SELECT m.MsgID, RTRIM(m.MsgTypeName) AS MsgTypeName, m.Created, RTRIM(md.MsgDataKey) AS MsgDataKey, md.MsgDataValue ";
			text += "FROM SessionMsgs AS sm INNER JOIN ";
			text += "Messages AS m ON sm.MsgID = m.MsgID LEFT OUTER JOIN ";
			text += "MessageData AS md ON m.MsgID = md.MsgID ";
			text += "WHERE (sm.SessionID = @SessionID)";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				databaseCommand.AddParameter(new DatabaseParameter("@SessionID", sessionID));
				using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
				while (databaseDataReader.Read())
				{
					if (message == null || message.ID != databaseDataReader.GetInt32(0))
					{
						if (message != null)
						{
							if (num > returnMaxSize)
							{
								moreMsgsToRet = true;
								break;
							}
							dictionary.Add(message.ID, message);
							data = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData>();
						}
						num += 42;
						num += databaseDataReader.GetString(1).Length;
						message = new Message(databaseDataReader.GetInt32(0), databaseDataReader.GetString(1), databaseDataReader.GetDateTime(2), data);
					}
					if (!databaseDataReader.IsDBNull(3))
					{
						num += databaseDataReader.GetString(3).Length;
						num += databaseDataReader.GetString(4).Length;
						message.Data.Add(databaseDataReader.GetString(3), new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(databaseDataReader.GetString(3), databaseDataReader.GetString(4)));
					}
				}
				if ((message != null) & !moreMsgsToRet)
				{
					dictionary.Add(message.ID, message);
				}
			}
			if (dictionary.Count > 0)
			{
				string text2 = null;
				foreach (Message value in dictionary.Values)
				{
					text2 = text2 + value.ID + ",";
				}
				text2 = text2.Substring(0, text2.Length - 1);
				text = "delete from SessionMsgs where SessionID = @SessionID and MsgID in (" + text2 + ")";
				using DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(text);
				databaseCommand2.AddParameter(new DatabaseParameter("@SessionID", sessionID));
				databaseCommand2.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			return dictionary;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public Dictionary<int, CreatedSessionMessageItem> GetSessionMessagesLaterThan(DateTime createTime)
	{
		throw new NotImplementedException();
	}

	public bool DeleteNonSessionMessages()
	{
		List<string> list = new List<string>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "SELECT Messages.MsgID ";
			text += "FROM SessionMsgs RIGHT OUTER JOIN ";
			text += "Messages ON SessionMsgs.MsgID = Messages.MsgID ";
			text += "WHERE (SessionMsgs.SessionID IS NULL)";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
				while (databaseDataReader.Read())
				{
					list.Add(databaseDataReader.GetInt32(0).ToString());
				}
			}
			if (list.Count > 0)
			{
				string text2 = string.Join(",", list.ToArray());
				text = "delete from MessageData where MsgID in (" + text2 + ")";
				using (DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(text))
				{
					databaseCommand2.ExecuteNonQuery();
				}
				text = "delete from Messages where MsgID in (" + text2 + ")";
				using DatabaseCommand databaseCommand3 = databaseConnection.CreateCommand(text);
				databaseCommand3.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			return true;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public int GetLastMessageID()
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "select IDENT_CURRENT('Messages')";
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		return Convert.ToInt32(databaseCommand.ExecuteScalar());
	}

	public List<Message> GetMessages(int returnMaxSize, out bool moreMsgsToRet)
	{
		List<Message> list = new List<Message>();
		Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData> data = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData>();
		Message message = null;
		int num = 0;
		moreMsgsToRet = false;
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string text = "SELECT m.MsgID, RTRIM(m.MsgTypeName) AS MsgTypeName, m.Created, RTRIM(md.MsgDataKey) AS MsgDataKey, md.MsgDataValue ";
		text += "FROM Messages AS m LEFT OUTER JOIN ";
		text += "MessageData AS md ON m.MsgID = md.MsgID ";
		text += "ORDER BY m.MsgID ";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
		{
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				if (message == null || message.ID != databaseDataReader.GetInt32(0))
				{
					if (message != null)
					{
						if (num > returnMaxSize)
						{
							moreMsgsToRet = true;
							break;
						}
						list.Add(message);
						data = new Dictionary<string, VideoOS.SCH.Services.Business.Definitions.Custom.MessageData>();
					}
					num += 42;
					num += databaseDataReader.GetString(1).Length;
					message = new Message(databaseDataReader.GetInt32(0), databaseDataReader.GetString(1), databaseDataReader.GetDateTime(2), data);
				}
				if (!databaseDataReader.IsDBNull(3))
				{
					num += databaseDataReader.GetString(3).Length;
					num += databaseDataReader.GetString(4).Length;
					message.Data.Add(databaseDataReader.GetString(3), new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(databaseDataReader.GetString(3), databaseDataReader.GetString(4)));
				}
			}
			if ((message != null) & !moreMsgsToRet)
			{
				list.Add(message);
			}
		}
		return list;
	}

	public bool GetTotalSessionMsgInfo(out int totalMsgCount, out DateTime latestMsgCreateTime)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string text = "SELECT COUNT(SessionMsgs.MsgID) AS MsgCount, MAX(Messages.Created) AS LatestMsgTime";
		text += " FROM SessionMsgs INNER JOIN Messages ON SessionMsgs.MsgID = Messages.MsgID";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
		{
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			if (databaseDataReader.Read())
			{
				totalMsgCount = databaseDataReader.GetInt32(0);
				if (databaseDataReader.IsDBNull(1))
				{
					latestMsgCreateTime = DateTime.MinValue;
				}
				else
				{
					latestMsgCreateTime = databaseDataReader.GetDateTime(1);
				}
				return true;
			}
		}
		ServiceContext.Current.AddErrAndLog(-206010, "Select COUNT() and MAX() from session messages did not return any row.", LogType.Error);
		totalMsgCount = 0;
		latestMsgCreateTime = DateTime.MinValue;
		return false;
	}

	public List<SessionMsgInfo> GetSessionMsgInfoList()
	{
		List<SessionMsgInfo> list = new List<SessionMsgInfo>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string text = "SELECT SessionMsgs.SessionID, COUNT(SessionMsgs.MsgID) AS MsgCount, MAX(Messages.Created) AS LatestMsgTime";
		text += " FROM SessionMsgs INNER JOIN Messages ON SessionMsgs.MsgID = Messages.MsgID";
		text += " GROUP BY SessionMsgs.SessionID";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
		{
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new SessionMsgInfo(databaseDataReader.GetGuid(0), databaseDataReader.GetInt32(1), databaseDataReader.GetDateTime(2)));
			}
		}
		return list;
	}
}
