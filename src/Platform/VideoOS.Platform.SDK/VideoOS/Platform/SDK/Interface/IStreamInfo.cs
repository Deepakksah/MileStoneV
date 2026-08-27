using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IStreamInfo
{
	Guid StreamId { get; set; }

	string Name { get; set; }

	bool Default { get; set; }
}
