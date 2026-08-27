using System;

namespace VideoOS.IdentityServer.Helper;

public class DateTimeHelper : IDateTimeHelper
{
	public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
