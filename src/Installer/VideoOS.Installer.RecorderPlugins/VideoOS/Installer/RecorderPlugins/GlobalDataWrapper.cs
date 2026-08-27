using System.Collections.Generic;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.RecorderPlugins;

public class GlobalDataWrapper : IGlobalData
{
	public bool GetValueAsBoolean(string parameterName)
	{
		return GlobalData.GetValueAsBoolean(parameterName);
	}

	public bool HasParameter(string parameterName)
	{
		return GlobalData.HasParameter(parameterName);
	}

	public void RemoveValue(string parameterName)
	{
		GlobalData.RemoveValue(parameterName);
	}

	public void SetValue(string parameterName, string value, string location)
	{
		GlobalData.SetValue(parameterName, value, location);
	}

	public Dictionary<string, string> ToDictionary()
	{
		return GlobalData.ToDict();
	}

	public bool TryGetValue(string parameterName, out string value)
	{
		return GlobalData.TryGetValue(parameterName, out value);
	}
}
