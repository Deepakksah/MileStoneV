using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Login;

public class XPServerType : ServerType
{
	private readonly List<AuthenticationInfo> authenticationList;

	public override Guid ServerTypeId => GetServerTypeId();

	public override AuthenticationType DefaultAuthenticationType => AuthenticationType.Simple;

	public override ReadOnlyCollection<AuthenticationInfo> AuthenticationTypes => new ReadOnlyCollection<AuthenticationInfo>(authenticationList);

	public XPServerType(string name)
		: base(name)
	{
		authenticationList = new List<AuthenticationInfo>();
		authenticationList.Add(new AuthenticationWindowsDefault(Resources.UserLabelWindows, Resources.PasswordLabelWindows));
		authenticationList.Add(new AuthenticationSimple(Resources.UserLabelSimple, Resources.PasswordLabelSimple));
	}

	public static Guid GetServerTypeId()
	{
		return new Guid("5D049AA5-ADB5-4A1C-8027-5B01734F5277");
	}
}
