using System;
using System.Collections.Generic;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.CertificateRegistration;

public interface IVmoSystemSettingClient : IDisposable
{
	IEnumerable<Address> ManagementServerAddresses { get; }

	void Connect();

	void Disconnect();

	string GetSystemSetting(string name);

	void SetSystemSetting(string name, string value);

	void RemoveSiteAddresses(IEnumerable<Address> addresses);
}
