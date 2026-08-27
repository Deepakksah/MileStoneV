using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public class ResourceLoader
{
	private readonly List<string> _resourList = new List<string>();

	public List<string> ResourList => _resourList;

	public void FillResources(Type type, string resourceName)
	{
		ResourceManager resourceManager = new ResourceManager(type);
		CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
		foreach (CultureInfo culture in cultures)
		{
			try
			{
				ResourceSet resourceSet = resourceManager.GetResourceSet(culture, createIfNotExists: true, tryParents: false);
				if (resourceSet != null)
				{
					string text = resourceSet.GetString(resourceName);
					if (!string.IsNullOrEmpty(text))
					{
						ResourList.Add(text);
					}
				}
			}
			catch (CultureNotFoundException ex)
			{
				Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
			}
		}
		foreach (string resour in _resourList)
		{
			_ = resour;
		}
	}
}
