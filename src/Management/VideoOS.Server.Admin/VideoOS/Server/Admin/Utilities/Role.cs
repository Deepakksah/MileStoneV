using System;
using VideoOS.Management.Common;

namespace VideoOS.Server.Admin.Utilities;

public sealed class Role
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public RoleType Type { get; set; }

	public DateTime LastModified { get; set; }
}
