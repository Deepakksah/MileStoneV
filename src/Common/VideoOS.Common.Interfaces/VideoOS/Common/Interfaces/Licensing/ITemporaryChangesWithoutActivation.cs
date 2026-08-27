using System;
using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ITemporaryChangesWithoutActivation
{
	HashSet<string> LoadTemporaryHardwareChangesWithoutActivationIdentifiers(DateTime licenseActivatedDate);

	HashSet<string> LoadTemporaryDeviceChangesWithoutActivationIdentifiers(DateTime licenseActivatedDate);
}
