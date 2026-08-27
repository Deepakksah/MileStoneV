using System;

namespace VideoOS.Management.Server;

public interface IAudioMessageItem
{
	string Id { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	DateTime LastModified { get; set; }

	string OriginalFileName { get; set; }
}
