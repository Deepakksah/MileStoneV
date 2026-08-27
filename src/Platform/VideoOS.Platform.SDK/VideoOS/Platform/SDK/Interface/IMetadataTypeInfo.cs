using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IMetadataTypeInfo
{
	Guid Id { get; set; }

	string Name { get; set; }

	Guid DisplayId { get; set; }

	string DisplayName { get; set; }

	int ValidTime { get; set; }
}
