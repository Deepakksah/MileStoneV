using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.XPath;
using VideoOS.Common.Integration.Exceptions;
using VideoOS.Utilities.Database;

namespace VideoOS.Common.Integration;

public class SqlLogPendingAccessor : SqlLogResourceComponentBase
{
	public static class PendingLogColumns
	{
		private static DatabaseColumnBinder LogID = new DatabaseColumnBinder("log_id");

		private static DatabaseColumnBinder Parameters = new DatabaseColumnBinder("params");

		public static PendingLog Bind(DatabaseDataReader reader)
		{
			using StringReader textReader = new StringReader(Parameters.GetString(reader, allowNulls: false));
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
			string xpath = "/params/param";
			XPathNodeIterator xPathNodeIterator = xPathNavigator.Select(xpath);
			List<string> list = new List<string>();
			foreach (XPathNavigator item in xPathNodeIterator)
			{
				list.Add(item.Value);
			}
			return new PendingLog
			{
				LogId = (LogEntryId)LogID.GetInt32(reader),
				Parameters = list.ToArray()
			};
		}
	}

	public class PendingLog
	{
		public LogEntryId LogId { get; set; }

		public string[] Parameters { get; set; }
	}

	public SqlLogPendingAccessor(ISqlConnectionManager sqlConnectionManager)
		: base(sqlConnectionManager, 0, 0)
	{
	}

	public List<PendingLog> QueryPendingLogs(Guid sessionId)
	{
		PrepareSqlBatch();
		AddStatement(string.Format(CultureInfo.InvariantCulture, "SELECT [log_id], [params] FROM [LogPending] WHERE session_id = '{0}'", sessionId.ToString()));
		DatabaseDataReader databaseDataReader = ExecuteReader();
		List<PendingLog> list = new List<PendingLog>();
		while (databaseDataReader.Read())
		{
			list.Add(PendingLogColumns.Bind(databaseDataReader));
		}
		if (databaseDataReader.NextResult())
		{
			throw new DatabaseConfigExtraRowsException();
		}
		return list;
	}

	public void DeletePendingLogs(Guid sessionId)
	{
		PrepareSqlBatch();
		AddStatement(string.Format(CultureInfo.InvariantCulture, "DELETE FROM [LogPending] WHERE session_id = '{0}'", sessionId.ToString()));
		ExecuteNonQuery();
	}
}
