namespace VideoOS.Platform.Proxy.SecurityApi;

public class SecurityAction
{
	public string name { get; set; }

	public string displayName { get; set; }

	public bool visible { get; set; }

	public ulong securityMask { get; set; }
}
