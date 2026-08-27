namespace VideoOS.Mobile.Portable.Encryption;

public interface IRandomGenerator
{
	void GetBytes(byte[] data);
}
