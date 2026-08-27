using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.Common.DriverResources;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ConfigurationApiService : GeminiWebServiceBase, IConfigurationService
{
	public ConfigurationItem ConstructChildItem(string itemType, string parentPath)
	{
		try
		{
			throw new NotSupportedException();
		}
		catch (NotSupportedException ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	public ConfigurationItem ConstructItem(string itemType, Property[] properties)
	{
		try
		{
			throw new NotSupportedException();
		}
		catch (NotSupportedException ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	public void DeleteItem(string path)
	{
		try
		{
			throw new NotSupportedException();
		}
		catch (NotSupportedException ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	public ConfigurationItem[] GetChildItems(string path)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("GetChildItems");
			methodInformation.AddParameter("path", path);
			EnterWebMethod(methodInformation);
			ConfigurationItemPath pathInstance = null;
			IConfigurationApiSupport configurationApiSupport = null;
			try
			{
				ThrowIfPathInvalid(path, out pathInstance);
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(pathInstance.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The server doesn't support the specified path", "path");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			catch (NotSupportedException ex4)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex4);
			}
			try
			{
				return configurationApiSupport.GetChildItems(base.RequestContext, pathInstance);
			}
			catch (ObjectNotFoundException)
			{
				throw new PathNotFoundException(pathInstance.Text);
			}
		}
		catch (FaultException)
		{
			throw;
		}
		catch (PathInvalidException ex7)
		{
			string message = (string.IsNullOrEmpty(ex7.Message) ? "The path is not a valid/recognized path." : string.Format(CultureInfo.InvariantCulture, "The path is not a valid/recognized path.\n\nDetails:\n{0}", ex7.Message));
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException(message, "path", ex7));
		}
		catch (ArgumentException ex8)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex8);
		}
		catch (Exception ex9)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex9);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ConfigurationItem[] GetDependantItems(string path)
	{
		try
		{
			throw new NotSupportedException();
		}
		catch (NotSupportedException ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	public ConfigurationItem GetItem(string path)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("GetItem");
			methodInformation.AddParameter("path", path);
			EnterWebMethod(methodInformation);
			ConfigurationItemPath pathInstance = null;
			IConfigurationApiSupport configurationApiSupport = null;
			try
			{
				ThrowIfPathInvalid(path, out pathInstance);
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(pathInstance.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The server doesn't support the specified path", "path");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			try
			{
				return configurationApiSupport.GetItem(base.RequestContext, pathInstance);
			}
			catch (ObjectNotFoundException)
			{
				throw new PathNotFoundException(pathInstance.Text);
			}
		}
		catch (FaultException)
		{
			throw;
		}
		catch (PathInvalidException ex6)
		{
			string message = (string.IsNullOrEmpty(ex6.Message) ? "The path is not a valid/recognized path." : string.Format(CultureInfo.InvariantCulture, "The path is not a valid/recognized path.\n\nDetails:\n{0}", ex6.Message));
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException(message, "path", ex6));
		}
		catch (ArgumentException ex7)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex7);
		}
		catch (Exception ex8)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex8);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ConfigurationItem[] GetItems(string itemType)
	{
		try
		{
			throw new NotSupportedException();
		}
		catch (NotSupportedException ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	public MethodInfo GetMethodInfo(string methodId)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("GetMethodInfo");
			methodInformation.AddParameter("methodId", methodId);
			EnterWebMethod(methodInformation);
			return MethodRepository.GetMethod(methodId);
		}
		catch (Exception ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MethodInfo[] GetMethodInfos()
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("GetMethodInfos");
			EnterWebMethod(methodInformation);
			return MethodRepository.GetAllMethods();
		}
		catch (Exception ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Dictionary<string, string> GetTranslations(string languageId)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("GetTranslations");
			methodInformation.AddParameter("languageId", languageId);
			EnterWebMethod(methodInformation);
			CultureInfo cultureInfo = null;
			try
			{
				if (languageId == null)
				{
					throw new ArgumentNullException("languageId");
				}
				if (string.IsNullOrEmpty(languageId))
				{
					throw new ArgumentException("The parameter: 'languageid' can't contain a empty string", "languageId");
				}
				cultureInfo = new CultureInfo(languageId);
			}
			catch (ArgumentNullException ex)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex);
			}
			catch (ArgumentException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			ResourceSet resourceSet = ConfigurationApiResources.ResourceManager.GetResourceSet(cultureInfo, createIfNotExists: true, tryParents: true);
			if (resourceSet != null)
			{
				foreach (DictionaryEntry item in from DictionaryEntry f in resourceSet
					where f.Value is string && f.Key is string
					select f)
				{
					string key = (string)item.Key;
					string value = (string)item.Value;
					value = InsertOemNames(value);
					dictionary.Add(key, value);
				}
			}
			ResourceSet resourceSet2 = DriverResourcesLookup.getResourceManager().GetResourceSet(cultureInfo, createIfNotExists: true, tryParents: true);
			if (resourceSet2 != null)
			{
				foreach (DictionaryEntry item2 in from DictionaryEntry f in resourceSet2
					where f.Value is string && f.Key is string
					select f)
				{
					string key2 = (string)item2.Key;
					string value2 = (string)item2.Value;
					dictionary.Add(key2, value2);
				}
			}
			return dictionary;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (ArgumentException ex4)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex4);
		}
		catch (Exception ex5)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex5);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ConfigurationItem[] QueryItems(ItemFilter itemFilter, int maxResult)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("QueryItems");
			methodInformation.AddParameter("itemFilter", itemFilter);
			EnterWebMethod(methodInformation);
			IConfigurationApiSupport configurationApiSupport = null;
			ConfigurationItemPath configurationItemPath = null;
			try
			{
				if (itemFilter == null)
				{
					throw new ArgumentNullException("itemFilter");
				}
				if (maxResult < 1)
				{
					throw new ArgumentException("maxResult must be greater than 0");
				}
				if (itemFilter.PropertyFilters == null)
				{
					throw new ArgumentNullException("itemFilter", "ItemFilter.PropertyFilter cannot be null");
				}
				if (string.IsNullOrEmpty(itemFilter.ItemType))
				{
					throw new ArgumentException("ItemType is blank", "itemFilter");
				}
				configurationItemPath = BuildQueryRootPath(itemFilter.ItemType);
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(configurationItemPath.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The server doesn't support the specified ItemType");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			catch (NotSupportedException ex4)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex4);
			}
			return configurationApiSupport.QueryItems(base.RequestContext, configurationItemPath, itemFilter, maxResult, 0, null);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (ArgumentException ex6)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex6);
		}
		catch (Exception ex7)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex7);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static string InsertOemNames(string value)
	{
		if (value != null)
		{
			value = value.Replace("{OemClientName}", OemResources.Instance.SmartClient.ClientName);
			value = value.Replace("{MatrixApplicationName}", OemResources.Instance.Manager.MatrixApplicationName);
			value = value.Replace("{SmartWall}", OemResources.Instance.VideoWall.ApplicationNameShort);
			value = value.Replace("{MobileClientNameShort}", OemResources.Instance.MobileServer.MobileClientNameShort);
			value = value.Replace("{WebClientNameShort}", OemResources.Instance.MobileServer.WebClientNameShort);
		}
		return value;
	}

	public bool HasChildItems(string path)
	{
		try
		{
			throw new NotSupportedException();
		}
		catch (NotSupportedException ex)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex);
		}
	}

	public ConfigurationItem InvokeMethod(ConfigurationItem item, string method)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("InvokeMethod");
			methodInformation.AddParameter("item", item);
			methodInformation.AddParameter("method", method);
			EnterWebMethod(methodInformation);
			ConfigurationItemPath pathInstance = null;
			ConfigurationItemPath parentPathInstance = null;
			IConfigurationApiSupport configurationApiSupport = null;
			try
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				ThrowIfItemTypeInvalid(item.ItemType);
				ThrowIfPathsInvalid(item.Path, item.ParentPath, out pathInstance, out parentPathInstance);
				if (method == null)
				{
					throw new ArgumentNullException("method");
				}
				if (string.IsNullOrEmpty(method))
				{
					throw new ArgumentException("The method cannot be a empty string", "method");
				}
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(pathInstance.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The path on configuration item is not supported by the server", "item");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			catch (NotSupportedException ex4)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex4);
			}
			return configurationApiSupport.InvokeMethod(base.RequestContext, item, pathInstance, parentPathInstance, method);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (ConfigurationItemInvalidException)
		{
			try
			{
				throw new ArgumentException("The configuration item is invalid", "item");
			}
			catch (ArgumentException ex7)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex7);
			}
		}
		catch (PathInvalidException ex8)
		{
			string message = (string.IsNullOrEmpty(ex8.Message) ? "The path is not a valid/recognized path." : string.Format(CultureInfo.InvariantCulture, "The path is not a valid/recognized path.\n\nDetails:\n{0}", ex8.Message));
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException(message, "item", ex8));
		}
		catch (Exception ex9)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex9);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ValidateResult SetItem(ConfigurationItem item)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("SetItem");
			methodInformation.AddParameter("item", item);
			EnterWebMethod(methodInformation);
			ConfigurationItemPath pathInstance = null;
			ConfigurationItemPath parentPathInstance = null;
			IConfigurationApiSupport configurationApiSupport = null;
			try
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				ThrowIfItemTypeInvalid(item.ItemType);
				ThrowIfPathsInvalid(item.Path, item.ParentPath, out pathInstance, out parentPathInstance);
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(pathInstance.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The path on configuration item is not supported by the server", "item");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			try
			{
				return configurationApiSupport.SetItem(base.RequestContext, item, pathInstance, parentPathInstance);
			}
			catch (ObjectNotFoundException)
			{
				throw new PathNotFoundException(pathInstance.Text);
			}
		}
		catch (FaultException)
		{
			throw;
		}
		catch (PathInvalidException ex6)
		{
			try
			{
				string message = (string.IsNullOrEmpty(ex6.Message) ? "The configuration item contains an invalid/unrecognized path or parent path." : string.Format(CultureInfo.InvariantCulture, "The configuration item contains an invalid/unrecognized path or parent path.\n\nDetails:\n{0}", ex6.Message));
				throw new ArgumentException(message, "item", ex6);
			}
			catch (ArgumentException ex7)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex7);
			}
		}
		catch (ConfigurationItemInvalidException innerException)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException("The configuration item is invalid", "item", innerException));
		}
		catch (ArgumentException ex8)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex8);
		}
		catch (SecurityObjectDoesNotExistException exception)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(new PathNotFoundException("The configuration item is invalid", exception));
		}
		catch (Exception ex9)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex9);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ValidateResult ValidateItem(ConfigurationItem item)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("ValidateItem");
			methodInformation.AddParameter("item", item);
			EnterWebMethod(methodInformation);
			ConfigurationItemPath pathInstance = null;
			ConfigurationItemPath parentPathInstance = null;
			IConfigurationApiSupport configurationApiSupport = null;
			try
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				ThrowIfPathsInvalid(item.Path, item.ParentPath, out pathInstance, out parentPathInstance);
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(pathInstance.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The path on configuration item is not supported by the server", "item");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			catch (NotSupportedException ex4)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex4);
			}
			try
			{
				return configurationApiSupport.ValidateItem(base.RequestContext, item, pathInstance, parentPathInstance);
			}
			catch (ObjectNotFoundException)
			{
				throw new PathNotFoundException(pathInstance.Text);
			}
		}
		catch (FaultException)
		{
			throw;
		}
		catch (PathInvalidException ex7)
		{
			string message = (string.IsNullOrEmpty(ex7.Message) ? "The configuration item contains an invalid/unrecognized path or parent path." : string.Format(CultureInfo.InvariantCulture, "The configuration item contains an invalid/unrecognized path or parent path.\n\nDetails:\n{0}", ex7.Message));
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException(message, "item", ex7));
		}
		catch (ConfigurationItemInvalidException innerException)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException("The configuration item is invalid", "item", innerException));
		}
		catch (ArgumentException ex8)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex8);
		}
		catch (Exception ex9)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex9);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ConfigurationItem[] GetChildItemsHierarchy(string path, string[] itemTypes, ItemFilter[] itemFilters)
	{
		try
		{
			CheckConnect();
			MethodInformation methodInformation = new MethodInformation("GetChildItemsHierarchy");
			methodInformation.AddParameter("path", path);
			EnterWebMethod(methodInformation);
			ConfigurationItemPath pathInstance = null;
			IConfigurationApiSupport configurationApiSupport = null;
			try
			{
				ThrowIfPathInvalid(path, out pathInstance);
				if (itemTypes == null)
				{
					throw new ArgumentNullException("itemTypes");
				}
				if (itemFilters == null)
				{
					throw new ArgumentNullException("itemFilters");
				}
				foreach (ItemFilter filter in itemFilters)
				{
					if (string.IsNullOrEmpty(filter.ItemType))
					{
						throw new ArgumentNullException("itemFilters", "ItemType not valid");
					}
					if (!itemTypes.Contains(filter.ItemType))
					{
						throw new ArgumentException("Filters ItemType needs to be defined in the itemTypes array");
					}
					if (itemFilters.Where((ItemFilter f) => f.ItemType == filter.ItemType).Count() > 1)
					{
						throw new ArgumentException("Only one ItemFilter can be defined for an ItemType");
					}
				}
				try
				{
					configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject(pathInstance.ItemType);
				}
				catch (NotSupportedException)
				{
					throw new ArgumentException("The server doesn't support the specified path", "path");
				}
			}
			catch (ArgumentNullException ex2)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex2);
			}
			catch (ArgumentException ex3)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex3);
			}
			catch (NotSupportedException ex4)
			{
				throw ConfigurationApiExceptionUtilities.HandleException(ex4);
			}
			try
			{
				return configurationApiSupport.GetChildItems(base.RequestContext, pathInstance, itemTypes, itemFilters, int.MaxValue, 0, itemTypes, itemFilters);
			}
			catch (ObjectNotFoundException)
			{
				throw new PathNotFoundException(pathInstance.Text);
			}
		}
		catch (FaultException)
		{
			throw;
		}
		catch (PathInvalidException ex7)
		{
			string message = (string.IsNullOrEmpty(ex7.Message) ? "The path is not a valid/recognized path." : string.Format(CultureInfo.InvariantCulture, "The path is not a valid/recognized path.\n\nDetails:\n{0}", ex7.Message));
			throw ConfigurationApiExceptionUtilities.HandleException(new ArgumentException(message, "path", ex7));
		}
		catch (ArgumentException ex8)
		{
			throw ConfigurationApiExceptionUtilities.CreateServerException(ex8);
		}
		catch (Exception ex9)
		{
			throw ConfigurationApiExceptionUtilities.HandleException(ex9);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private static ConfigurationItemPath BuildQueryRootPath(string itemType)
	{
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => string.Equals(r.ItemType, itemType, StringComparison.Ordinal));
		if (deviceTypeRelation != null)
		{
			return ConfigurationItemPath.CreatePath(string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", deviceTypeRelation.GroupType, deviceTypeRelation.AllFolderId, deviceTypeRelation.FolderType));
		}
		return ConfigurationItemPath.CreateSystemFolderPath(itemType + "Folder");
	}

	private static void ThrowIfPathInvalid(string path, out ConfigurationItemPath pathInstance)
	{
		pathInstance = null;
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		try
		{
			pathInstance = ConfigurationItemPath.CreatePath(path);
		}
		catch (ArgumentException innerException)
		{
			throw new ArgumentException("The path on configuration item is not a valid/recognized path", "path", innerException);
		}
	}

	private static void ThrowIfPathsInvalid(string path, string parentPath, out ConfigurationItemPath pathInstance, out ConfigurationItemPath parentPathInstance)
	{
		pathInstance = null;
		parentPathInstance = null;
		ThrowIfPathInvalid(path, out pathInstance);
		if (!string.Equals(pathInstance.ItemType, "System", StringComparison.Ordinal))
		{
			if (parentPath == null)
			{
				throw new ArgumentNullException("parentPath");
			}
			try
			{
				parentPathInstance = ConfigurationItemPath.CreatePath(parentPath);
			}
			catch (ArgumentException innerException)
			{
				throw new ArgumentException("The path on configuration item is not a valid/recognized path", "parentPath", innerException);
			}
		}
	}

	private static void ThrowIfItemTypeInvalid(string itemType)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (string.IsNullOrEmpty(itemType))
		{
			throw new ArgumentException("The itemtype cannot be an empty string", "itemType");
		}
	}
}
