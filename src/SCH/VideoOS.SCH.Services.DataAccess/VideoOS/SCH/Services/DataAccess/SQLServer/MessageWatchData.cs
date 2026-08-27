using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.Utilities.Database;

namespace VideoOS.SCH.Services.DataAccess.SQLServer;

public class MessageWatchData : IMessageWatchDataMgr
{
	public void CreateMessageWatch(Guid sessionID, string messageTypeName)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			InsertMessageWatcher(databaseConnection, sessionID, messageTypeName);
			databaseTransaction.Commit();
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	private static MessageWatch GetMessageWatcher(DatabaseConnection conn, Guid sessionID, string msgTypeName)
	{
		MessageWatch messageWatch = null;
		string text = "SELECT mw.SessionID, RTRIM(mw.MsgTypeName) AS MsgTypeName, RTRIM(me.MsgDataKey) AS MsgDataKey, me.MsgDataValue, RTRIM(me.MatchOprName) as MatchOprName ";
		text += "FROM MessageWatchers AS mw LEFT OUTER JOIN ";
		text += "MatchExpressions AS me ON mw.SessionID = me.SessionID AND mw.MsgTypeName = me.MsgTypeName ";
		text += "WHERE (mw.SessionID = @SessionID) AND (mw.MsgTypeName = @MsgTypeName)";
		using DatabaseCommand databaseCommand = conn.CreateCommand(text);
		DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
		databaseParameter.Value = sessionID;
		databaseCommand.AddParameter(databaseParameter);
		databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
		databaseParameter.Value = msgTypeName;
		databaseCommand.AddParameter(databaseParameter);
		using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
		while (databaseDataReader.Read())
		{
			if (messageWatch == null)
			{
				messageWatch = new MessageWatch(databaseDataReader.GetGuid(0), databaseDataReader.GetString(1));
			}
			if (!databaseDataReader.IsDBNull(2))
			{
				messageWatch.Expressions.Add(new MatchExpression(new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(databaseDataReader.GetString(2), databaseDataReader.GetString(3)), databaseDataReader.GetString(4)));
			}
		}
		return messageWatch;
	}

	private static void InsertMessageWatcher(DatabaseConnection conn, Guid sessionID, string msgTypeName)
	{
		string text = "insert into MessageWatchers (SessionID, MsgTypeName) ";
		text += "values (@SessionID, @MsgTypeName)";
		using DatabaseCommand databaseCommand = conn.CreateCommand(text);
		DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
		databaseParameter.Value = sessionID;
		databaseCommand.AddParameter(databaseParameter);
		databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
		databaseParameter.Value = msgTypeName;
		databaseCommand.AddParameter(databaseParameter);
		databaseCommand.ExecuteNonQuery();
	}

	private static int InsertMatchExpressions(DatabaseConnection conn, Guid sessionID, string msgTypeName, List<MatchExpression> matchExpressions)
	{
		int num = 0;
		foreach (MatchExpression matchExpression in matchExpressions)
		{
			string text = "insert into MatchExpressions (SessionID, MsgTypeName, MsgDataKey, MsgDataValue, MatchOprName) ";
			text += "values (@SessionID, @MsgTypeName, @MsgDataKey, @MsgDataValue, @MatchOprName)";
			using DatabaseCommand databaseCommand = conn.CreateCommand(text);
			DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
			databaseParameter.Value = sessionID;
			databaseCommand.AddParameter(databaseParameter);
			databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
			databaseParameter.Value = msgTypeName;
			databaseCommand.AddParameter(databaseParameter);
			databaseParameter = new DatabaseParameter("@MsgDataKey", SqlDbType.Char, 20);
			databaseParameter.Value = matchExpression.Data.Key;
			databaseCommand.AddParameter(databaseParameter);
			databaseParameter = new DatabaseParameter("@MsgDataValue", SqlDbType.NVarChar, 50);
			databaseParameter.Value = matchExpression.Data.Value;
			databaseCommand.AddParameter(databaseParameter);
			databaseParameter = new DatabaseParameter("@MatchOprName", SqlDbType.Char, 10);
			databaseParameter.Value = matchExpression.OperatorName;
			databaseCommand.AddParameter(databaseParameter);
			databaseCommand.ExecuteNonQuery();
			num++;
		}
		return num;
	}

	private bool EqualExpressions(MatchExpression expr1, MatchExpression expr2)
	{
		if (expr1.Data.Key == expr2.Data.Key && expr1.OperatorName == expr2.OperatorName && expr1.Data.Value == expr2.Data.Value)
		{
			return true;
		}
		return false;
	}

	private bool ExpressionExistInList(MatchExpression expr1, List<MatchExpression> expressions)
	{
		if (expressions != null)
		{
			foreach (MatchExpression expression in expressions)
			{
				if (EqualExpressions(expr1, expression))
				{
					return true;
				}
			}
		}
		return false;
	}

	public int CreateMatchExpressions(MessageWatch messageWatch)
	{
		int num = 0;
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			MessageWatch messageWatcher = GetMessageWatcher(databaseConnection, messageWatch.SessionID, messageWatch.MessageTypeName);
			if (messageWatcher == null)
			{
				InsertMessageWatcher(databaseConnection, messageWatch.SessionID, messageWatch.MessageTypeName);
			}
			List<MatchExpression> list = new List<MatchExpression>();
			foreach (MatchExpression expression in messageWatch.Expressions)
			{
				if (messageWatcher != null && ExpressionExistInList(expression, messageWatcher.Expressions))
				{
					ServiceContext.Current.AddErrAndLog(-206005, "Invalid MatchExpression; duplicate expressions are not allowed when creating a MatchExpression.", LogType.Error);
				}
				else
				{
					list.Add(expression);
				}
			}
			num = InsertMatchExpressions(databaseConnection, messageWatch.SessionID, messageWatch.MessageTypeName, list);
			databaseTransaction.Commit();
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
		return num;
	}

