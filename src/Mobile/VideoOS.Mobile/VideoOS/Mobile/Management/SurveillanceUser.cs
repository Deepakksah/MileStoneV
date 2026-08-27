using VideoOS.Mobile.Utilities;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Util.Security;

namespace VideoOS.Mobile.Management;

public class SurveillanceUser
{
	private const string BasicUser = "BasicUser";

	private const string WindowsUser = "WindowsUser";

	public string Name { get; private set; }

	public string Sid { get; private set; }

	public string SurveillanceType { get; private set; }

	public TSVUserType TSVUserType { get; set; }

	public SurveillanceUser(User user)
	{
		Name = ((user.IdentityType == IdentityType.BasicUser.ToString()) ? user.AccountName : (user.Domain + '\\' + user.AccountName));
		Sid = user.Sid;
		SurveillanceType = user.IdentityType;
	}

	public TSVUserType GetTsvUserType(string hostName)
	{
		if (SurveillanceType == "BasicUser" || SurveillanceType == "WindowsUser")
		{
			return TSVUserType.User;
		}
		string domain = UserDomainNameHelper.GetDomain(Name);
		return WindowsAccountHelper.IsPredefinedGroup(hostName, Sid) ? TSVUserType.WindowsGroup : (PrincipalWrapper.CheckWindowsGroupSid(hostName, Sid) ? TSVUserType.WindowsGroup : (PrincipalWrapper.CheckDomainGroupSid(domain, Sid) ? TSVUserType.DomainGroup : (WindowsAccountHelper.IsDomainName(hostName, domain) ? TSVUserType.DomainGroup : TSVUserType.Unknown)));
	}
}
