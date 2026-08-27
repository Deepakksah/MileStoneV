namespace VideoOS.Utilities.Database.KeyManagement.KeyProvider;

public interface IDatabaseKeyProvider : IDatabaseKeyReader
{
	void UpdateKey(byte[] key);

	void InsertKey(byte[] key);
}
