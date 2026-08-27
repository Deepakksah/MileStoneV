using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace VideoOS.Common.SharedComponent.Data.ViewLayout;

public class ViewLayoutGroupConstants
{
	protected static Dictionary<Guid, string> _predefinedLayoutGroups;

	public static string LayoutGroup4x3;

	public static string LayoutGroup16x9;

	public static string LayoutGroupCustom;

	public static Dictionary<Guid, string> PredefinedLayoutGroups => _predefinedLayoutGroups;

	static ViewLayoutGroupConstants()
	{
		_predefinedLayoutGroups = new Dictionary<Guid, string>();
		LayoutGroup4x3 = "7EB5B9CC-D56F-4785-BA72-4E71FADA8C6B";
		LayoutGroup16x9 = "14C3A703-6835-4843-B18C-8DB95436A80C";
		LayoutGroupCustom = "9ABA35C0-6AE1-446f-8CCF-25B2208E14C9";
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		ResourceManager resourceManager = new ResourceManager(executingAssembly.GetName().Name + ".Properties.Resources", executingAssembly);
		_predefinedLayoutGroups.Add(new Guid(LayoutGroup16x9), resourceManager.GetString("ViewLayoutGroup16x9", CultureInfo.CurrentUICulture));
		_predefinedLayoutGroups.Add(new Guid(LayoutGroup4x3), resourceManager.GetString("ViewLayoutGroup4x3", CultureInfo.CurrentUICulture));
		_predefinedLayoutGroups.Add(new Guid(LayoutGroupCustom), resourceManager.GetString("ViewLayoutGroupCustom", CultureInfo.CurrentUICulture));
	}
}
