using System;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public interface IValueValidatorFactory
{
	IValueValidator GetValidator(Type validatorType);

	IValueValidator GetValidator(EventGroupType eventGroupType, bool isGroup);
}
