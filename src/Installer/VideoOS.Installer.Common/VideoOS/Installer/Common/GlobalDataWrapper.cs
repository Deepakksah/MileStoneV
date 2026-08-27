using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Common;

public class GlobalDataWrapper : IGlobalData
{
	public IEnumerable<IPackageParameter> Settings => GlobalData.Settings;

	public bool GetValueAsBoolean(string name)
	{
		return GlobalData.GetValueAsBoolean(name);
	}

	public bool? GetValueAsNullableBoolean(string name)
	{
		return GlobalData.GetValueAsNullableBoolean(name);
	}

	public bool HasParameter(string name)
	{
		return GlobalData.HasParameter(name);
	}

	public void RemoveValue(string name)
	{
		GlobalData.RemoveValue(name);
	}

	public void SetValue(string name, string value, string location)
	{
		GlobalData.SetValue(name, value, location);
	}

	public bool TryGetValue(string name, out string value)
	{
		return GlobalData.TryGetValue(name, out value);
	}
}
