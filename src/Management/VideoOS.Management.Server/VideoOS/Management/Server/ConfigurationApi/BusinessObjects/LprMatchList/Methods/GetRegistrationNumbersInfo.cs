using System;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;

public class GetRegistrationNumbersInfo : IMethod
{
	private readonly ILPRMatchListClient _client;

	private readonly IStringCollectionConverter _stringCollectionConverter;

	public string Id => "MethodIdGetRegistrationNumbersInfo";

	public GetRegistrationNumbersInfo(ILPRMatchListClient client, IStringCollectionConverter stringCollectionConverter)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\GetRegistrationNumbersInfo.cs", 27);
		_client = client;
		_stringCollectionConverter = stringCollectionConverter;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return InvokeMethod(MethodTypesHelper.CreateInvokeInfo(Id, ConfigurationApiResources.MethodGetRegistrationNumbersInfo, item), path);
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item, ConfigurationItemPath path)
	{
		LicensePlateInfo[] licensePlates = _client.GetLicensePlates(new Guid(path.Id));
		string collectionString = CreatePlatesDataListString(licensePlates);
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true, CreateRegistrationNumbersProperty(collectionString));
	}

	private string CreatePlatesDataListString(LicensePlateInfo[] plates)
	{
		string[][] enumerable = plates.Select((LicensePlateInfo p) => new string[1] { p.LicensePlate.RegistrationNumber }.Concat(from s in p.Properties
			orderby s.ColumnIndex
			select s.Value).ToArray()).ToArray();
		return _stringCollectionConverter.EnumerableToString(enumerable);
	}

	private static Property CreateRegistrationNumbersProperty(string collectionString)
	{
		return GeneralPropertyDefinitions.GetListProperty(collectionString, "RegistrationNumbers", ConfigurationApiResources.PropertyMatchListRegistrationNumbersDataList, "PropertyMatchListRegistrationNumbersDataList", isSettable: false);
	}
}
