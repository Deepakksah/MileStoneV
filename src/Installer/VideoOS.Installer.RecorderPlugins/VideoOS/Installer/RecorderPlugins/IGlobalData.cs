using System.Collections.Generic;

namespace VideoOS.Installer.RecorderPlugins;

public interface IGlobalData
{
	bool GetValueAsBoolean(string parameterName);

	bool HasParameter(string parameterName);

	void RemoveValue(string parameterName);

	void SetValue(string parameterName, string value, string location);

	Dictionary<string, string> ToDictionary();

	bool TryGetValue(string parameterName, out string value);
}
