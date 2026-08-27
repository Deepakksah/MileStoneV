using System;
using System.IO;
using System.Reflection;

namespace VideoOS.Common.Integration;

public sealed class DllScanner
{
	public delegate void ClassTypeCallback(Type type);

	public static void Scan(Assembly assembly, ClassTypeCallback classTypeCallback)
	{
		if (assembly == null)
		{
			throw new ArgumentNullException("assembly");
		}
		if (classTypeCallback == null)
		{
			throw new ArgumentNullException("classTypeCallback");
		}
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
		foreach (string text in files)
		{
			try
			{
				if (text.Length >= 5)
				{
					string text2 = Path.GetExtension(text).ToUpperInvariant();
					if (text2 == ".DLL" || text2 == ".EXE")
					{
						Scan(Assembly.LoadFrom(text), classTypeCallback);
					}
				}
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
