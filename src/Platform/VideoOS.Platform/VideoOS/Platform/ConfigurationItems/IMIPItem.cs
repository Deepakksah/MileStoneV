namespace VideoOS.Platform.ConfigurationItems;

public interface IMIPItem
{
	bool Enabled { get; set; }

	string GisPoint { get; set; }

	string Id { get; }

	string LastModified { get; set; }

	MIPItemFolder MIPItemFolder { get; }

	ConfigurationApiProperties Properties { get; }

	void Save();
}
