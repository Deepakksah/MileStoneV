namespace VideoOS.Management.Server;

public class PerformanceCounterHelper
{
	public static string CurrentServerRequestsSec => "VideoOS.Management.Server.Performance_CurrentServerRequestsSec";

	public static string CurrentServerRequests => "VideoOS.Management.Server.Performance_CurrentServerRequests";

	public static string AverageResponseTime => "VideoOS.Management.Server.Performance_AverageResponseTime";

	public static string AverageResponseTimeBase => "VideoOS.Management.Server.Performance_AverageResponseTimeBase";

	public static string CurrentSqlCallsFromComponentSec => "VideoOS.Management.Server.Performance_CurrentSqlCallsFromComponentSec";

	public static string ActiveSqlConnections => "VideoOS.Management.Server.ActiveSqlConnections";

	public static string AverageSqlConnectTime => "VideoOS.Management.Server.AverageSqlConnectTime";

	public static string AverageSqlConnectTimeBase => "VideoOS.Management.Server.AverageSqlConnectTimeBase";

	public static string CurrentSqlConnectionFailuresSec => "VideoOS.Management.Server.CurrentSqlConnectionFailures";

	public static string CurrentSqlConnectionRetriesSec => "VideoOS.Management.Server.CurrentSqlConnectionRetries";

	public static string CurrentSqlExecutionsSec => "VideoOS.Management.Server.CurrentSqlExecutionsSec";

	public static string CurrentSqlExecutionRetriesSec => "VideoOS.Management.Server.CurrentSqlExecutionRetries";

	public static string TotalSqlBatches => "VideoOS.Management.Server.TotalSqlBatches";

	public static string TotalFailedRetrySequences => "VideoOS.Management.Server.TotalFailedRetrySequences";

	public static string MemoryCacheHitsSec => "VideoOS.Management.Server.MemoryCacheHitsSec";

	public static string MemoryCacheMissesSec => "VideoOS.Management.Server.MemoryCacheMissesSec";

	public static string MemoryCacheInvalidationsSec => "VideoOS.Management.Server.MemoryCacheInvalidationsSec";

	public static string MemoryCacheTotalHits => "VideoOS.Management.Server.MemoryCacheTotalHits";

	public static string MemoryCacheTotalMisses => "VideoOS.Management.Server.MemoryCacheTotalMisses";

	public static string MemoryCacheTotalInvalidations => "VideoOS.Management.Server.MemoryCacheTotalInvalidations";
}
