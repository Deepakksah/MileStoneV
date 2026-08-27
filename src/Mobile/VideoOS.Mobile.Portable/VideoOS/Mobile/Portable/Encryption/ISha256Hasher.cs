namespace VideoOS.Mobile.Portable.Encryption;

public interface ISha256Hasher
{
	byte[] ComputeHash(byte[] buffer);
}
