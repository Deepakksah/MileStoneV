namespace VideoOS.Management.Server.RestApi.Models;

public class EffectivePermissionsModel
{
	public string id { get; set; }

	public bool isAdministrator { get; set; }

	public ObjectPermissionsModel[] objectPermissions { get; set; }
}
