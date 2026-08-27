using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.LprMatchList;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Validators;
using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPILprMatchList")]
[EntityName("ConfigAPILprMatchListFolder")]
public class LprMatchListConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private readonly IConfigurationApiSupport _service;

	public LprMatchListConfigurationApi()
	{
		LPRMatchListClient client = new LPRMatchListClient(new PluginCommandClientProvider(), () => EnvironmentManager.Instance);
		FQIDConverter fqidConverter = new FQIDConverter();
		StringCollectionConverter stringCollectionConverter = new StringCollectionConverter(new StringSplitter());
		LicensePlateItemsBuilder licensePlateItemsBuilder = new LicensePlateItemsBuilder(fqidConverter, stringCollectionConverter);
		CommonValidator commonValidator = new CommonValidator();
		ItemValidator itemValidator = new ItemValidator(commonValidator);
		ConfigurationItemBuilder itemBuilder = new ConfigurationItemBuilder(itemValidator, fqidConverter, stringCollectionConverter);
		ItemReaderFactory itemReaderFactory = (ConfigurationItem item, LicensePlateList[] lists) => new ItemReader(item, lists);
		MethodProvider folderMethodProvider = new MethodProvider(new IMethod[2]
		{
			new AddMatchList(client, itemBuilder),
			new RemoveMatchList(client)
		});
		MethodProvider itemMethodProvider = new MethodProvider(new IMethod[5]
		{
			new GetCustomFieldsForRegistrationNumber(client, stringCollectionConverter),
			new GetRegistrationNumbersInfo(client, stringCollectionConverter),
			new AddEditRegistrationNumbersInfo(client, licensePlateItemsBuilder),
			new DeleteRegistrationNumbers(client, licensePlateItemsBuilder),
			new DeleteAllRegistrationNumbers(client)
		});
		_service = new VideoOS.Management.Server.ConfigurationApi.LprMatchList.ConfigurationApiService(client, itemBuilder, licensePlateItemsBuilder, folderMethodProvider, itemMethodProvider, commonValidator, itemValidator, itemReaderFactory);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return _service.GetChildItems(requestContext, path);
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return _service.GetItem(requestContext, path);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return _service.InvokeMethod(requestContext, item, path, parentPath, method);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return _service.SetItem(requestContext, item, path, parentPath);
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		MIPFactory.CheckAndInitialize(requestContext);
		return _service.ValidateItem(requestContext, item, path, parentPath);
	}
}
