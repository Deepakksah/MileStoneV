using System;
using System.Text;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Encryption;

public class PBE
{
	private const int KeySize = 32;

	private const int IvSize = 16;

	public static readonly int KeybytesSize = 48;

	private readonly byte[] _iv = new byte[16];

	private readonly byte[] _key = new byte[32];

	private readonly IAesCrypter _implementer;

	private EncodingPadding _padding;

	public EncodingPadding Padding
	{
		get
		{
			return _padding;
		}
		set
		{
			_padding = value;
			if (_implementer != null)
			{
				_implementer.Padding = _padding;
			}
		}
	}

	public PBE(byte[] keyBytes)
	{
		if (keyBytes.Length < KeybytesSize)
		{
			throw new PbeCryptographicException("Invalid keyBytes size!");
		}
		Array.Copy(keyBytes, 0, _iv, 0, 16);
		Array.Copy(keyBytes, 16, _key, 0, 32);
		_implementer = new AesCrypter(_key, _iv);
	}

	public byte[] GetSecretKey()
	{
		byte[] array = new byte[KeybytesSize];
		Array.Copy(_iv, 0, array, 0, 16);
		Array.Copy(_key, 0, array, 16, 32);
		return array;
	}

	public string GetEncryptedValue(string inputValue)
	{
		byte[] bytes = new UTF8Encoding().GetBytes(inputValue);
		return Convert.ToBase64String(_implementer.Encrypt(bytes));
	}

	public string GetDecryptedValue(string inputValue)
	{
		byte[] input = Convert.FromBase64String(inputValue);
		byte[] array = _implementer.Decrypt(input);
		return new UTF8Encoding().GetString(array, 0, array.Length);
	}
}
