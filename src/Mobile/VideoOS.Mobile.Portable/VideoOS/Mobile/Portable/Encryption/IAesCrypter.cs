using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Encryption;

public interface IAesCrypter
{
	EncodingPadding Padding { get; set; }

	byte[] Encrypt(byte[] input);

	byte[] Decrypt(byte[] input);
}
