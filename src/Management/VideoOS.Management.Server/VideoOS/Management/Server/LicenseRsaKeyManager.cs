namespace VideoOS.Management.Server;

public class LicenseRsaKeyManager : GenericReadonlyStringManager
{
	public LicenseRsaKeyManager()
		: base("LicenseRsaKey")
	{
	}

	public override string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		return requestContext.GetEncryptionService().Encrypt(value);
	}

	public override string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		return requestContext.GetEncryptionService().Decrypt(value);
	}

	public override string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		return requestContext.GetEncryptionService().Decrypt(value);
	}
}
