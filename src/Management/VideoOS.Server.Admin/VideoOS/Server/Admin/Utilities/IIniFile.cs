namespace VideoOS.Server.Admin.Utilities;

public interface IIniFile
{
	string ReadValue(string section, string key);

	void WriteValue(string section, string key, string value);
}
