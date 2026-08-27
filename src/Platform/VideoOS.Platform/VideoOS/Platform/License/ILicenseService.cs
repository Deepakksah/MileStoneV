using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace VideoOS.Platform.License;

[ServiceContract]
public interface ILicenseService
{
	[OperationContract]
	Guid RegisterConcurrentUse(ConcurrentLicenseUsed concurrentLicenseUsed);

	[OperationContract]
	void ReRegisterConcurrentUse(Guid registerId);

	[OperationContract]
	void UnRegisterConcurrentUse(Guid registerId);

	[OperationContract]
	List<ConcurrentLicenseUsed> GetConcurrentLicenses(Guid pluginId, string licenseType);
}