	public bool DeleteMessageWatchers(Guid sessionID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "delete from MatchExpressions where SessionID = @SessionID";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = sessionID;
				databaseCommand.AddParameter(databaseParameter);
				databaseCommand.ExecuteNonQuery();
			}
			commandText = "delete from MessageWatchers where SessionID = @SessionID";
			using (DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = sessionID;
				databaseCommand2.AddParameter(databaseParameter);
				databaseCommand2.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
		return true;
	}

	public bool DeleteMessageWatch(Guid sessionID, string messageTypeName)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "delete from MatchExpressions where SessionID = @SessionID and MsgTypeName = @MsgTypeName";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = sessionID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
				databaseParameter.Value = messageTypeName;
				databaseCommand.AddParameter(databaseParameter);
				databaseCommand.ExecuteNonQuery();
			}
			commandText = "delete from MessageWatchers where SessionID = @SessionID and MsgTypeName = @MsgTypeName";
			using (DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = sessionID;
				databaseCommand2.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
				databaseParameter.Value = messageTypeName;
				databaseCommand2.AddParameter(databaseParameter);
				databaseCommand2.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
		return true;
	}

	public void DeleteMatchExpressions(MessageWatch messageWatch)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "delete from MatchExpressions where SessionID = @SessionID and MsgTypeName = @MsgTypeName and MsgDataKey = @MsgDataKey and MsgDataValue = @MsgDataValue";
			foreach (MatchExpression expression in messageWatch.Expressions)
			{
				using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
				DatabaseParameter databaseParameter = new DatabaseParameter("@SessionID", SqlDbType.UniqueIdentifier);
				databaseParameter.Value = messageWatch.SessionID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
				databaseParameter.Value = messageWatch.MessageTypeName;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgDataKey", SqlDbType.Char, 20);
				databaseParameter.Value = expression.Data.Key;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@MsgDataValue", SqlDbType.NVarChar, 50);
				databaseParameter.Value = expression.Data.Value;
				databaseCommand.AddParameter(databaseParameter);
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

	public bool DeleteMessageWatchOrphans()
	{
		return true;
	}

	public Dictionary<string, MatchOperator> GetAllMatchOperators(bool includeDescriptions)
	{
		Dictionary<string, MatchOperator> dictionary = new Dictionary<string, MatchOperator>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "select RTRIM(MatchOprName) AS MatchOprName, MatchOprDescr from MatchOperators";
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
		{
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				dictionary.Add(value: (!includeDescriptions || databaseDataReader.IsDBNull(1)) ? new MatchOperator(databaseDataReader.GetString(0), null) : new MatchOperator(databaseDataReader.GetString(0), databaseDataReader.GetString(1)), key: databaseDataReader.GetString(0));
			}
		}
		return dictionary;
	}

	private List<MessageWatch> GetMessageWatchersPrivate(string messageTypeName)
	{
		List<MessageWatch> list = new List<MessageWatch>();
		List<MatchExpression> expressions = new List<MatchExpression>();
		MessageWatch messageWatch = null;
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string text = "SELECT mw.SessionID, RTRIM(mw.MsgTypeName) AS MsgTypeName, RTRIM(me.MsgDataKey) AS MsgDataKey, me.MsgDataValue, RTRIM(me.MatchOprName) AS MatchOprName ";
		text += "FROM MessageWatchers AS mw LEFT OUTER JOIN ";
		text += "MatchExpressions AS me ON mw.SessionID = me.SessionID AND mw.MsgTypeName = me.MsgTypeName ";
		if (messageTypeName != null)
		{
			text += "WHERE mw.MsgTypeName = @MsgTypeName";
		}
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
		{
			if (messageTypeName != null)
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@MsgTypeName", SqlDbType.Char, 30);
				databaseParameter.Value = messageTypeName;
				databaseCommand.AddParameter(databaseParameter);
			}
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				if (messageWatch == null || messageWatch.SessionID != databaseDataReader.GetGuid(0) || messageWatch.MessageTypeName != databaseDataReader.GetString(1))
				{
					if (messageWatch != null)
					{
						list.Add(messageWatch);
						expressions = new List<MatchExpression>();
					}
					messageWatch = new MessageWatch(databaseDataReader.GetGuid(0), databaseDataReader.GetString(1), expressions);
				}
				if (!databaseDataReader.IsDBNull(2))
				{
					messageWatch.Expressions.Add(new MatchExpression(new VideoOS.SCH.Services.Business.Definitions.Custom.MessageData(databaseDataReader.GetString(2), databaseDataReader.GetString(3)), databaseDataReader.GetString(4)));
				}
			}
			if (messageWatch != null)
			{
				list.Add(messageWatch);
			}
		}
		return list;
	}

	public List<MessageWatch> GetMessageWatchers()
	{
		return GetMessageWatchersPrivate(null);
	}

	public List<MessageWatch> GetMessageWatchers(string messageTypeName)
	{
		return GetMessageWatchersPrivate(messageTypeName);
	}
}
