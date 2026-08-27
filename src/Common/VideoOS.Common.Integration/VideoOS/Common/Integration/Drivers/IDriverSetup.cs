using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriverSetup : IDriver, IDisposable
{
	void SetupNewDriver(string activationDefinitionXml, Uri uri, string userName, string password, string driverProxyName, string customDriverData, string initialDriverSettings, Action stillAlive);

	void SetupNewTransientDriver(string activationDefinitionXml, Uri uri, string userName, string password, string driverProxyName, string customDriverData, string initialDriverSettings);
}
