using System;

namespace VideoOS.Platform.Proxy.SecurityApi;

public class ObjectPermissionsModel
{
	public Guid id { get; set; }

	public ulong mask { get; set; }
}
