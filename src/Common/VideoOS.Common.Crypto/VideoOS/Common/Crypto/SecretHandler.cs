using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Crypto;

[CLSCompliant(true)]
public class SecretHandler : ISecretHandlerDisposable, ISecretHandler, IDisposable
{
	private readonly ISecretProvider _provider;

	private SecureString _secret = new SecureString();

	private bool disposedValue;

	public SecretHandler(ISecretProvider provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("provider");
		}
		_provider = provider;
	}

	public void WriteSecret(byte[] secret)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		byte[] secret2 = _provider.EncryptSecret(secret, DataProtectionScope.LocalMachine);
		_provider.WriteTempSecret(secret2);
	}

	public void WriteSecret(byte[] secret, FileSystemAccessRule rule)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		byte[] secret2 = _provider.EncryptSecret(secret, DataProtectionScope.LocalMachine);
		_provider.WriteTempSecret(secret2, rule);
	}

	public void EnsureUserEncryptedSecret()
	{
		ConvertTempSecretToSecret();
		ReadSecretFromProvider();
	}

	public char[] ReadSecret()
	{
		if (_secret == null || _secret.Length == 0)
		{
			ReadSecretFromProvider();
		}
		return ReadSecretChars();
	}

	public bool SecretExists()
	{
		if (_secret != null && _secret.Length > 0)
		{
			return true;
		}
		try
		{
			_provider.ReadSecret();
			return true;
		}
		catch (SecretNotFoundException)
		{
			return false;
		}
	}

	private char[] ReadSecretChars()
	{
		if (_secret == null || _secret.Length == 0)
		{
			return null;
		}
		IntPtr intPtr = IntPtr.Zero;
		try
		{
			intPtr = Marshal.SecureStringToBSTR(_secret);
			char[] array = new char[_secret.Length];
			Marshal.Copy(intPtr, array, 0, _secret.Length);
			return array;
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.ZeroFreeBSTR(intPtr);
			}
		}
	}

	private void ReadSecretFromProvider()
	{
		byte[] secret = _provider.ReadSecret();
		byte[] bytes = _provider.DecryptSecret(secret, DataProtectionScope.CurrentUser);
		_secret.Clear();
		Array.ForEach(Encoding.UTF8.GetChars(bytes), _secret.AppendChar);
	}

	private void ConvertTempSecretToSecret()
	{
		byte[] array = _provider.ReadTempSecret();
		if (array != null)
		{
			byte[] secret = _provider.DecryptSecret(array, DataProtectionScope.LocalMachine);
			byte[] secret2 = _provider.EncryptSecret(secret, DataProtectionScope.CurrentUser);
			_provider.WriteSecret(secret2);
		}
		if (array != null)
		{
			_provider.DeleteTempSecret();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && _secret != null)
			{
				_secret.Dispose();
				_secret = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
