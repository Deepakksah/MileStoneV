using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;

namespace VideoOS.ConfigurationAPI;

[ServiceContract(Namespace = "http://VideoOS.Net/ConfigurationService")]
public interface IConfigurationService
{
	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem GetItem(string path);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem[] GetItems(string itemType);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ValidateResult SetItem(ConfigurationItem item);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	bool HasChildItems(string path);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem[] GetChildItems(string path);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem ConstructItem(string itemType, Property[] properties);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem ConstructChildItem(string itemType, string parentPath);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ValidateResult ValidateItem(ConfigurationItem item);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	void DeleteItem(string path);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem[] GetDependantItems(string path);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	MethodInfo[] GetMethodInfos();

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	MethodInfo GetMethodInfo(string methodId);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem InvokeMethod(ConfigurationItem item, string method);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	Dictionary<string, string> GetTranslations(string languageId);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem[] GetChildItemsHierarchy(string path, string[] itemTypes, ItemFilter[] itemFilters);

	[FaultContract(typeof(ArgumentExceptionFault))]
	[FaultContract(typeof(ArgumentNullExceptionFault))]
	[FaultContract(typeof(LicenseExceptionFault))]
	[FaultContract(typeof(NotSupportedExceptionFault))]
	[FaultContract(typeof(PathNotFoundExceptionFault))]
	[FaultContract(typeof(UnauthorizedAccessFault))]
	[FaultContract(typeof(ServerExceptionFault))]
	[OperationContract]
	ConfigurationItem[] QueryItems(ItemFilter itemFilter, int maxResult);
}
