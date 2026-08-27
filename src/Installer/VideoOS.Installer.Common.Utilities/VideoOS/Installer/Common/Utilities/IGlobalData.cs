namespace VideoOS.Installer.Common.Utilities;

public interface IGlobalData
{
	bool GetValueAsBoolean(string name);

	bool? GetValueAsNullableBoolean(string name);

	bool HasParameter(string name);

	void RemoveValue(string name);

	void SetValue(string name, string value, string location);

	bool TryGetValue(string name, out string value);
}
