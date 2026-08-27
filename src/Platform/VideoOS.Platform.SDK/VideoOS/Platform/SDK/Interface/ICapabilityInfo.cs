using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ICapabilityInfo
{
	Guid CapabilityId { get; set; }

	string Name { get; set; }

	bool Absolute { get; set; }

	bool Relative { get; set; }

	bool Start { get; set; }

	bool Stop { get; set; }

	bool Speed { get; set; }

	bool Automatic { get; set; }
}
