using System;

namespace VideoOS.Common.Integration.Drivers.Arcules;

public interface IGuidVerifier
{
	bool IsMatchingGuid(Guid id);
}
