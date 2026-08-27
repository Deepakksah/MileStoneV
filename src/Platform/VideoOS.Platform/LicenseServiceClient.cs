using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Platform.License;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class LicenseServiceClient : ClientBase<ILicenseService>, ILicenseService
{
	public LicenseServiceClient()
	{
	}

	public LicenseServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public LicenseServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LicenseServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public LicenseServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public Guid RegisterConcurrentUse(ConcurrentLicenseUsed concurrentLicenseUsed)
	{
		return base.Channel.RegisterConcurrentUse(concurrentLicenseUsed);
	}

	public void ReRegisterConcurrentUse(Guid registerId)
	{
		base.Channel.ReRegisterConcurrentUse(registerId);
	}

	public void UnRegisterConcurrentUse(Guid registerId)
	{
		base.Channel.UnRegisterConcurrentUse(registerId);
	}

	public ConcurrentLicenseUsed[] GetConcurrentLicenses(Guid pluginId, string licenseType)
	{
		return base.Channel.GetConcurrentLicenses(pluginId, licenseType);
	}
}
