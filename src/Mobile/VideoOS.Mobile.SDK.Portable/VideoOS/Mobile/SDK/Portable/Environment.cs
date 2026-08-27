using System.Net;
using VideoOS.Mobile.Portable.Upnp;
using VideoOS.Mobile.SDK.Portable.OnlineService;
using VideoOS.Mobile.SDK.Portable.Server.Communication;

namespace VideoOS.Mobile.SDK.Portable;

public class Environment
{
	private const int MaxConnectionLimit = 100;

	public static Environment Instance = new Environment();

	private Environment()
	{
	}

	public void Initialize()
	{
		InitializeConnectionLimit(100);
	}

	public void InitializeConnectionLimit(int maxConnectionLimit)
	{
		ServicePointManager.DefaultConnectionLimit = maxConnectionLimit;
	}

	public void InitializeCommunicationAlias(string baseAlias)
	{
		Aliases.InitBaseAlias(baseAlias);
	}

	public void InitializeAbsDefaultLinks(string appLink, string storeLink)
	{
		InvitationLinksDefaultValues.Instance.AppLink = appLink;
		InvitationLinksDefaultValues.Instance.StoreLink = storeLink;
	}

	public void InitializeMobileSeverUpnpServiceDescription(string serverName, string companyName)
	{
		MobileServiceDescription.MobileServerNameUpnpServiceDescription = serverName;
		MobileServiceDescription.CompanyShortNameUpnpServiceDescription = companyName;
	}
}
