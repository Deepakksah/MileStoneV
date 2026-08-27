namespace VideoOS.EventServer.Utils;

public interface IRegistryTools
{
	string ReadRegistryValue(string propertyName, string defaultValue);

	string ReadRegistryValue(string propertyName);

	bool WriteRegistryValue(string propertyName, string propertyValue);
}
