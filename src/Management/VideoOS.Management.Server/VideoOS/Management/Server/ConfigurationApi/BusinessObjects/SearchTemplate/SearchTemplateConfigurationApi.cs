using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.SearchTemplate;

[EntityName("ConfigAPISaveSearches")]
[EntityName("ConfigAPIFindSaveSearches")]
[EntityName("ConfigAPISaveSearchesFolder")]
public class SearchTemplateConfigurationApi : SearchTemplateConfigurationApiBase
{
	public SearchTemplateConfigurationApi(ISearchTemplateRepository repository)
		: base(repository)
	{
	}

	public SearchTemplateConfigurationApi()
		: base(null)
	{
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		ThrowIfNotFeatured(requestContext, path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				List<ISearchTemplateItem> list = new List<ISearchTemplateItem>();
				ISearchTemplateFind searchTemplateFind = SearchTemplateFind.Default;
				list.AddRange(base.Repository.FindSearchTemplates(requestContext, searchTemplateFind.Count, searchTemplateFind.StartIndex, searchTemplateFind.KeywordFilter, searchTemplateFind.PrivateOnly));
				return list.Select((ISearchTemplateItem x) => ConvertToSearchTemplateConfigurationItem(x)).ToArray();
			}
			if (string.Equals(path.ParentItemType, "FindSaveSearches", StringComparison.Ordinal))
			{
				ISearchTemplateItem[] source;
				try
				{
					source = base.Repository.FindSearchTemplates(requestContext, path.Id.ToGuid());
				}
				catch (SearchTemplateNotExistException ex)
				{
					throw new PropertyInvalidException(ex.Message);
				}
				return source.Select((ISearchTemplateItem x) => ConvertToSearchTemplateConfigurationItem(x)).ToArray();
			}
			throw new NotSupportedException();
		}
		if (string.Equals(path.ItemType, "FindSaveSearches", StringComparison.Ordinal))
		{
			ISearchTemplateItem[] source2;
			try
			{
				source2 = base.Repository.FindSearchTemplates(requestContext, path.Id.ToGuid());
			}
			catch (SearchTemplateNotExistException ex2)
			{
				throw new PropertyInvalidException(ex2.Message);
			}
			return source2.Select((ISearchTemplateItem x) => ConvertToSearchTemplateConfigurationItem(x)).ToArray();
		}
		return new ConfigurationItem[0];
	}

	public override ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ThrowIfArgumentsInvalid(requestContext, item, path, parentPath);
		method.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\SearchTemplate\\SearchTemplateConfigurationApi.cs", 80);
		ThrowIfNotFeatured(requestContext, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddSaveSearch":
			if (!flag)
			{
				return CreateInvokeInfoAddSearchTemplate(item, method);
			}
			return AddSearchTemplate(requestContext, item, path);
		case "UpdateSaveSearch":
			if (!flag)
			{
				return CreateInvokeInfoUpdateSearchTempate(item, method);
			}
			return UpdateSearchTemplate(requestContext, item, path);
		case "RemoveSaveSearch":
			if (!flag)
			{
				return CreateInvokeInfoRemoveSearchTemplate(item, method);
			}
			return RemoveSearchTemplate(requestContext, item, path);
		case "GetSaveSearch":
			if (!flag)
			{
				return CreateInvokeInfoGetSearchTemplate(item, method);
			}
			return GetSearchTemplate(requestContext, item, path);
		case "FindSaveSearches":
			if (!flag)
			{
				return CreateInvokeInfoSearchTemplateFind(item, method);
			}
			return AddSearchTemplateFind(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown method: {0}", method));
		}
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		ThrowIfNotFeatured(requestContext, path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("SaveSearchesFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (string.Equals(path.ItemType, "FindSaveSearches", StringComparison.Ordinal))
		{
			ISearchTemplateFind searchTemplateFind = null;
			try
			{
				searchTemplateFind = base.Repository.QuerySearchTemplateFind(requestContext, path.Id.ToGuid());
			}
			catch (SearchTemplateNotExistException)
			{
				throw new PathNotFoundException(path.Text);
			}
			return ConvertToSearchTemplateFindConfigurationItem(searchTemplateFind);
		}
		ISearchTemplateItem searchTemplateItem = null;
		try
		{
			searchTemplateItem = base.Repository.QuerySearchTemplate(requestContext, path.Id.ToGuid());
		}
		catch (SearchTemplateNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToSearchTemplateConfigurationItem(searchTemplateItem);
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ThrowIfArgumentsInvalid(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfNotFeatured(requestContext, path);
		if (string.Equals(path.ItemType, "FindSaveSearches", StringComparison.Ordinal))
		{
			ValidateResult validateResult = ValidateSearchTemplateFindItemInternal(requestContext, item, path, out var count, out var startIndex, out var keywordFilter, out var privateOnly);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			Guid id = path.Id.ToGuid();
			ISearchTemplateFind searchTemplateFind;
			try
			{
				searchTemplateFind = base.Repository.UpdateSearchTemplateFind(requestContext, id, count, startIndex, keywordFilter, privateOnly);
			}
			catch (SearchTemplateUpdateException)
			{
				throw new PathNotFoundException(path.Text);
			}
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = ConvertToSearchTemplateFindConfigurationItem(searchTemplateFind);
			return validateResult;
		}
		ValidateResult validateResult2 = ValidateSearchTemplateItemInternal(requestContext, item, path, out var name, out var description, out var searchQuery, out var availability, out var includesScopeItems);
		if (!validateResult2.ValidatedOk)
		{
			return validateResult2;
		}
		Guid id2 = path.Id.ToGuid();
		ISearchTemplateItem searchTemplate;
		try
		{
			searchTemplate = base.Repository.UpdateSearchTemplate(requestContext, id2, name, description, searchQuery, availability.Value, includesScopeItems);
		}
		catch (SearchTemplateUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
		validateResult2 = new ValidateResult();
		validateResult2.ValidatedOk = true;
		validateResult2.ResultItem = ConvertToSearchTemplateConfigurationItem(searchTemplate);
		return validateResult2;
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ThrowIfArgumentsInvalid(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfNotFeatured(requestContext, path);
		int count;
		int startIndex;
		string keywordFilter;
		bool privateOnly;
		if (string.Equals(path.ItemType, "FindSaveSearches", StringComparison.Ordinal))
		{
			return ValidateSearchTemplateFindItemInternal(requestContext, item, path, out count, out startIndex, out keywordFilter, out privateOnly);
		}
		string name;
		string description;
		string searchQuery;
		SearchTemplateAvailability? availability;
		bool includesScopeItems;
		return ValidateSearchTemplateItemInternal(requestContext, item, path, out name, out description, out searchQuery, out availability, out includesScopeItems);
	}

	private ConfigurationItem AddSearchTemplate(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		ThrowIfPathInvalid(path);
		if (item.Properties == null)
		{
			throw new ArgumentException("Invalid properties");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SearchQuery", out string value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Availability", out SearchTemplateAvailability? value5);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("IncludesScopeItems", out string value6);
		if (!Guid.TryParse(value, out var result) || result == Guid.Empty)
		{
			throw new ArgumentException("id incorrect");
		}
		if (value5.HasValue && value5.Value != SearchTemplateAvailability.Private && value5.Value != SearchTemplateAvailability.Public)
		{
			value5 = SearchTemplateAvailability.Public;
		}
		if (!bool.TryParse(value6, out var result2))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid includesScopeItems, Path: {0}", path.Text));
		}
		ConfigurationItem configurationItem = ConvertToSearchTemplateConfigurationItem(base.Repository.CreateSearchTemplate(requestContext, result, value2, value3, value4, value5.Value, result2));
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private ConfigurationItem UpdateSearchTemplate(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		ThrowIfPathInvalid(path);
		if (item.Properties == null)
		{
			throw new ArgumentException("Invalid properties");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SearchQuery", out string value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Availability", out SearchTemplateAvailability? value5);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("IncludesScopeItems", out string value6);
		if (!Guid.TryParse(value, out var result) || result == Guid.Empty)
		{
			throw new ArgumentException("id incorrect");
		}
		if (value5.HasValue && value5.Value != SearchTemplateAvailability.Private && value5.Value != SearchTemplateAvailability.Public)
		{
			value5 = SearchTemplateAvailability.Public;
		}
		if (!bool.TryParse(value6, out var result2))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid includesScopeItems, Path: {0}", path.Text));
		}
		ISearchTemplateItem searchTemplate;
		try
		{
			searchTemplate = base.Repository.UpdateSearchTemplate(requestContext, result, value2, value3, value4, value5.Value, result2);
		}
		catch (SearchTemplateUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItem configurationItem = ConvertToSearchTemplateConfigurationItem(searchTemplate);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private ConfigurationItem RemoveSearchTemplate(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (item.Properties == null)
		{
			throw new ArgumentException("Invalid properties");
		}
		Guid result = Guid.Empty;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value);
		if (!Guid.TryParse(value, out result) || result == Guid.Empty)
		{
			throw new ArgumentException("id incorrect");
		}
		try
		{
			base.Repository.DeleteSearchTemplate(requestContext, result);
		}
		catch (SearchTemplateDeleteException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", false, new Property("Id", result.ToString()));
	}

	private ConfigurationItem GetSearchTemplate(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (item.Properties == null)
		{
			throw new ArgumentException("Invalid properties");
		}
		Guid result = Guid.Empty;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value);
		if (!Guid.TryParse(value, out result) || result == Guid.Empty)
		{
			throw new ArgumentException("id incorrect");
		}
		ISearchTemplateItem searchTemplate;
		try
		{
			searchTemplate = base.Repository.QuerySearchTemplate(requestContext, result);
		}
		catch (SearchTemplateNotExistException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToSearchTemplateConfigurationItem(searchTemplate);
	}

	private ConfigurationItem AddSearchTemplateFind(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (item.Properties == null)
		{
			throw new ArgumentException("Invalid properties");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Count", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("StartIndex", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("KeywordFilter", out string value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PrivateOnly", out string value4);
		if (!int.TryParse(value, out var result) || result < 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid count, Path: {0}", path.Text));
		}
		if (!int.TryParse(value2, out var result2) || result2 < 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid startIndex, Path: {0}", path.Text));
		}
		if (!bool.TryParse(value4, out var result3))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid privateOnly, Path: {0}", path.Text));
		}
		ConfigurationItem configurationItem = ConvertToSearchTemplateFindConfigurationItem(base.Repository.CreateSearchTemplateFind(requestContext, result, result2, value3, result3));
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem CreateInvokeInfoAddSearchTemplate(ConfigurationItem item, string method)
	{
		Guid id = Guid.NewGuid();
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddSearchTemplate;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { method };
		configurationItem.Properties = new Property[6]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(id),
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			SearchTemplatePropertyDefinition.SearchQueryProperty(string.Empty),
			SearchTemplatePropertyDefinition.AvailabilityProperty(SearchTemplateAvailability.Public, isSettable: true),
			SearchTemplatePropertyDefinition.IncludesScopeItemsProperty(includesScopeItems: false, isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoUpdateSearchTempate(ConfigurationItem item, string method)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardUpdateSearchTemplate;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { method };
		configurationItem.Properties = new Property[6]
		{
			SearchTemplatePropertyDefinition.StringProperty(string.Empty, isSettable: true),
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			SearchTemplatePropertyDefinition.SearchQueryProperty(string.Empty),
			SearchTemplatePropertyDefinition.AvailabilityProperty(SearchTemplateAvailability.Public, isSettable: true),
			SearchTemplatePropertyDefinition.IncludesScopeItemsProperty(includesScopeItems: false, isSettable: false)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveSearchTemplate(ConfigurationItem item, string method)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveSearchTemplate;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { method };
		configurationItem.Properties = new Property[1] { SearchTemplatePropertyDefinition.StringProperty(string.Empty, isSettable: true) };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoGetSearchTemplate(ConfigurationItem item, string method)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardGetSearchTemplate;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { method };
		configurationItem.Properties = new Property[1] { SearchTemplatePropertyDefinition.StringProperty(string.Empty, isSettable: true) };
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoSearchTemplateFind(ConfigurationItem item, string method)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardFindSearchTemplates;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { method };
		configurationItem.Properties = new Property[4]
		{
			SearchTemplatePropertyDefinition.CountProperty(1000),
			SearchTemplatePropertyDefinition.StartIndexProperty(0),
			SearchTemplatePropertyDefinition.KeywordFilterProperty(string.Empty),
			SearchTemplatePropertyDefinition.PrivateOnlyProperty(privateOnly: false)
		};
		return configurationItem;
	}

	private ValidateResult ValidateSearchTemplateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out string searchQuery, out SearchTemplateAvailability? availability, out bool includesScopeItems)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		ThrowIfPathInvalid(path);
		if (!path.IsFolder)
		{
			Guid id = path.Id.ToGuid();
			try
			{
				base.Repository.QuerySearchTemplate(requestContext, id);
			}
			catch (SearchTemplateNotExistException)
			{
				throw new PathNotFoundException(path.Text);
			}
			if (item.Properties == null)
			{
				throw new ArgumentException("Invalid properties");
			}
			name = null;
			description = null;
			searchQuery = null;
			availability = null;
			includesScopeItems = false;
			ValidateResult validateResult;
			try
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SearchQuery", out searchQuery);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Availability", out availability);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("IncludesScopeItems", out string value);
				if (availability.HasValue && availability.Value != SearchTemplateAvailability.Private && availability.Value != SearchTemplateAvailability.Public)
				{
					availability = SearchTemplateAvailability.Public;
				}
				if (!bool.TryParse(value, out includesScopeItems))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid includesScopeItems, Path: {0}", path.Text));
				}
			}
			catch (ArgumentException exception)
			{
				validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		throw new NotSupportedException();
	}

	private ValidateResult ValidateSearchTemplateFindItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out int count, out int startIndex, out string keywordFilter, out bool privateOnly)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		ThrowIfPathInvalid(path);
		if (!path.IsFolder)
		{
			Guid id = path.Id.ToGuid();
			try
			{
				base.Repository.QuerySearchTemplateFind(requestContext, id);
			}
			catch (SearchTemplateNotExistException)
			{
				throw new PathNotFoundException(path.Text);
			}
			count = 0;
			startIndex = 0;
			keywordFilter = string.Empty;
			privateOnly = false;
			ValidateResult validateResult;
			try
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Count", out string value);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("StartIndex", out string value2);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("KeywordFilter", out keywordFilter);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PrivateOnly", out string value3);
					if (!int.TryParse(value, out count) || count < 0)
					{
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid count, Path: {0}", path.Text));
					}
					if (!int.TryParse(value2, out startIndex) || startIndex < 0)
					{
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid startIndex, Path: {0}", path.Text));
					}
					if (!bool.TryParse(value3, out privateOnly))
					{
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid privateOnly, Path: {0}", path.Text));
					}
				}
			}
			catch (ArgumentException exception)
			{
				validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		throw new NotSupportedException();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal) && !string.Equals(path.ParentItemType, "FindSaveSearches", StringComparison.Ordinal))
			{
				throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Invalid parent. Path: {0}", path.Text));
			}
			if (string.Equals(path.ParentItemType, "FindSaveSearches", StringComparison.Ordinal))
			{
				if (!string.Equals(path.ItemType, "SaveSearchesFolder", StringComparison.Ordinal))
				{
					throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Invalid path. Path: {0}", path.Text));
				}
				if (!path.Id.IsGuid())
				{
					throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Invalid id. Path: {0}", path.Text));
				}
			}
		}
		else
		{
			if (!string.Equals(path.ItemType, "SaveSearches", StringComparison.Ordinal) && !string.Equals(path.ItemType, "FindSaveSearches", StringComparison.Ordinal))
			{
				throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Invalid path. Path: {0}", path.Text));
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Invalid id. Path: {0}", path.Text));
			}
		}
	}

	private static void ThrowIfArgumentsInvalid(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		parentPath.NullCheck("", "ThrowIfArgumentsInvalid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\SearchTemplate\\SearchTemplateConfigurationApi.cs", 694);
	}

	private void ThrowIfNotFeatured(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (!base.Repository.IsLicensed(requestContext))
		{
			throw new PathInvalidException(string.Format(CultureInfo.InvariantCulture, "Invalid parent. Path: {0}", path.Text));
		}
	}

	private static ConfigurationItem ConvertToSearchTemplateConfigurationItem(ISearchTemplateItem searchTemplate)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "SaveSearches";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = searchTemplate.Name;
		configurationItem.ParentPath = ConfigurationItemPath.CreateSystemFolderPath("SaveSearchesFolder").Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath("SaveSearches", searchTemplate.Id.ToString()).Text;
		configurationItem.MethodIds = new string[0];
		configurationItem.Properties = new Property[6]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(searchTemplate.Id),
			GeneralPropertyDefinitions.GetNameProperty(searchTemplate.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(searchTemplate.Description),
			SearchTemplatePropertyDefinition.SearchQueryProperty(searchTemplate.SearchQuery),
			SearchTemplatePropertyDefinition.AvailabilityProperty(searchTemplate.Availability, isSettable: true),
			SearchTemplatePropertyDefinition.IncludesScopeItemsProperty(searchTemplate.IncludesScopeItems, isSettable: false)
		};
		return configurationItem;
	}

	private static ConfigurationItem ConvertToSearchTemplateFindConfigurationItem(ISearchTemplateFind searchTemplateFind)
	{
		string displayName = string.Format(CultureInfo.InvariantCulture, "[\"{0}\",{1},{2}]", searchTemplateFind.KeywordFilter, searchTemplateFind.Count, searchTemplateFind.StartIndex);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "FindSaveSearches";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = displayName;
		configurationItem.ParentPath = ConfigurationItemPath.CreateSystemFolderPath("SaveSearchesFolder").Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath("FindSaveSearches", searchTemplateFind.Id.ToString()).Text;
		configurationItem.MethodIds = new string[0];
		configurationItem.Properties = new Property[4]
		{
			SearchTemplatePropertyDefinition.CountProperty(searchTemplateFind.Count),
			SearchTemplatePropertyDefinition.StartIndexProperty(searchTemplateFind.StartIndex),
			SearchTemplatePropertyDefinition.KeywordFilterProperty(searchTemplateFind.KeywordFilter),
			SearchTemplatePropertyDefinition.PrivateOnlyProperty(searchTemplateFind.PrivateOnly)
		};
		return configurationItem;
	}
}
