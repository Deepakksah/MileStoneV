namespace VideoOS.Management.Server;

public class AccessControlEntryDetails : AccessControlEntry2
{
	public string Token { get; set; }

	public AccessControlEntryDetails()
		: base(null, 0uL, 0uL)
	{
	}
}
