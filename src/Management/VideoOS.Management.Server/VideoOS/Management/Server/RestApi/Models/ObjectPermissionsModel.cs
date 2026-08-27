using System;

namespace VideoOS.Management.Server.RestApi.Models;

public class ObjectPermissionsModel
{
	public Guid id { get; set; }

	public ulong mask { get; set; }
}
