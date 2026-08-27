using System;
using System.Data;
using System.Text;
using VideoOS.Utilities.Database;

namespace VideoOS.Common.Integration;

public class SqlDataAccessor : SqlLogResourceComponentBase, ILogAuditLogger
{
	public SqlDataAccessor(ISqlConnectionManager sqlConnectionManager)
		: base(sqlConnectionManager, 0, 0)
	{
	}

	public void AddAuditLogEntry(LogEntryAudit logEntryAudit)
	{
		if (logEntryAudit == null)
		{
			throw new ArgumentNullException("logEntryAudit");
		}
		try
		{
			string[] array = new string[7];
			int num = Math.Min(7, logEntryAudit.Arguments.Length);
			for (int i = 0; i < num; i++)
			{
				array[i] = logEntryAudit.Arguments[i];
			}
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO [LogAuditVMOQueue] ( [TimeStampUTC], [TimeStamp], [LogID], [LogLevel], [AccessGranted], [AuditType], [User], [UserLocation], [ResourceType], [ResourceName], [ResourceLocation], [ResourceID], [Argument0], [Argument1], [Argument2], [Argument3], [Argument4], [Argument5], [Argument6]) ");
			stringBuilder.Append("VALUES (@TimeStampUTC, @TimeStamp, @LogID, @LogLevel, @AccessGranted, @AuditType, @User, @UserLocation, @ResourceType, @ResourceName, @ResourceLocation, @ResourceID, @Arg0, @Arg1, @Arg2, @Arg3, @Arg4, @Arg5, @Arg6) ");
			PrepareSqlBatch();
			AddStatement(stringBuilder.ToString());
			BindDateTime("@TimeStampUTC", logEntryAudit.TimeStampUtc);
			BindDateTime("@TimeStamp", logEntryAudit.TimeStampLocal);
			BindLogIDType("@LogID", logEntryAudit.LogId);
			BindEnumType("@LogLevel", typeof(LogLevel), LogLevel.Info);
			BindShortNameType("@AccessGranted", logEntryAudit.AccessGrantedValue);
			BindShortNameType("@AuditType", logEntryAudit.AuditTypeValue);
			BindUserIdType("@User", logEntryAudit.UserCredentials);
			BindUriType("@UserLocation", logEntryAudit.Location);
			BindShortNameType("@ResourceType", logEntryAudit.ResourceTypeValue);
			BindNameType("@ResourceName", logEntryAudit.ResourceName);
			BindNameType("@ResourceLocation", logEntryAudit.ResourceLocation);
			BindGuidType("@ResourceID", logEntryAudit.ResourceID.ToString());
			BindString("@Arg0", array[0], 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Arg1", array[1], 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Arg2", array[2], 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Arg3", array[3], 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Arg4", array[4], 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Arg5", array[5], 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Arg6", array[6], 512, allowNull: true, SqlDbType.NVarChar);
			ExecuteNonQuery();
		}
		catch (DatabaseException)
		{
			throw;
		}
	}
}
