using System.Linq;
using VideoOS.Platform.Proxy.BookmarkApi;

namespace VideoOS.Platform.Proxy.SecurityApi;

public class SecurityNamespaceModel
{
	public string name { get; set; }

	public string displayName { get; set; }

	public string id { get; set; }

	public string resourceType { get; set; }

	public bool userDefined { get; set; }

	public int structure { get; set; }

	public SecurityAction[] securityActions { get; set; }

	public RelationsDataModel relations { get; set; }

	public ulong LookupPermissionMask(string actionName)
	{
		return securityActions.FirstOrDefault((SecurityAction a) => a.name == actionName)?.securityMask ?? 0;
	}
}
