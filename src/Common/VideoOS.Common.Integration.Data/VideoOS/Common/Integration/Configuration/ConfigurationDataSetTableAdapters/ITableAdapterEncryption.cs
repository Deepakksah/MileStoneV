using VideoOS.Common.Crypto;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public interface ITableAdapterEncryption
{
	void Encrypt(ConfigurationDataSet ds, ISymmetricEncryption symmetricEncryption);

	void Decrypt(ConfigurationDataSet ds, ISymmetricEncryption symmetricEncryption);
}
