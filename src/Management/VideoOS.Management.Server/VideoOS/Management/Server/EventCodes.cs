namespace VideoOS.Management.Server;

public class EventCodes
{
	private const int _offset = 100000;

	public const int AuthenticationFailure = 100001;

	public const int AuthenticationSuccess = 100002;

	public const int CacheHit = 100003;

	public const int CacheMiss = 100004;

	public const int CacheAdd = 100005;

	public const int CacheAddError = 100006;
}
