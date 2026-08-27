using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.LPR.Common.WCF.Client;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Validators;
using VideoOS.Platform;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.LprMatchList;

public class ConfigurationApiService : ConfigurationApiBase, IConfigurationApiSupport
{
	private readonly ILPRMatchListClient _client;

	private readonly IConfigurationItemBuilder _itemBuilder;

	private readonly ILicensePlateItemsBuilder _licensePlateItemBuilder;

	private readonly IMethodProvider _folderMethodProvider;

	private readonly IMethodProvider _itemMethodProvider;

	private readonly ICommonValidator _commonValidator;

	private readonly IItemValidator _itemValidator;

	private readonly ItemReaderFactory _itemReaderFactory;

	public ConfigurationApiService(ILPRMatchListClient client, IConfigurationItemBuilder itemBuilder, ILicensePlateItemsBuilder licensePlateItemBuilder, IMethodProvider folderMethodProvider, IMethodProvider itemMethodProvider, ICommonValidator commonValidator, IItemValidator itemValidator, ItemReaderFactory itemReaderFactory)
	{
		_client = client;
		_itemBuilder = itemBuilder;
		_licensePlateItemBuilder = licensePlateItemBuilder;
		_folderMethodProvider = folderMethodProvider;
		_itemMethodProvider = itemMethodProvider;
		_commonValidator = commonValidator;
		_itemValidator = itemValidator;
		_itemReaderFactory = itemReaderFactory;
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		PrepareRequest(requestContext, path);
		if (!path.IsFolder)
		{
			return new ConfigurationItem[0];
		}
		LicensePlateList[] array = null;
		try
		{
			array = _client.GetLicensePlateLists();
		}
		catch (ServiceFaultException)
		{
		}
		if (array == null || !array.Any())
		{
			return new ConfigurationItem[0];
		}
		List<ConfigurationItem> list = new List<ConfigurationItem>(array.Length);
		LicensePlateList[] array2 = array;
		foreach (LicensePlateList licensePlateList in array2)
		{
			ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("LprMatchList", licensePlateList.Id.ToString());
			list.Add(BuildItem(path2, path, licensePlateList));
		}
		return list.ToArray();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		PrepareRequest(requestContext, path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("LprMatchListFolder", ConfigurationItemPath.CreateSystemPath());
		}
		LicensePlateList licensePlateList = _client.GetLicensePlateList(new Guid(path.Id));
		if (licensePlateList == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return BuildItem(path, CreateParentPath(), licensePlateList);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		item.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\ConfigurationApiService.cs", 101);
		PrepareRequest(requestContext, path);
		if (path.IsFolder)
		{
			return _folderMethodProvider.Provide(item, method).Invoke(requestContext, item, path, parentPath);
		}
		return _itemMethodProvider.Provide(item, method).Invoke(requestContext, item, path, parentPath);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\ConfigurationApiService.cs", 115);
		_commonValidator.ValidatePath(parentPath);
		PrepareRequest(requestContext, path);
		LicensePlateList[] licensePlateLists = _client.GetLicensePlateLists();
		IItemReader itemReader = _itemReaderFactory(item, licensePlateLists);
		ValidateResult validateResult = _itemValidator.Validate(itemReader, path);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		FQID[] triggerEvents = _licensePlateItemBuilder.BuildTriggeredEventsIds(requestContext, itemReader.TriggeredEvents);
		LicensePlatePropertyDefinition[] customFields = _licensePlateItemBuilder.BuildCustomFields(itemReader.CustomFields);
		_client.ModifyLicensePlateList(itemReader.Id.Value, itemReader.Name, triggerEvents, customFields);
		return _itemValidator.CreateOkResult(GetItem(requestContext, path));
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\ConfigurationApiService.cs", 135);
		_commonValidator.ValidatePath(parentPath);
		PrepareRequest(requestContext, path);
		LicensePlateList[] licensePlateLists = _client.GetLicensePlateLists();
		IItemReader item2 = _itemReaderFactory(item, licensePlateLists);
		return _itemValidator.Validate(item2, path);
	}

	private ConfigurationItem BuildItem(ConfigurationItemPath path, ConfigurationItemPath parentPath, LicensePlateList matchList)
	{
		LicensePlateInfo[] licensePlates = _client.GetLicensePlates(matchList.Id);
		return _itemBuilder.Build(path, parentPath, matchList, licensePlates, _itemMethodProvider.GetIds());
	}

	private void PrepareRequest(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "PrepareRequest", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\ConfigurationApiService.cs", 151);
		_commonValidator.ValidatePath(path);
		_client.CheckInit(requestContext);
	}

	private static ConfigurationItemPath CreateParentPath()
	{
		return new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "LprMatchListFolder").CreatePath();
	}
}
