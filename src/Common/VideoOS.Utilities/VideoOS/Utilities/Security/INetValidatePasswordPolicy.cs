using System;

namespace VideoOS.Utilities.Security;

public interface INetValidatePasswordPolicy
{
	Win32Api.NET_API_STATUS NetValidatePasswordPolicy(string serverName, IntPtr qualifier, Win32Api.NET_VALIDATE_PASSWORD_TYPE validationType, IntPtr inputArg, ref IntPtr outputArg);
}
