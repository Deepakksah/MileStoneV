using VideoOS.Common.Integration.Configuration.ConfigurationDataSetEncryption;

namespace VideoOS.Common.Integration.Configuration;

public static class XmlTransformationFactory
{
	public static IXmlTransformation Create(string type)
	{
		switch (type)
		{
		case "Database.SetEncryptionPassword":
			return new SetStoragePasswordEncryption();
		case "System.ChangeHardwarePassword":
			return new ChangeHardwarePasswordEncryption();
		case "System.AddHardware":
			return new AddHardwareEncryption();
		case "System.HardwareScan":
			return new HardwareScanEncryption();
		case "System.HardwareScanExpress":
			return new HardwareScanExpressEncryption();
		case "System.ActivateLicense":
			return new ActivateLicenseEncryption();
		case "System.ReplaceHardware":
			return new ReplaceHardwareEncryption();
		case "diffgram_delete":
		case "diffgram_insert":
		case "diffgram_update":
			return new DiffgramEncryption();
		default:
			return null;
		}
	}

	public static IXmlTransformation DiffGramCreate()
	{
		return new DiffgramEncryption();
	}
}
