namespace VideoOS.Utilities.Database.KeyManagement.KeyProvider;

public interface IDatabaseKeyReader
{
	byte[] ReadKey();

	byte[] TryReadKey();

	bool HasKey();
}
