using System;
using System.Drawing;
using System.Globalization;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration;

public abstract class ApplicationResources
{
	private OemResourcePackage resourcePackage;

	public virtual string Version => GetResourceString("Version");

	public ApplicationResources(OemResourcePackage resourcePackage)
	{
		this.resourcePackage = resourcePackage;
	}

	protected Image GetResourceImage(string name)
	{
		return ((OemResourceItemImage)resourcePackage.GetResourceItem(name)).ResourceValue;
	}

	protected int GetResourceInt32(string name)
	{
		return ((OemResourceItemInteger)resourcePackage.GetResourceItem(name)).ResourceValue;
	}

	protected Icon GetResourceIcon(string name)
	{
		return (Icon)resourcePackage.GetResourceItem(name).ResourceValueAsObject;
	}

	protected string GetResourceString(string name)
	{
		return (string)resourcePackage.GetResourceItem(name).ResourceValueAsObject;
	}

	protected bool GetResourceBool(string name)
	{
		string strA = (string)resourcePackage.GetResourceItem(name)?.ResourceValueAsObject;
		return string.Compare(strA, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0;
	}

	protected bool TryGetResourceString(string name, out string value)
	{
		value = null;
		OemResourceItemBase resourceItem = resourcePackage.GetResourceItem(name);
		if (resourceItem != null)
		{
			value = (string)resourceItem.ResourceValueAsObject;
			return true;
		}
		return false;
	}

	protected string GetResourceString(string name, CultureInfo cultureInfo, CultureInfo defaultCultureInfo, OemResourceCultureLookupOption options)
	{
		if (cultureInfo == null)
		{
			throw new ArgumentNullException("cultureInfo");
		}
		if (defaultCultureInfo == null)
		{
			throw new ArgumentNullException("defaultCultureInfo");
		}
		string name2 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", name, cultureInfo.Name);
		OemResourceItemBase resourceItem = resourcePackage.GetResourceItem(name2);
		if (resourceItem == null && options == OemResourceCultureLookupOption.ExactThenParent)
		{
			while (!cultureInfo.Parent.Equals(CultureInfo.InvariantCulture))
			{
				cultureInfo = cultureInfo.Parent;
				name2 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", name, cultureInfo);
				resourceItem = resourcePackage.GetResourceItem(name2);
				if (resourceItem != null)
				{
					break;
				}
			}
		}
		if (resourceItem == null)
		{
			name2 = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", name, defaultCultureInfo.Name);
			resourceItem = resourcePackage.GetResourceItem(name2);
		}
		if (resourceItem != null)
		{
			return resourceItem.ResourceValueAsObject as string;
		}
		return null;
	}
}
