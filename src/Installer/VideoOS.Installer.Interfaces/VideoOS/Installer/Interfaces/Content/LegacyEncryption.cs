using System;

namespace VideoOS.Installer.Interfaces.Content;

public class LegacyEncryption
{
	public static readonly LegacyEncryption Empty = new LegacyEncryption(hasValue: false);

	public static readonly LegacyEncryption Unencrypted = new LegacyEncryption(hasValue: true);

	public bool HasValue { get; }

	public bool IsEncrypted => Thumbprint != null;

	public string Thumbprint { get; private set; }

	private LegacyEncryption()
	{
	}

	private LegacyEncryption(bool hasValue)
	{
		HasValue = hasValue;
	}

	private LegacyEncryption(string thumbprint)
		: this(hasValue: true)
	{
		if (string.IsNullOrEmpty(thumbprint))
		{
			throw new ArgumentOutOfRangeException("thumbprint", "value cannot null or empty");
		}
		Thumbprint = thumbprint;
	}

	public static LegacyEncryption Encrypted(string thumbprint)
	{
		return new LegacyEncryption(thumbprint);
	}
}
