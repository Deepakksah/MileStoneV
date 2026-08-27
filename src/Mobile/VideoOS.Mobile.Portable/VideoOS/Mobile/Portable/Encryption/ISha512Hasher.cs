namespace VideoOS.Mobile.Portable.Encryption;

public interface ISha512Hasher
{
	byte[] ComputeHash(byte[] buffer);
}
