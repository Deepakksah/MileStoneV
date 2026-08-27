using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Utilities.Security;

public static class Password
{
	public static bool ValidatePassword(string userName, SecureString password)
	{
		return LocalValidatePassword(userName, password, new NetValidatePasswordPolicy()) == Win32Api.NET_API_STATUS.NERR_Success;
	}

	public static Win32Api.NET_API_STATUS LocalValidatePassword(string userName, SecureString password, INetValidatePasswordPolicy policyValidator)
	{
		Win32Api.NET_VALIDATE_PASSWORD_CHANGE_INPUT_ARG structure = new Win32Api.NET_VALIDATE_PASSWORD_CHANGE_INPUT_ARG
		{
			PasswordMatched = true,
			UserAccountName = userName
		};
		Win32Api.NET_VALIDATE_OUTPUT_ARG nET_VALIDATE_OUTPUT_ARG = default(Win32Api.NET_VALIDATE_OUTPUT_ARG);
		IntPtr intPtr = IntPtr.Zero;
		IntPtr outputArg = IntPtr.Zero;
		try
		{
			structure.ClearPassword = Marshal.SecureStringToBSTR(password);
			intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, intPtr, fDeleteOld: false);
			Win32Api.NET_API_STATUS nET_API_STATUS = policyValidator.NetValidatePasswordPolicy(null, IntPtr.Zero, Win32Api.NET_VALIDATE_PASSWORD_TYPE.NetValidatePasswordChange, intPtr, ref outputArg);
			if (nET_API_STATUS != Win32Api.NET_API_STATUS.NERR_Success)
			{
				throw new InvalidOperationException(nET_API_STATUS.ToString());
			}
			return ((Win32Api.NET_VALIDATE_OUTPUT_ARG)Marshal.PtrToStructure(outputArg, typeof(Win32Api.NET_VALIDATE_OUTPUT_ARG))).ValidationStatus;
		}
		finally
		{
			if (outputArg != IntPtr.Zero)
			{
				Win32Api.NetValidatePasswordPolicyFree(ref outputArg);
			}
			if (structure.ClearPassword != IntPtr.Zero)
			{
				Marshal.ZeroFreeBSTR(structure.ClearPassword);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}
}
