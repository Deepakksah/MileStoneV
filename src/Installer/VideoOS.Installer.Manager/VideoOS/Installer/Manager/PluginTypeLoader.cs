using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Manager;

public class PluginTypeLoader : IPluginTypeLoader
{
	public IEnumerable<Type> GetSpecificTypes(string assemblyFileName, Type interfaceType)
	{
		if (assemblyFileName == null)
		{
			throw new ArgumentNullException("assemblyFileName");
		}
		IEnumerable<Type> result = new List<Type>();
		try
		{
			string text = (assemblyFileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? Path.GetFileNameWithoutExtension(assemblyFileName) : assemblyFileName);
			if (!string.IsNullOrEmpty(text))
			{
				result = (from t in Assembly.Load(text).GetTypes()
					where !t.IsAbstract && !t.IsInterface && interfaceType.IsAssignableFrom(t)
					select t).ToList();
			}
		}
		catch (FileNotFoundException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Critical);
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File not found {0}.", assemblyFileName));
		}
		return result;
	}
}
