using System;

namespace VideoOS.Platform.Client;

public class ViewGroupInfo
{
	public Guid Id { get; set; }

	public DateTime LastModifiedTime { get; set; }

	public string LastModifiedUser { get; set; }

	public string Name { get; set; }

	public bool ReadOnly { get; set; }

	public ViewGroupType ViewGroupType { get; set; }
}
