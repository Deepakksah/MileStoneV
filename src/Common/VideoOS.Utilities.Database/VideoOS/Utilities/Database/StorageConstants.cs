namespace VideoOS.Utilities.Database;

public class StorageConstants
{
	public const string REGISTRY_PATH_CONNECTIONSTRING = "SOFTWARE\\VideoOS\\Server\\ConnectionString";

	public static readonly byte[] Entropy = new byte[16]
	{
		2, 8, 2, 6, 5, 1, 1, 9, 8, 6,
		0, 1, 9, 4, 1, 7
	};

	public const string FAIL_TO_UPDATE_REGISTRY_MESSAGE = "Setting connection string failed";

	public const string FAIL_TO_READ_REGISTRY_MESSAGE = "Reading connection string failed";
}
