using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IGroupInfo
{
	Guid GroupId { get; set; }

	string Name { get; set; }

	string Description { get; set; }
}
