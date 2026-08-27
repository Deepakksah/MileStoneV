using System;
using System.Globalization;

namespace VideoOS.Server.DataComponents;

public sealed class SequencesComponent : VmoResourceComponent, ISequencesComponent, IDisposable
{
	public SequencesComponent()
	{
	}

	public long LatestIdpEventLogId()
	{
		string sql = "SELECT [Sequence] FROM [Sequences] WHERE [Name] = 'IdpEventLogIdSequence'";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			object value = ExecuteScalar();
			return Convert.ToInt64(value, NumberFormatInfo.InvariantInfo);
		});
	}

	public void UpdateIdpEventLogId(long newId)
	{
		string sql = "UPDATE [Sequences] SET [Sequence] = @latestId WHERE [Name] = 'IdpEventLogIdSequence'";
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindLong("@latestId", newId);
			ExecuteScalar();
		});
	}
}
