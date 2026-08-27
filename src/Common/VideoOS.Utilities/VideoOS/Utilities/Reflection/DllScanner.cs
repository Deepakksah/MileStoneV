using System;
using System.IO;
using System.Reflection;

namespace VideoOS.Utilities.Reflection;

public sealed class DllScanner
{
	public static void Scan(Assembly assembly, ClassTypeCallback classTypeCallback)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (type.IsClass)
			{
				classTypeCallback(type);
			}
		}
	}

	public static void Scan(string path, ClassTypeCallback classTypeCallback)
	{
		if (!Directory.Exists(path))
		{
			return;
		}
		string[] files = Directory.GetFiles(path, "*.dll");
		foreach (string assemblyFile in files)
		{
			try
			{
				Scan(Assembly.LoadFrom(assemblyFile), classTypeCallback);
			}
			catch (BadImageFormatException)
			{
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception)
			{
			}
		}
	}
}
