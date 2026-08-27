namespace VideoOS.Mobile.KeyManagement;

public interface IKeyProvider
{
	byte[] GetKey();

	byte[] GetOrCreateKey();
}
