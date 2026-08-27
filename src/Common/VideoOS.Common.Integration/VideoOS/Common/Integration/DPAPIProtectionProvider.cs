using System;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Integration;

public class DPAPIProtectionProvider : IProtectionProvider
{
	private byte[] _entropy = new Guid("4abddced-a6fe-4620-b6c8-8ce5099daf78").ToByteArray();

	private DataProtectionScope _scope = DataProtectionScope.LocalMachine;

	private Encoding _textEncoding = Encoding.UTF8;

	public DPAPIProtectionProvider()
	{
	}

	public DPAPIProtectionProvider(DataProtectionScope scope, byte[] entropy, Encoding textEncoding)
	{
		_scope = scope;
		_entropy = entropy;
		_textEncoding = textEncoding;
	}

	public string Decode(string encodedText)
	{
		if (encodedText == null)
		{
			throw new ArgumentNullException("encodedText");
		}
		byte[] bytes = ProtectedData.Unprotect(Convert.FromBase64String(encodedText), _entropy, _scope);
		return _textEncoding.GetString(bytes);
	}

	public string Encode(string clearText)
	{
		if (clearText == null)
		{
			throw new ArgumentNullException("clearText");
		}
		return Convert.ToBase64String(ProtectedData.Protect(_textEncoding.GetBytes(clearText), _entropy, _scope));
	}
}
