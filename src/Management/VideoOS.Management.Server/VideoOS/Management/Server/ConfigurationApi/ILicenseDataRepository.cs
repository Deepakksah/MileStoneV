using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Management.Server.ConfigurationApi;

public interface ILicenseDataRepository
{
	void HasPermission(IGeminiRequestContext requestContext, string actionId);

	string GetLicenseInformationXml();

	ILicenseDocument QueryLicenseDocument(IGeminiRequestContext requestContext);

	string QueryMIPLicenseItem(IGeminiRequestContext requestContext);

	string GetServerId(IGeminiRequestContext requestContext);

	string QuerySystemSetting(IGeminiRequestContext requestContext, string id);

	bool SetSystemSetting(IGeminiRequestContext requestContext, string id, string value);

	Task CreateLicenseActivateTask(IGeminiRequestContext requestContext, string username, string password);

	void UpdateLicense(IGeminiRequestContext requestContext, byte[] license, string ownerXml, string siteInformation);

	byte[] RequestLicense();

	Version GetVersion(IGeminiRequestContext requestContext);
}
