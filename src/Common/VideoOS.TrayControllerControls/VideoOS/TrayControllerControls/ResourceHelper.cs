using System;
using System.Drawing;
using System.Reflection;

namespace VideoOS.TrayControllerControls;

public class ResourceHelper
{
	public static Icon LoadIcon(Assembly assembly, string path, string name)
	{
		try
		{
			string name2 = path + "." + name + ".ico";
			return new Icon(assembly.GetManifestResourceStream(name2));
		}
		catch (Exception)
		{
			throw;
		}
	}
}
