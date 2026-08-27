using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Manager;

public interface IPluginTypeLoader
{
	IEnumerable<Type> GetSpecificTypes(string assemblyFileName, Type interfaceType);
}
