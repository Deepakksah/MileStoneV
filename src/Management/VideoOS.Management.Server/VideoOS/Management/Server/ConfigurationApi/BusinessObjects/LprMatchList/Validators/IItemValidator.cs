using System;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Validators;

public interface IItemValidator
{
	string[] ValidEventItemTypes { get; }

	ValidateResult Validate(IItemReader item, ConfigurationItemPath path);

	void ValidateUniqueName(Guid itemId, string name, LicensePlateList[] licensePlateLists);

	ValidateResult CreateOkResult(ConfigurationItem item);
}
