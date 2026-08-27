using System.Text;

namespace VideoOS.Server.Admin.Utilities;

public class IniFile : IIniFile
{
	private string _fileName;

	public IniFile(string fileName)
	{
		_fileName = fileName;
	}

	public void WriteValue(string section, string key, string value)
	{
		NativeMethods.WritePrivateProfileString(section, key, value, _fileName);
	}

	public string ReadValue(string section, string key)
	{
		StringBuilder stringBuilder = new StringBuilder(255);
		NativeMethods.GetPrivateProfileString(section, key, "", stringBuilder, 255u, _fileName);
		return stringBuilder.ToString();
	}
}
