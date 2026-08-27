using System;

namespace VideoOS.WebService.Common.Framework;

public interface ITimeoutSettings
{
	TimeSpan Timeout { get; set; }
}
