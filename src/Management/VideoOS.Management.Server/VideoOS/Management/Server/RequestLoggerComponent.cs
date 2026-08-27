using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.Data.SqlClient.Server;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RequestLoggerComponent : GeminiResourceComponent
{
	private static readonly SqlMetaData[] _metadataRequestLogTable = new SqlMetaData[7]
	{
		new SqlMetaData("TempId", SqlDbType.Int),
		new SqlMetaData("WebMethodName", SqlDbType.VarChar, 256L),
		new SqlMetaData("StartTime", SqlDbType.DateTime),
		new SqlMetaData("ExecutionTime", SqlDbType.BigInt),
		new SqlMetaData("IdentityName", SqlDbType.NVarChar, 256L),
		new SqlMetaData("IPAddress", SqlDbType.VarChar, 40L),
		new SqlMetaData("ExecutionCount", SqlDbType.Int)
	};

	private static readonly SqlMetaData[] _metadataRequestParameterTable = new SqlMetaData[4]
	{
		new SqlMetaData("TempId", SqlDbType.Int),
		new SqlMetaData("ParameterName", SqlDbType.VarChar, 64L),
		new SqlMetaData("ParameterIndex", SqlDbType.Int),
		new SqlMetaData("ParameterValue", SqlDbType.NVarChar, -1L)
	};

	public void SubmitRecords(IEnumerable<RequestDetails> requestList)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("submit_request_records");
			BindRequestLogTable("@request_records", requestList);
			BindRequestParameterTable("@request_parameters", requestList);
			ExecuteNonQuery();
		});
	}

	private DatabaseParameter BindRequestLogTable(string parameterName, IEnumerable<RequestDetails> details)
	{
		details = details ?? Enumerable.Empty<RequestDetails>();
		Func<RequestDetails, SqlDataRecord> selector = (RequestDetails requestDetails) => BindRequestLogRow(requestDetails, _metadataRequestLogTable);
		return BindTable(parameterName, "tvp_RequestLogTable", details.Select(selector).ToList(), convertEmptyToNull: true);
	}

	private static SqlDataRecord BindRequestLogRow(RequestDetails requestDetails, SqlMetaData[] type)
	{
		SqlDataRecord sqlDataRecord = new SqlDataRecord(type);
		sqlDataRecord.SetInt32(0, requestDetails.TempId);
		sqlDataRecord.SetString(1, MakeStringValue(requestDetails.WebMethodName, type[1].MaxLength));
		sqlDataRecord.SetDateTime(2, requestDetails.StartTime);
		sqlDataRecord.SetInt64(3, requestDetails.ExecutionTime);
		sqlDataRecord.SetString(4, MakeStringValue(requestDetails.IdentityName, type[4].MaxLength));
		sqlDataRecord.SetString(5, MakeStringValue(requestDetails.RemoteIPAddress, type[5].MaxLength));
		sqlDataRecord.SetInt32(6, requestDetails.ExecutionCount);
		return sqlDataRecord;
	}

	private DatabaseParameter BindRequestParameterTable(string parameterName, IEnumerable<RequestDetails> details)
	{
		details = details ?? Enumerable.Empty<RequestDetails>();
		Func<RequestDetails, IEnumerable<SqlDataRecord>> selector = (RequestDetails requestDetails) => BindRequestParameterRow(requestDetails, _metadataRequestParameterTable);
		return BindTable(parameterName, "tvp_RequestParameterTable", details.SelectMany(selector).ToList(), convertEmptyToNull: true);
	}

	private static IEnumerable<SqlDataRecord> BindRequestParameterRow(RequestDetails requestDetails, SqlMetaData[] type)
	{
		int parameterIndex = 0;
		if (requestDetails.Method != null)
		{
			string[] parameterNames = requestDetails.Method.Parameters.AllKeys;
			for (int i = 0; i < parameterNames.Length; i++)
			{
				parameterIndex = i;
				SqlDataRecord sqlDataRecord = new SqlDataRecord(type);
				sqlDataRecord.SetInt32(0, requestDetails.TempId);
				sqlDataRecord.SetString(1, MakeStringValue(parameterNames[i], 64L));
				sqlDataRecord.SetInt32(2, i);
				sqlDataRecord.SetString(3, requestDetails.Method.Parameters[parameterNames[i]]);
				yield return sqlDataRecord;
			}
		}
		if (requestDetails.RecursiveSqlCalls == null)
		{
			yield break;
		}
		foreach (MethodTime recursiveSqlCall in requestDetails.RecursiveSqlCalls)
		{
			parameterIndex++;
			string value = ((recursiveSqlCall.LogicalReads != 0) ? string.Format(CultureInfo.InvariantCulture, "{0} ms, {1} logical reads", (int)recursiveSqlCall.Time.TotalMilliseconds, recursiveSqlCall.LogicalReads) : string.Format(CultureInfo.InvariantCulture, "{0} ms", (int)recursiveSqlCall.Time.TotalMilliseconds));
			SqlDataRecord sqlDataRecord2 = new SqlDataRecord(type);
			sqlDataRecord2.SetInt32(0, requestDetails.TempId);
			sqlDataRecord2.SetString(1, recursiveSqlCall.Name);
			sqlDataRecord2.SetInt32(2, parameterIndex);
			sqlDataRecord2.SetString(3, value);
			yield return sqlDataRecord2;
		}
	}

	private static string MakeStringValue(string value, long maxLength)
	{
		if (value == null)
		{
			return string.Empty;
		}
		if (value.Length > (int)maxLength)
		{
			return value.Substring(0, (int)maxLength);
		}
		return value;
	}
}
