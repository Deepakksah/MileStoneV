using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.Utilities.Database;

namespace VideoOS.SCH.Services.DataAccess.SQLServer;

public class SessionData : ISessionDataMgr
{
	public bool CreateSession(string clientName, string clientUserName, DateTime expiryTime, out Guid sessionID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "insert into Sessions (SessionID, ClientName, ClientUserName, ExpiryTime) values (@SessionID, @ClientName, @ClientUserName, @ExpiryTime)";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				sessionID = Guid.NewGuid();
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = sessionID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@ClientName", SqlDbType.VarChar, 50);
				databaseParameter.Value = clientName;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@ClientUserName", SqlDbType.VarChar, 50);
				if (clientUserName == null)
				{
					databaseParameter.Value = DBNull.Value;
				}
				else
				{
					databaseParameter.Value = clientUserName;
				}
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@ExpiryTime", SqlDbType.DateTime);
				databaseParameter.Value = expiryTime;
				databaseCommand.AddParameter(databaseParameter);
				databaseCommand.ExecuteNonQuery();
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

	public Session GetSession(Guid sessionID)
	{
		List<int> list = new List<int>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "SELECT MsgID FROM SessionMsgs where SessionID = @SessionID";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
		{
			databaseCommand.AddParameter(new DatabaseParameter("@SessionID", sessionID));
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(databaseDataReader.GetInt32(0));
			}
		}
		string clientUserName = null;
		commandText = "select SessionID, ClientName, ClientUserName, ExpiryTime from Sessions where SessionID = @SessionID";
		using DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText);
		databaseCommand2.AddParameter(new DatabaseParameter("@SessionID", sessionID));
		using DatabaseDataReader databaseDataReader2 = databaseCommand2.ExecuteReader();
		if (databaseDataReader2.Read())
		{
			if (!databaseDataReader2.IsDBNull(2))
			{
				clientUserName = databaseDataReader2.GetString(2);
			}
			return new Session(databaseDataReader2.GetGuid(0), list, databaseDataReader2.GetString(1), clientUserName, databaseDataReader2.GetDateTime(3));
		}
		return null;
	}

	public bool DeleteSession(Guid sessionID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "delete from MatchExpressions where SessionID = @SessionID";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				databaseCommand.AddParameter(new DatabaseParameter("@SessionID", sessionID));
				databaseCommand.ExecuteNonQuery();
			}
			commandText = "delete from MessageWatchers where SessionID = @SessionID";
			using (DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText))
			{
				databaseCommand2.AddParameter(new DatabaseParameter("@SessionID", sessionID));
				databaseCommand2.ExecuteNonQuery();
			}
			commandText = "delete from SessionMsgs where SessionID = @SessionID";
			using (DatabaseCommand databaseCommand3 = databaseConnection.CreateCommand(commandText))
			{
				databaseCommand3.AddParameter(new DatabaseParameter("@SessionID", sessionID));
				databaseCommand3.ExecuteNonQuery();
			}
			commandText = "delete from Sessions where SessionID = @SessionID";
			using (DatabaseCommand databaseCommand4 = databaseConnection.CreateCommand(commandText))
			{
				databaseCommand4.AddParameter(new DatabaseParameter("@SessionID", sessionID));
				databaseCommand4.ExecuteNonQuery();
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

	public bool SessionExists(Guid sessionID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "select COUNT(*) from Sessions where SessionID = @SessionID";
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		databaseCommand.AddParameter(new DatabaseParameter("@SessionID", sessionID));
		if ((int)databaseCommand.ExecuteScalar() == 1)
		{
			return true;
		}
		return false;
	}

	public List<Guid> GetSessionIDsWithExpiryTimeLessThan(DateTime expireTime)
	{
		List<Guid> list = new List<Guid>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "SELECT SessionID FROM Sessions where ExpiryTime < @ExpiryTime";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
		{
			databaseCommand.AddParameter(new DatabaseParameter("@ExpiryTime", expireTime));
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(databaseDataReader.GetGuid(0));
			}
		}
		return list;
	}

	public Dictionary<Guid, Session> GetAllSessions()
	{
		Dictionary<Guid, Session> dictionary = new Dictionary<Guid, Session>();
		List<int> messageIDs = new List<int>();
		Session session = null;
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string text = "SELECT s.SessionID, s.ClientName, s.ClientUserName, s.ExpiryTime, sm.MsgID FROM Sessions AS s ";
		text += "LEFT OUTER JOIN SessionMsgs AS sm ON s.SessionID = sm.SessionID ";
		text += "ORDER BY s.SessionID";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
		{
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				if (session == null || session.ID != databaseDataReader.GetGuid(0))
				{
					if (session != null)
					{
						dictionary.Add(session.ID, session);
						messageIDs = new List<int>();
					}
					session = new Session(clientUserName: (!databaseDataReader.IsDBNull(2)) ? databaseDataReader.GetString(2) : null, sessionID: databaseDataReader.GetGuid(0), messageIDs: messageIDs, clientName: databaseDataReader.GetString(1), expiryTime: databaseDataReader.GetDateTime(3));
				}
				if (!databaseDataReader.IsDBNull(4))
				{
					session.MessageIDs.Add(databaseDataReader.GetInt32(4));
				}
			}
			if (session != null)
			{
				dictionary.Add(session.ID, session);
			}
		}
		return dictionary;
	}

	public void SetSessionExpireTime(IEnumerable<Guid> sessionIDs, DateTime expiryTime)
	{
		if (sessionIDs == null)
		{
			ServiceContext.Current.AddErrAndLog(-200004, "Invalid session ID array; null reference not allowed in SetSessionExpireTime().", LogType.Error);
		}
		else
		{
			if (sessionIDs.Count() <= 0)
			{
				return;
			}
			string text = null;
			foreach (Guid sessionID in sessionIDs)
			{
				text = text + "'" + sessionID.ToString() + "',";
			}
			text = text.Substring(0, text.Length - 1);
			using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
			using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				string commandText = "update Sessions set ExpiryTime = @ExpiryTime where SessionID in (" + text + ")";
				using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
				{
					databaseCommand.AddParameter(new DatabaseParameter("@ExpiryTime", expiryTime));
					databaseCommand.ExecuteNonQuery();
				}
				databaseTransaction.Commit();
			}
			catch (DatabaseException)
			{
				databaseTransaction.Rollback();
				throw;
			}
		}
	}

	public List<ServiceChannelClient> GetAllClients()
	{
		List<ServiceChannelClient> list = new List<ServiceChannelClient>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "select ClientName from Sessions";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
		{
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new ServiceChannelClient(databaseDataReader.GetString(0), null));
			}
		}
		return list;
	}
}
