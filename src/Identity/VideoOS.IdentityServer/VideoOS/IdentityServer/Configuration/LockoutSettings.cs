namespace VideoOS.IdentityServer.Configuration;

public class LockoutSettings
{
	public bool EnableLockoutNewUsers { get; set; } = true;

	public int LockoutTimeSpanInMinutes { get; set; } = 5;

	public int MaxFailedAccessAttempts { get; set; } = 5;
}
