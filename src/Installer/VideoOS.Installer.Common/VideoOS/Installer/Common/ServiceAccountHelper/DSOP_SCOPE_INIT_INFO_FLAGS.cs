namespace VideoOS.Installer.Common.ServiceAccountHelper;

public class DSOP_SCOPE_INIT_INFO_FLAGS
{
	public const uint DSOP_SCOPE_FLAG_STARTING_SCOPE = 1u;

	public const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT = 2u;

	public const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP = 4u;

	public const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_GC = 8u;

	public const uint DSOP_SCOPE_FLAG_WANT_SID_PATH = 16u;

	public const uint DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH = 32u;

	public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS = 64u;

	public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS = 128u;

	public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS = 256u;

	public const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS = 512u;
}
