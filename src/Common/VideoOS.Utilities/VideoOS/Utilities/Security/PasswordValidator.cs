using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Utilities.Security;

public class PasswordValidator : IPasswordValidator
{
	private readonly INetValidatePasswordPolicy _policyValidator;

	public PasswordValidator()
		: this(new NetValidatePasswordPolicy())
	{
	}

	public PasswordValidator(INetValidatePasswordPolicy policyValidator)
	{
		_policyValidator = policyValidator;
	}

	public PasswordValidationResult ValidatePassword(SecureString password)
	{
		if (password == null || password.Length == 0)
		{
			return PasswordValidationResult.PasswordIsEmpty;
		}
		return Password.LocalValidatePassword(null, password, _policyValidator) switch
		{
			Win32Api.NET_API_STATUS.NERR_Success => PasswordValidationResult.Ok, 
			Win32Api.NET_API_STATUS.NERR_PasswordTooShort => PasswordValidationResult.TooShort, 
			Win32Api.NET_API_STATUS.NERR_PasswordTooLong => PasswordValidationResult.TooLong, 
			Win32Api.NET_API_STATUS.NERR_PasswordNotComplexEnough => PasswordValidationResult.ComplexityPolicyNotMet, 
			Win32Api.NET_API_STATUS.NERR_PasswordFilterError => PasswordValidationResult.LocalPasswordFilterNotMet, 
			_ => PasswordValidationResult.OtherError, 
		};
	}

	public PasswordValidationResult ValidateNewPassword(SecureString passwordEntry1, SecureString passwordEntry2)
	{
		if (IsNullOrEmpty(passwordEntry1) && IsNullOrEmpty(passwordEntry2))
		{
			return PasswordValidationResult.PasswordIsEmpty;
		}
		if (IsNullOrEmpty(passwordEntry1) || IsNullOrEmpty(passwordEntry2))
		{
			return PasswordValidationResult.PasswordsDiffers;
		}
		if (!SecureStringEqual(passwordEntry1, passwordEntry2))
		{
			return PasswordValidationResult.PasswordsDiffers;
		}
		return ValidatePassword(passwordEntry1);
	}

	private static bool IsNullOrEmpty(SecureString secureString)
	{
		if (secureString != null)
		{
			return secureString.Length == 0;
		}
		return true;
	}

	private static bool SecureStringEqual(SecureString secureString1, SecureString secureString2)
	{
		if (secureString1 == null)
		{
			throw new ArgumentNullException("secureString1");
		}
		if (secureString2 == null)
		{
			throw new ArgumentNullException("secureString2");
		}
		if (secureString1.Length != secureString2.Length)
		{
			return false;
		}
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = IntPtr.Zero;
		try
		{
			intPtr = Marshal.SecureStringToBSTR(secureString1);
			intPtr2 = Marshal.SecureStringToBSTR(secureString2);
			string text = Marshal.PtrToStringBSTR(intPtr);
			string value = Marshal.PtrToStringBSTR(intPtr2);
			return text.Equals(value);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.ZeroFreeBSTR(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.ZeroFreeBSTR(intPtr2);
			}
		}
	}
}
