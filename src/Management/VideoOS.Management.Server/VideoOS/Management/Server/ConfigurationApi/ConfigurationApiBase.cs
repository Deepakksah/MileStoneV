using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

public class ConfigurationApiBase
{
	public virtual bool SupportGetChildItemsWithItemTypes => false;

	public virtual ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path, string[] itemTypes, ItemFilter[] itemFilters, int maxResults, int startIndex, string[] childItemTypes, ItemFilter[] childFilters)
	{
		ConfigurationItem[] childItems = ((IConfigurationApiSupport)this).GetChildItems(requestContext, path);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		int num = 0;
		ConfigurationItem[] array = childItems;
		foreach (ConfigurationItem item in array)
		{
			ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == item.ItemType || f.ItemType + "Folder" == item.ItemType);
			if (itemFilter == null || !Match(item, itemFilter))
			{
				continue;
			}
			if (num >= startIndex)
			{
				list.Add(item);
				if (list.Count >= maxResults)
				{
					break;
				}
			}
			num++;
		}
		childItems = list.ToArray();
		if (childItemTypes != null && childItemTypes.Any())
		{
			FillChildren(requestContext, childItems, childItemTypes, childFilters);
		}
		return childItems;
	}

	public virtual ConfigurationItem[] QueryItems(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, int maxResults, int startIndex, List<ItemFilter> childItemFilters)
	{
		List<string> list = new List<string>();
		if (childItemFilters != null)
		{
			foreach (ItemFilter childItemFilter in childItemFilters)
			{
				list.Add(childItemFilter.ItemType);
			}
		}
		ConfigurationItem[] childItems = ((IConfigurationApiSupport)this).GetChildItems(requestContext, folderPath, new string[1] { itemFilter.ItemType }, new ItemFilter[1] { itemFilter }, maxResults, startIndex, list.ToArray(), childItemFilters?.ToArray());
		List<string> list2 = new List<string> { itemFilter.ItemType };
		list2.AddRange(list);
		using PropertyCache propertyCache = new PropertyCache();
		propertyCache.FillConfigurationItemProperties(requestContext, childItems, list2);
		return childItems;
	}

	public virtual int QueryItemCount(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter)
	{
		return ((IConfigurationApiSupport)this).GetChildItems(requestContext, folderPath, new string[1] { itemFilter.ItemType }, new ItemFilter[1] { itemFilter }, 999999, 0, null, null).Length;
	}

	public void FillChildren(IGeminiRequestContext requestContext, ConfigurationItem[] configurationItems, string[] itemTypes, ItemFilter[] itemFilters)
	{
		if (configurationItems == null)
		{
			throw new ArgumentNullException("configurationItems");
		}
		if (!configurationItems.Any())
		{
			return;
		}
		using (ChildrenCache childrenCache = new ChildrenCache())
		{
			childrenCache.CheckLoad(requestContext, configurationItems, itemTypes, itemFilters);
			FillChildren(requestContext, configurationItems, itemTypes, itemFilters, childrenCache);
		}
		using PropertyCache propertyCache = new PropertyCache();
		List<string> list = new List<string> { configurationItems[0].ItemType };
		list.AddRange(itemTypes);
		propertyCache.FillConfigurationItemProperties(requestContext, configurationItems, list);
	}

	private void FillChildren(IGeminiRequestContext requestContext, ConfigurationItem[] configurationItems, string[] itemTypes, ItemFilter[] itemFilters, ChildrenCache childrenCache)
	{
		IConfigurationApiSupport configurationApiSupport = null;
		string text = "";
		foreach (ConfigurationItem configurationItem in configurationItems)
		{
			if (configurationItem.ItemCategory == ItemCategories.ChildItem)
			{
				continue;
			}
			try
			{
				string text2 = string.Empty;
				if (!configurationItem.ItemType.EndsWith("Folder", StringComparison.Ordinal))
				{
					goto IL_0074;
				}
				text2 = configurationItem.ItemType.Substring(0, configurationItem.ItemType.Length - 6);
				if (itemTypes.Contains(text2))
				{
					goto IL_0074;
				}
				goto end_IL_002d;
				IL_0074:
				ConfigurationItemPath path = ConfigurationItemPath.CreatePath(configurationItem.Path);
				if (configurationItem.ItemType != text)
				{
					text = configurationItem.ItemType;
					try
					{
						configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(configurationItem.ItemType);
					}
					catch (NotSupportedException)
					{
						throw new PathNotFoundException("The server doesn't support the specified path:" + configurationItem.ItemType);
					}
				}
				if (configurationItem.ItemType.EndsWith("Folder", StringComparison.Ordinal))
				{
					if (!configurationItem.ChildrenFilled && configurationItem.ItemCategory != ItemCategories.ChildItem)
					{
						if (childrenCache.LoadedItemTypes.Contains(text2))
						{
							List<ConfigurationItem> childrenForParent = childrenCache.GetChildrenForParent(configurationItem.Path);
							if (childrenForParent.Any())
							{
								configurationItem.Children = FilterItems(itemFilters, childrenForParent);
							}
						}
						else if (configurationApiSupport.SupportGetChildItemsWithItemTypes)
						{
							configurationItem.Children = configurationApiSupport.GetChildItems(requestContext, path, itemTypes, itemFilters, int.MaxValue, 0, null, null);
						}
						else
						{
							configurationItem.Children = FilterItems(itemFilters, configurationApiSupport.GetChildItems(requestContext, path));
						}
					}
					if (configurationItem.Children != null && configurationItem.Children.Any())
					{
						FillChildren(requestContext, configurationItem.Children, itemTypes, itemFilters, childrenCache);
						configurationItem.ChildrenFilled = true;
					}
				}
				else
				{
					if (!configurationItem.ChildrenFilled && configurationItem.ItemCategory != ItemCategories.ChildItem)
					{
						ConfigurationItem[] children = ((!configurationApiSupport.SupportGetChildItemsWithItemTypes) ? configurationApiSupport.GetChildItems(requestContext, path) : configurationApiSupport.GetChildItems(requestContext, path, itemTypes, itemFilters, int.MaxValue, 0, null, null));
						configurationItem.Children = FilterOnType(children, itemTypes, itemFilters).ToArray();
						configurationItem.ChildrenFilled = true;
					}
					if (configurationItem.Children != null && configurationItem.Children.Length != 0 && !configurationItem.Children[0].ChildrenFilled)
					{
						FillChildren(requestContext, configurationItem.Children, itemTypes, itemFilters, childrenCache);
					}
				}
				end_IL_002d:;
			}
			catch (FeatureNotLicensedException)
			{
				configurationItem.Children = new ConfigurationItem[0];
				configurationItem.ChildrenFilled = true;
			}
		}
	}

	private static ConfigurationItem[] FilterItems(ItemFilter[] itemFilters, IEnumerable<ConfigurationItem> children)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (ConfigurationItem child in children)
		{
			ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == child.ItemType);
			if (itemFilter == null || Match(child, itemFilter))
			{
				list.Add(child);
			}
		}
		return list.ToArray();
	}

	private static List<ConfigurationItem> FilterOnType(ConfigurationItem[] children, string[] itemTypes, ItemFilter[] itemFilters)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (ConfigurationItem configurationItem in children)
		{
			bool flag = configurationItem.ItemType.EndsWith("Folder", StringComparison.OrdinalIgnoreCase);
			string childItemType = (flag ? configurationItem.ItemType.Substring(0, configurationItem.ItemType.Length - 6) : configurationItem.ItemType);
			if (itemTypes.Contains(childItemType))
			{
				ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == childItemType);
				if (itemFilter == null || flag || Match(configurationItem, itemFilter))
				{
					list.Add(configurationItem);
				}
			}
		}
		return list;
	}

	public static bool Match(ConfigurationItem item, ItemFilter itemFilter)
	{
		if (itemFilter == null)
		{
			return true;
		}
		string text = null;
		Property property = null;
		_ = RemoveFolderPartFromItemType(item.ItemType) != itemFilter.ItemType;
		if (itemFilter.EnableFilter != EnableFilter.All)
		{
			bool flag = true;
			if (item.EnableProperty != null)
			{
				flag = item.EnableProperty.Enabled;
			}
			if (itemFilter.EnableFilter == EnableFilter.Enabled != flag)
			{
				return false;
			}
		}
		if (itemFilter.PropertyFilters == null || !itemFilter.PropertyFilters.Any())
		{
			return true;
		}
		if (item.Properties == null)
		{
			return false;
		}
		PropertyFilter[] propertyFilters = itemFilter.PropertyFilters;
		foreach (PropertyFilter filter in propertyFilters)
		{
			if (filter.Key == "IDRecorder" || filter.Key == "IDHardware")
			{
				continue;
			}
			property = item.Properties.FirstOrDefault((Property prop) => prop.Key.Equals(filter.Key, StringComparison.OrdinalIgnoreCase));
			if (property == null)
			{
				continue;
			}
			text = property.Value;
			switch (filter.Operator)
			{
			case Operator.BeginsWith:
				if (text.IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) != 0)
				{
					return false;
				}
				break;
			case Operator.Contains:
				if (text.IndexOf(filter.Value, StringComparison.CurrentCultureIgnoreCase) < 0)
				{
					return false;
				}
				break;
			case Operator.Equals:
				if (!text.Equals(filter.Value, StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				break;
			case Operator.GreaterThan:
				if (property == null)
				{
					break;
				}
				if (property.ValueType == "Double" || property.ValueType == "Int" || property.ValueType == "Slider" || property.ValueType == "Progress")
				{
					double result3 = 0.0;
					double result4 = 0.0;
					if (double.TryParse(text, out result3) && double.TryParse(filter.Value, out result4) && !(result3 > result4))
					{
						return false;
					}
				}
				else if (string.Compare(text, filter.Value, StringComparison.OrdinalIgnoreCase) <= 0)
				{
					return false;
				}
				break;
			case Operator.LessThan:
				if (property == null)
				{
					break;
				}
				if (property.ValueType == "Double" || property.ValueType == "Int" || property.ValueType == "Slider" || property.ValueType == "Progress")
				{
					double result = 0.0;
					double result2 = 0.0;
					if (double.TryParse(text, out result) && double.TryParse(filter.Value, out result2) && !(result < result2))
					{
						return false;
					}
				}
				else if (string.Compare(text, filter.Value, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return false;
				}
				break;
			case Operator.NotEquals:
				if (!(text != filter.Value))
				{
					return false;
				}
				break;
			}
		}
		return true;
	}

	public static void ThrowIfArgumentsInvalid(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "ThrowIfArgumentsInvalid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\ConfigurationApiBase.cs", 352);
		path.NullCheck("", "ThrowIfArgumentsInvalid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\ConfigurationApiBase.cs", 353);
	}

	public static void ThrowIfArgumentsInvalid(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ThrowIfArgumentsInvalid(requestContext, path);
		item.NullCheck("", "ThrowIfArgumentsInvalid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\ConfigurationApiBase.cs", 359);
	}

	public static void ThrowIfAnyParameterNull(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ThrowIfArgumentsInvalid(requestContext, item, path);
		parentPath.NullCheck("", "ThrowIfAnyParameterNull", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\ConfigurationApiBase.cs", 365);
	}

	public static void ThrowIfPathGuidInvalid(ConfigurationItemPath path)
	{
		if (!Guid.TryParse(path.Id, out var _))
		{
			throw new PathInvalidException();
		}
	}

	public static string RemoveFolderPartFromItemType(string itemType)
	{
		if (itemType.EndsWith("Folder", StringComparison.Ordinal))
		{
			return itemType.Substring(0, itemType.Length - 6);
		}
		return itemType;
	}

	public static bool HasGeneralReadRights(IGeminiRequestContext requestContext)
	{
		if (requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			return true;
		}
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "DifferentiatedAdministratorSecurity"))
		{
			return requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, requestContext.AuthenticatedUserSid, "GENERIC_READ");
		}
		return false;
	}
}
