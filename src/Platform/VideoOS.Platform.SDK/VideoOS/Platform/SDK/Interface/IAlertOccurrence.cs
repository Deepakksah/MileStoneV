using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IAlertOccurrence
{
	Guid AlertId { get; set; }

	DateTime Time { get; set; }
}
