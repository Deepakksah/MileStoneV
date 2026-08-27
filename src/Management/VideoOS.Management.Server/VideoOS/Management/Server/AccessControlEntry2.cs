namespace VideoOS.Management.Server;

public class AccessControlEntry2
{
	public IdentityDescriptor Descriptor { get; set; }

	public ulong Allow { get; set; }

	public ulong Deny { get; set; }

	public AccessControlEntry2(IdentityDescriptor descriptor, ulong allow, ulong deny)
	{
		Descriptor = descriptor;
		Allow = allow;
		Deny = deny;
	}
}
