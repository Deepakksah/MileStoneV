using System;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi.LprClient;

public interface ILPRMatchListClient
{
	void CheckInit(IGeminiRequestContext requestContext);

	AddLicensePlateListResp AddLicensePlateList(Guid listId, string name);

	DeleteLicensePlateListResp DeleteLicensePlateList(Guid listId);

	LicensePlateList[] GetLicensePlateLists();

	LicensePlateList GetLicensePlateList(Guid listId);

	LicensePlateInfo[] GetLicensePlates(Guid listId, string searchFilter = "");

	ModifyLicensePlateListResp ModifyLicensePlateList(Guid listId, string newName, FQID[] triggerEvents, LicensePlatePropertyDefinition[] customFields);

	AddLicensePlatesResp AddLicensePlates(Guid listId, LicensePlateInfo[] licensePlates);

	DeleteLicensePlatesResp DeleteLicensePlates(Guid listId, LicensePlate[] licensePlates);

	ClearLicensePlatesResp DeleteAllLicensePlates(Guid listId);
}
