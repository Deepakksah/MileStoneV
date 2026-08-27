using System;

namespace VideoOS.Management.Server;

public class UserUpdateInfo
{
	public Guid Id { get; }

	public DateTime LastModified { get; set; }

	public UserUpdateInfo(Guid id, DateTime lastModified)
	{
		Id = id;
		LastModified = lastModified;
	}
}
