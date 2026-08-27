using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Login;

public class DualAuthType : ServerType
{
	private readonly List<AuthenticationInfo> authenticationList;

	public override Guid ServerTypeId => XPServerType.GetServerTypeId();

	public override AuthenticationType DefaultAuthenticationType => AuthenticationType.Windows;

	public override ReadOnlyCollection<AuthenticationInfo> AuthenticationTypes => new ReadOnlyCollection<AuthenticationInfo>(authenticationList);

	public DualAuthType(string name)
		: base(name)
	{
		authenticationList = GenerateAuthenticationList();
	}

	public static Guid GetServerTypeId()
	{
		return new Guid("5980E5AA-06C0-4E1D-9D3D-1F5561475017");
	}

	public static List<AuthenticationInfo> GenerateAuthenticationList()
	{
		return new List<AuthenticationInfo>
		{
			new AuthenticationWindows(Resources.UserLabelWindows, Resources.PasswordLabelWindows),
			new AuthenticationSimple(Resources.UserLabelSimple, Resources.PasswordLabelSimple)
		};
	}
}
