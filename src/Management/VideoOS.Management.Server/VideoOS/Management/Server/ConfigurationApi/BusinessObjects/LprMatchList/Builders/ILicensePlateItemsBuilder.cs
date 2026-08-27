using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;

public interface ILicensePlateItemsBuilder
{
	LicensePlatePropertyDefinition[] BuildCustomFields(string[] customFields);

	LicensePlateInfo[] BuildLicensePlateInfos(string registrationNumbersInfoSeparatedList);

	LicensePlate[] BuildLicensePlates(string registrationNumbersSeparatedList);

	FQID[] BuildTriggeredEventsIds(IGeminiRequestContext request, string[] triggeredEvents);
}
