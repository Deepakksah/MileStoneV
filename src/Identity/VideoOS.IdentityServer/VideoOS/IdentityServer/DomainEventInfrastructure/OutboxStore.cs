using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Data;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxStore(ApplicationDbContext dbContext, IOptions<OutboxProcessorOptions> options) : IOutboxStore
{
	private static readonly string CompatibilityLevelSql = string.Join(" OR ", from int l in Enum.GetValues<EventLevel>()
		select $"o.CompatibilityLevel = {l}");

	public async Task<OutboxMessageResult> ClaimMessages(string instanceId, CancellationToken cancellationToken)
	{
		SqlParameter[] parameters = CreateClaimParameters(instanceId);
		List<OutboxMessage> messages = await GetClaimedMessages(parameters, cancellationToken);
		SqlParameter[] parameters2 = CreateMessageResultParameters(messages, instanceId);
		return await CreateMessageResult(messages, parameters2, cancellationToken);
	}

	private SqlParameter[] CreateMessageResultParameters(List<OutboxMessage> messages, string instanceId)
	{
		SqlParameter[] array = CreateCommonParameters(instanceId);
		SqlParameter[] array2 = array;
		int num = 0;
		SqlParameter[] array3 = new SqlParameter[1 + array2.Length];
		ReadOnlySpan<SqlParameter> readOnlySpan = new ReadOnlySpan<SqlParameter>(array2);
		readOnlySpan.CopyTo(new Span<SqlParameter>(array3).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		array3[num] = new SqlParameter("@MessageIds", SqlDbType.NVarChar, -1)
		{
			Value = (messages.Any() ? ((IConvertible)string.Join(",", messages.Select((OutboxMessage m) => m.Id.ToString()))) : ((IConvertible)DBNull.Value))
		};
		num++;
		return array3;
	}

	private SqlParameter[] CreateCommonParameters(string instanceId)
	{
		int pollingIntervalSeconds = options.Value.PollingIntervalSeconds;
		int num = pollingIntervalSeconds * options.Value.MaxRetryDelayInPollingIntervals;
		return new SqlParameter[6]
		{
			new SqlParameter("@CreatedStatus", SqlDbType.Int)
			{
				Value = 0
			},
			new SqlParameter("@RetryRequiredStatus", SqlDbType.Int)
			{
				Value = 4
			},
			new SqlParameter("@ProcessingStatus", SqlDbType.Int)
			{
				Value = 1
			},
			new SqlParameter("@InstanceId", SqlDbType.NVarChar, 36)
			{
				Value = instanceId
			},
			new SqlParameter("@PollingIntervalSeconds", SqlDbType.Int)
			{
				Value = pollingIntervalSeconds
			},
			new SqlParameter("@MaxRetryDelay", SqlDbType.Int)
			{
				Value = num
			}
		};
	}

	private SqlParameter[] CreateClaimParameters(string instanceId)
	{
		SqlParameter[] array = CreateCommonParameters(instanceId);
		SqlParameter[] array2 = array;
		int num = 0;
		SqlParameter[] array3 = new SqlParameter[2 + array2.Length];
		ReadOnlySpan<SqlParameter> readOnlySpan = new ReadOnlySpan<SqlParameter>(array2);
		readOnlySpan.CopyTo(new Span<SqlParameter>(array3).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		array3[num] = new SqlParameter("@BatchSize", SqlDbType.Int)
		{
			Value = options.Value.BatchSize
		};
		num++;
		array3[num] = new SqlParameter("@LockExpirationSeconds", SqlDbType.Int)
		{
			Value = options.Value.LockExpirationSeconds
		};
		num++;
		return array3;
	}

	private async Task<List<OutboxMessage>> GetClaimedMessages(SqlParameter[] parameters, CancellationToken cancellationToken)
	{
		string sql = "\r\n            WITH NextBatch AS (\r\n                SELECT TOP(@BatchSize) *\r\n                FROM OutboxMessages o WITH (ROWLOCK, READPAST, UPDLOCK)\r\n                WHERE (o.Status = @CreatedStatus OR o.Status = @RetryRequiredStatus OR o.Status = @ProcessingStatus)\r\n                    AND (\r\n                        o.ProcessingInstanceId IS NULL \r\n                        OR o.LockExpiresAtUtc < SYSUTCDATETIME()\r\n                        OR (o.Status = @ProcessingStatus AND o.ProcessingInstanceId = @InstanceId)\r\n                    )\r\n                    AND (" + CompatibilityLevelSql + ")\r\n                    AND (\r\n                        o.Status = @CreatedStatus\r\n                        OR (\r\n                            o.Status = @RetryRequiredStatus AND\r\n                            (o.LastAttemptUtc IS NULL OR\r\n                             o.LastAttemptUtc < DATEADD(SECOND, \r\n                                -1 * CASE \r\n                                    WHEN @PollingIntervalSeconds * POWER(2, RetryCount) > @MaxRetryDelay\r\n                                    THEN @MaxRetryDelay\r\n                                    ELSE @PollingIntervalSeconds * POWER(2, RetryCount)\r\n                                END,\r\n                                SYSUTCDATETIME()\r\n                            ))\r\n                        )\r\n                    OR o.Status = @ProcessingStatus\r\n                    )\r\n                ORDER BY o.CreatedAtUtc\r\n            )\r\n            UPDATE m WITH (ROWLOCK, UPDLOCK)\r\n            SET \r\n                Status = @ProcessingStatus,\r\n                ProcessingInstanceId = @InstanceId,\r\n                LockExpiresAtUtc = DATEADD(SECOND, @LockExpirationSeconds, SYSUTCDATETIME()),\r\n                LastAttemptUtc = SYSUTCDATETIME()\r\n            OUTPUT \r\n                INSERTED.Id,\r\n                INSERTED.EventType,\r\n                INSERTED.Payload,\r\n                INSERTED.CompatibilityLevel,\r\n                INSERTED.Status,\r\n                INSERTED.CreatedAtUtc,\r\n                INSERTED.ProcessedAtUtc,\r\n                INSERTED.ProcessingInstanceId,\r\n                INSERTED.LockExpiresAtUtc,\r\n                INSERTED.LastAttemptUtc,\r\n                INSERTED.RetryCount,\r\n                INSERTED.Error\r\n            FROM OutboxMessages m\r\n            INNER JOIN NextBatch n ON m.Id = n.Id\r\n            OPTION (OPTIMIZE FOR UNKNOWN)";
		return (await dbContext.Set<OutboxMessage>().FromSqlRaw(sql, parameters.Cast<object>().ToArray()).AsNoTracking()
			.ToListAsync(cancellationToken)).OrderBy((OutboxMessage m) => m.CreatedAtUtc).ToList();
	}

	private async Task<OutboxMessageResult> CreateMessageResult(List<OutboxMessage> messages, SqlParameter[] parameters, CancellationToken cancellationToken)
	{
		bool isFullBatch = messages.Count == options.Value.BatchSize;
		bool flag = false;
		if (!isFullBatch)
		{
			string sql = "\r\n                SELECT 1 as Id\r\n                WHERE EXISTS (\r\n                    SELECT 1 \r\n                    FROM OutboxMessages o WITH (READPAST, READCOMMITTEDLOCK)\r\n                    WHERE (@MessageIds IS NULL OR o.Id NOT IN (\r\n                        SELECT value FROM STRING_SPLIT(@MessageIds, ',')\r\n                    ))\r\n                    AND (o.Status = @CreatedStatus \r\n                         OR o.Status = @RetryRequiredStatus \r\n                         OR o.Status = @ProcessingStatus)\r\n                    AND (\r\n                        o.ProcessingInstanceId IS NULL \r\n                        OR o.LockExpiresAtUtc < SYSUTCDATETIME()\r\n                        OR (o.Status = @ProcessingStatus \r\n                            AND o.ProcessingInstanceId = @InstanceId)\r\n                    )\r\n                    AND (" + CompatibilityLevelSql + ")\r\n                    AND (\r\n                        o.Status = @CreatedStatus\r\n                        OR (\r\n                            o.Status = @RetryRequiredStatus AND\r\n                            (o.LastAttemptUtc IS NULL OR\r\n                             o.LastAttemptUtc < DATEADD(SECOND, \r\n                                -1 * CASE \r\n                                    WHEN @PollingIntervalSeconds * POWER(2, RetryCount) > @MaxRetryDelay\r\n                                    THEN @MaxRetryDelay\r\n                                    ELSE @PollingIntervalSeconds * POWER(2, RetryCount) \r\n                                END,\r\n                                SYSUTCDATETIME()\r\n                            ))\r\n                        )\r\n                        OR o.Status = @ProcessingStatus\r\n                    )\r\n                )";
			flag = await dbContext.Set<OutboxMessage>().FromSqlRaw(sql, parameters.Cast<object>().ToArray()).AnyAsync(cancellationToken);
		}
		return new OutboxMessageResult
		{
			Messages = messages,
			IsFullBatch = isFullBatch,
			HasPendingMessages = (flag || isFullBatch)
		};
	}

	public async Task UpdateMessages(List<MessageUpdate> updates, CancellationToken cancellationToken)
	{
		if (updates.Count == 0)
		{
			return;
		}
		if (updates.All((MessageUpdate u) => u.Status == OutboxMessageStatus.Delivered))
		{
			await UpdateDeliveredMessageBatch(updates.Select((MessageUpdate u) => u.Id).ToList(), cancellationToken);
		}
		else
		{
			await UpdateMixedMessageBatch(updates, cancellationToken);
		}
	}

	private async Task UpdateDeliveredMessageBatch(List<Guid> messageIds, CancellationToken cancellationToken)
	{
		DateTime now = DateTime.UtcNow;
		await (from m in dbContext.Set<OutboxMessage>()
			where messageIds.Contains(m.Id)
			select m).ExecuteUpdateAsync((SetPropertyCalls<OutboxMessage> s) => s.SetProperty((OutboxMessage m) => m.Status, OutboxMessageStatus.Delivered).SetProperty((OutboxMessage m) => m.ProcessedAtUtc, (OutboxMessage _) => now), cancellationToken);
	}

	private async Task UpdateMixedMessageBatch(List<MessageUpdate> updates, CancellationToken cancellationToken)
	{
		List<object> list = new List<object>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < updates.Count; i++)
		{
			list2.Add($"({{{3 * i}}}, {{{3 * i + 1}}}, {{{3 * i + 2}}})");
			list.Add(updates[i].Id);
			list.Add((int)updates[i].Status);
			list.Add(updates[i].Error);
		}
		string sql = "UPDATE OutboxMessages\r\nSET Status = v.status,\r\n    ProcessedAtUtc = CASE \r\n        WHEN v.status IN (2, 3) THEN SYSUTCDATETIME()\r\n        ELSE NULL \r\n    END,\r\n    Error = v.error,\r\n    RetryCount = CASE \r\n        WHEN v.status = 4 AND RetryCount < 10000 THEN RetryCount + 1\r\n        ELSE RetryCount \r\n    END,\r\n    ProcessingInstanceId = CASE \r\n        WHEN v.status = 4 THEN NULL\r\n        ELSE ProcessingInstanceId \r\n    END,\r\n    LockExpiresAtUtc = CASE \r\n        WHEN v.status = 4 THEN NULL\r\n        ELSE LockExpiresAtUtc \r\n    END\r\nFROM (VALUES\r\n    " + string.Join(",", list2) + "\r\n) AS v(id, status, error)\r\nWHERE OutboxMessages.Id = v.id";
		await dbContext.Database.ExecuteSqlRawAsync(sql, list.ToArray(), cancellationToken);
	}

	public async Task<int> DeleteLogsOlderThanAsync(DateTimeOffset dateTime)
	{
		DbSet<OutboxMessage> source = dbContext.Set<OutboxMessage>();
		return await source.Where((OutboxMessage m) => (DateTimeOffset?)m.ProcessedAtUtc < dateTime && (int)m.Status == 2).ExecuteDeleteAsync();
	}
}
