using System;

namespace VideoOS.Utilities.Security;

public sealed class NetValidatePasswordPolicy : INetValidatePasswordPolicy
{
	Win32Api.NET_API_STATUS INetValidatePasswordPolicy.NetValidatePasswordPolicy(string ServerName, IntPtr Qualifier, Win32Api.NET_VALIDATE_PASSWORD_TYPE ValidationType, IntPtr InputArg, ref IntPtr OutputArg)
	{
		return Win32Api.NetValidatePasswordPolicy(ServerName, Qualifier, ValidationType, InputArg, ref OutputArg);
	}
}
