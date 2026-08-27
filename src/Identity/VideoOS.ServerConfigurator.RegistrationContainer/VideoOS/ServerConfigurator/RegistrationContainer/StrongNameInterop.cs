using System;
using System.Runtime.InteropServices;
using System.Security;
using CLRMetaHost;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public static class StrongNameInterop
{
	[SecurityCritical]
	[ThreadStatic]
	private static ICLRStrongName s_StrongName;

	public static ICLRStrongName StrongName
	{
		[SecurityCritical]
		get
		{
			if (s_StrongName == null)
			{
				s_StrongName = (ICLRStrongName)RuntimeEnvironment.GetRuntimeInterfaceAsObject(new Guid("B79B0ACD-F5CD-409b-B5A5-A16244610B92"), new Guid("9FD93CCF-3280-4391-B3A9-96E1CDE77C8D"));
			}
			return s_StrongName;
		}
	}
}
