using System;

namespace VideoOS.CertificateRegistration.Common;

public interface IHttpSysSettingsFactory
{
	IHttpSysSettings Create();

	IHttpSysSettings Create(Guid appId, int webApiPort);
}
