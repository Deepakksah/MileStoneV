namespace VideoOS.Utilities.Windows.Http.Internal;

public enum ClientCertCheckMode
{
	Enabled = 0,
	NoVerifyRevocation = 1,
	CachedRevocationOnly = 2,
	UseRevocationFreshnessTime = 4,
	NoUsageCheck = 0x10000
}
