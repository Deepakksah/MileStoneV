using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Platform.License;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[ServiceContract(ConfigurationName = "ILicenseService")]
public interface ILicenseService
{
	[OperationContract(Action = "http://tempuri.org/ILicenseService/RegisterConcurrentUse", ReplyAction = "http://tempuri.org/ILicenseService/RegisterConcurrentUseResponse")]
	Guid RegisterConcurrentUse(ConcurrentLicenseUsed concurrentLicenseUsed);

	[OperationContract(Action = "http://tempuri.org/ILicenseService/ReRegisterConcurrentUse", ReplyAction = "http://tempuri.org/ILicenseService/ReRegisterConcurrentUseResponse")]
	void ReRegisterConcurrentUse(Guid registerId);

	[OperationContract(Action = "http://tempuri.org/ILicenseService/UnRegisterConcurrentUse", ReplyAction = "http://tempuri.org/ILicenseService/UnRegisterConcurrentUseResponse")]
	void UnRegisterConcurrentUse(Guid registerId);

	[OperationContract(Action = "http://tempuri.org/ILicenseService/GetConcurrentLicenses", ReplyAction = "http://tempuri.org/ILicenseService/GetConcurrentLicensesResponse")]
	ConcurrentLicenseUsed[] GetConcurrentLicenses(Guid pluginId, string licenseType);
}
