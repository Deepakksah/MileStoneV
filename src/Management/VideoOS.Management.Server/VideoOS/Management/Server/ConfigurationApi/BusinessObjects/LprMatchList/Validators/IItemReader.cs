using System;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;

public interface IItemReader
{
	ConfigurationItem Item { get; }

	LicensePlateList[] LicensePlateLists { get; }

	LicensePlateList ItemLicensePlateList { get; }

	Guid? Id { get; }

	string Name { get; }

	string[] TriggeredEvents { get; }

	string[] CustomFields { get; }
}
