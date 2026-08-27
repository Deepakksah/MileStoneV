using System;
using System.Reflection;
using System.Resources;

namespace VideoOS.Installer.Setup;

public static class ResourceManagerExtensions
{
	internal static bool AttachToDesignerClass(this ResourceManager resourceManager, Type resourceManagerType)
	{
		resourceManagerType.GetField("resourceMan", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, resourceManager);
		return true;
	}
}
