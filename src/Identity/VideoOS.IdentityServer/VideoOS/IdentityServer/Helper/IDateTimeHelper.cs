using System;

namespace VideoOS.IdentityServer.Helper;

public interface IDateTimeHelper
{
	DateTimeOffset UtcNow { get; }
}
