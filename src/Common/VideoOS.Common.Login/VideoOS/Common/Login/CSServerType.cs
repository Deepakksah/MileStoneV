using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Login;

public class CSServerType : ServerType
{
	private readonly List<AuthenticationInfo> authenticationList;

	public override Guid ServerTypeId => GetServerTypeId();

	public override AuthenticationType DefaultAuthenticationType => AuthenticationType.WindowsDefault;

	public override ReadOnlyCollection<AuthenticationInfo> AuthenticationTypes => new ReadOnlyCollection<AuthenticationInfo>(authenticationList);

	public CSServerType(string name)
		: base(name)
	{
		authenticationList = new List<AuthenticationInfo>();
		authenticationList.Add(new AuthenticationWindowsDefault(Resources.UserLabelWindows, Resources.PasswordLabelWindows));
		authenticationList.Add(new AuthenticationWindows(Resources.UserLabelWindows, Resources.PasswordLabelWindows));
	}

	public static Guid GetServerTypeId()
	{
		return new Guid("3D4D610D-2A1B-4D1E-BF09-006F3EA50E0B");
	}
}
