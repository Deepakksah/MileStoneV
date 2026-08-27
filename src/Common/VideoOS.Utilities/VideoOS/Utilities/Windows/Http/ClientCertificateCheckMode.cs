namespace VideoOS.Utilities.Windows.Http;

public enum ClientCertificateCheckMode
{
	EnableRevocationCheck,
	NoRevocationCheck,
	OnlyCachedCertRevocation,
	UseDefaultRevocationFreshnessTime,
	NoUsageCheck
}
