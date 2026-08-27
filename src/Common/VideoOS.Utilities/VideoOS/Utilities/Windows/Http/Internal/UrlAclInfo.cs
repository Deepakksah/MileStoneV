namespace VideoOS.Utilities.Windows.Http.Internal;

public class UrlAclInfo
{
	public string UrlPrefix { get; }

	public string Permissions { get; }

	public UrlAclInfo(string urlPrefix, string permissionsSddl)
	{
		UrlPrefix = urlPrefix;
		Permissions = permissionsSddl;
	}
}
