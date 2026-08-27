using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.Utilities.Database;

namespace VideoOS.SCH.Services.DataAccess.SQLServer;

public class BatchJobData : IBatchJobDataMgr
{
	private const int MaxJobMessageLength = 1000;

	private string TruncateJobMessage(string jobMessage)
	{
		if (jobMessage != null && jobMessage.Length > 1000)
		{
			jobMessage = jobMessage.Substring(0, 1000);
		}
		return jobMessage;
	}

	public bool CreateJobStatus(BatchJobStatus batchJobStatus)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "insert into BatchJobStatus (BatchJobID, [Status], NoofFailedStarts, LastSuccess, LastJobMessage) values (@BatchJobID, @Status, @NoofFailedStarts, @LastSuccess, @LastJobMessage)";
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobStatus.BatchJobID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@Status", SqlDbType.SmallInt);
				databaseParameter.Value = batchJobStatus.Status;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@NoofFailedStarts", SqlDbType.Int);
				databaseParameter.Value = batchJobStatus.NoofFailedStarts;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastSuccess", SqlDbType.DateTime);
				databaseParameter.Value = batchJobStatus.LastSuccess;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastJobMessage", SqlDbType.VarChar, 1000);
				if (batchJobStatus.LastJobMessage == null)
				{
					databaseParameter.Value = DBNull.Value;
				}
				else
				{
					databaseParameter.Value = TruncateJobMessage(batchJobStatus.LastJobMessage);
				}
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

	public List<BatchJobStatus> GetBatchJobStatus(string batchJobID)
	{
		List<BatchJobStatus> list = new List<BatchJobStatus>();
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string text = "select RTRIM(BatchJobID) AS BatchJobID, Status, NoofFailedStarts, LastSuccess, LastJobMessage from BatchJobStatus ";
		if (!string.IsNullOrEmpty(batchJobID))
		{
			text += "where BatchJobID = @BatchJobID";
		}
		using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
		{
			if (!string.IsNullOrEmpty(batchJobID))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobID;
				databaseCommand.AddParameter(databaseParameter);
			}
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new BatchJobStatus(lastJobMessage: (!databaseDataReader.IsDBNull(4)) ? databaseDataReader.GetString(4) : null, batchJobID: databaseDataReader.GetString(0), status: databaseDataReader.GetInt16(1), noofFailedStarts: databaseDataReader.GetInt32(2), lastSuccess: databaseDataReader.GetDateTime(3)));
			}
		}
		return list;
	}

	public bool UpdateJobStatus(BatchJobStatus batchJobStatus)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "update BatchJobStatus set Status = @Status, NoofFailedStarts = @NoofFailedStarts, LastSuccess = @LastSuccess, LastJobMessage = @LastJobMessage ";
			text += "where BatchJobID = @BatchJobID";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobStatus.BatchJobID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@Status", SqlDbType.SmallInt);
				databaseParameter.Value = batchJobStatus.Status;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@NoofFailedStarts", SqlDbType.Int);
				databaseParameter.Value = batchJobStatus.NoofFailedStarts;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastSuccess", SqlDbType.DateTime);
				databaseParameter.Value = batchJobStatus.LastSuccess;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastJobMessage", SqlDbType.VarChar, 1000);
				if (batchJobStatus.LastJobMessage == null)
				{
					databaseParameter.Value = DBNull.Value;
				}
				else
				{
					databaseParameter.Value = TruncateJobMessage(batchJobStatus.LastJobMessage);
				}
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusOfUpdatedRecord, int statusToUpdate)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "update BatchJobStatus set Status = @NewStatus ";
			text += "where BatchJobID = @BatchJobID and Status = @Status";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = jobIDOfUpdatedRecord;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@Status", SqlDbType.SmallInt);
				databaseParameter.Value = statusOfUpdatedRecord;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@NewStatus", SqlDbType.SmallInt);
				databaseParameter.Value = statusToUpdate;
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusToUpdate)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "update BatchJobStatus set Status = @Status ";
			text += "where BatchJobID = @BatchJobID";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = jobIDOfUpdatedRecord;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@Status", SqlDbType.SmallInt);
				databaseParameter.Value = statusToUpdate;
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool SetLastSuccessInfo(string batchJobID, DateTime lastSuccess, string lastJobMessage)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "update BatchJobStatus set LastSuccess = @LastSuccess, LastJobMessage = @LastJobMessage ";
			text += "where BatchJobID = @BatchJobID";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastSuccess", SqlDbType.DateTime);
				databaseParameter.Value = lastSuccess;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastJobMessage", SqlDbType.VarChar, 1000);
				if (lastJobMessage == null)
				{
					databaseParameter.Value = DBNull.Value;
				}
				else
				{
					databaseParameter.Value = TruncateJobMessage(lastJobMessage);
				}
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool SetLastJobMessage(string batchJobID, string lastJobMessage)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "update BatchJobStatus set LastJobMessage = @LastJobMessage ";
			text += "where BatchJobID = @BatchJobID";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobID;
				databaseCommand.AddParameter(databaseParameter);
				databaseParameter = new DatabaseParameter("@LastJobMessage", SqlDbType.VarChar, 1000);
				if (lastJobMessage == null)
				{
					databaseParameter.Value = DBNull.Value;
				}
				else
				{
					databaseParameter.Value = TruncateJobMessage(lastJobMessage);
				}
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool IncreaseNoofFailedStarts(string batchJobID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string text = "update BatchJobStatus set NoofFailedStarts = NoofFailedStarts + 1 ";
			text += "where BatchJobID = @BatchJobID";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobID;
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool DeleteJobStatus(string batchJobID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		using DatabaseTransaction databaseTransaction = databaseConnection.BeginTransaction(IsolationLevel.ReadCommitted);
		try
		{
			string commandText = "delete from BatchJobStatus where BatchJobID = @BatchJobID";
			int num;
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
				databaseParameter.Value = batchJobID;
				databaseCommand.AddParameter(databaseParameter);
				num = databaseCommand.ExecuteNonQuery();
			}
			databaseTransaction.Commit();
			if (num == 1)
			{
				return true;
			}
			return false;
		}
		catch (DatabaseException)
		{
			databaseTransaction.Rollback();
			throw;
		}
	}

	public bool JobStatusExist(string batchJobID)
	{
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConfigSettings.SQLServerConnectionString, ServiceContext.Current.ServerLogger?.Logger);
		string commandText = "select COUNT(*) from BatchJobStatus where BatchJobID = @BatchJobID";
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		DatabaseParameter databaseParameter = new DatabaseParameter("@BatchJobID", SqlDbType.Char, 50);
		databaseParameter.Value = batchJobID;
		databaseCommand.AddParameter(databaseParameter);
		if ((int)databaseCommand.ExecuteScalar() == 1)
		{
			return true;
		}
		return false;
	}
}
