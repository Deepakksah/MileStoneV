using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Login;

public class UnknownServerType : ServerType
{
	private readonly List<AuthenticationInfo> authenticationList;

	public override Guid ServerTypeId => Guid.Empty;

	public override AuthenticationType DefaultAuthenticationType => AuthenticationType.Unknown;

	public override ReadOnlyCollection<AuthenticationInfo> AuthenticationTypes => new ReadOnlyCollection<AuthenticationInfo>(authenticationList);

	public UnknownServerType()
		: base(string.Empty)
	{
		authenticationList = new List<AuthenticationInfo>();
		authenticationList.Add(new AuthenticationWindowsDefault(Resources.UserLabelWindows, Resources.PasswordLabelWindows));
		authenticationList.Add(new AuthenticationWindows(Resources.UserLabelWindows, Resources.PasswordLabelWindows));
		authenticationList.Add(new AuthenticationSimple(Resources.UserLabelSimple, Resources.PasswordLabelSimple));
		authenticationList.Add(new AuthenticationExternal(Resources.UserLabelSimple, Resources.PasswordLabelSimple));
	}
}
