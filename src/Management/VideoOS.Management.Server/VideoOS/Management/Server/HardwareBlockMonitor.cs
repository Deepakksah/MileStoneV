using System;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class HardwareBlockMonitor : PeriodicTimerServiceBase
{
	protected override TimeSpan InitialTimeInterval => TimeSpan.FromSeconds(new Random().Next(60, 90));

	protected override TimeSpan PeriodicTimeInterval => TimeSpan.FromMinutes(1.0);

	protected override void OnTimer(object state)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			Hardware.CleanupExpiredHardwareBlocks(requestContext);
		}
		catch (DatabaseException ex)
		{
			GeminiTrace.Warning(TraceType.General, "A SQL error occurred during cleanup of hardware blocks.\r\nException:\r\n{0}", ex.ToString());
		}
	}
}
