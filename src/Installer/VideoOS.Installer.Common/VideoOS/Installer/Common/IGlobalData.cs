using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Common;

[CLSCompliant(true)]
public interface IGlobalData
{
	IEnumerable<IPackageParameter> Settings { get; }

	bool GetValueAsBoolean(string name);

	bool? GetValueAsNullableBoolean(string name);

	bool HasParameter(string name);

	void RemoveValue(string name);

	void SetValue(string name, string value, string location);

	bool TryGetValue(string name, out string value);
}
