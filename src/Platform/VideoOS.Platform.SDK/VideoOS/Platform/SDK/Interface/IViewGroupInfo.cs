using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IViewGroupInfo
{
	Guid Id { get; set; }

	string Name { get; set; }

	ViewGroupType ViewGroupType { get; set; }

	bool ReadOnly { get; set; }

	DateTime LastModifiedTime { get; set; }

	string LastModifiedUser { get; set; }
}
