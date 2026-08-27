using System;
using System.Linq;

namespace VideoOS.Platform.Proxy.SecurityApi;

public class EffectivePermissionsModel
{
	public string id { get; set; }

	public bool isAdministrator { get; set; }

	public ObjectPermissionsModel[] objectPermissions { get; set; }

	public ObjectPermissionsModel LookupObject(Guid objectId)
	{
		return objectPermissions.FirstOrDefault((ObjectPermissionsModel op) => op.id == objectId);
	}
}
