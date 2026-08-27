using System;
using System.Globalization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class IdentityCleanupService : PeriodicTimerServiceBase
{
	private const int minExpiryDay = 1;

	private const int maxExpiryDay = int.MaxValue;

	private const int defaultExpiryDay = 60;

	protected override TimeSpan InitialTimeInterval => TimeSpan.FromSeconds(10.0);

	protected override TimeSpan PeriodicTimeInterval => TimeSpan.FromDays(1.0);

	private static int GetExpiryDay()
	{
		if (int.TryParse(Config.Instance.GetAppSetting("IdentityCleanupService_ExpirationDays", 60.ToString(CultureInfo.InvariantCulture)), out var result) && result >= 1 && result <= int.MaxValue)
		{
			return result;
		}
		return 60;
	}

	protected override void OnTimer(object state)
	{
		GeminiTrace.Info(TraceType.Security, "Triggering scheduled CleanupDeletedIdentities for Windows users");
		using (GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext())
		{
			CleanupDeletedIdentities(requestContext, GetExpiryDay());
		}
		GeminiTrace.Info(TraceType.Security, "Finished scheduled CleanupDeletedIdentities for Windows users");
	}

	private static void CleanupDeletedIdentities(IGeminiRequestContext requestContext, int expiryDay)
	{
		using IIdentityCacheComponent identityCacheComponent = requestContext.CreateIdentityCacheComponent();
		try
		{
			identityCacheComponent.CleanupDeletedIdentities(expiryDay);
		}
		catch (DatabaseException ex)
		{
			GeminiTrace.Warning(TraceType.Security, "A SQL error occurred while cleaning up deleted identities.\r\nException:\r\n{0}", ex.ToString());
		}
		catch (DatabaseConfigUnknownErrorException ex2)
		{
			GeminiTrace.Warning(TraceType.Security, "A SQL error occurred while cleaning up deleted identities.\r\nException:\r\n{0}", ex2.ToString());
		}
	}
}
