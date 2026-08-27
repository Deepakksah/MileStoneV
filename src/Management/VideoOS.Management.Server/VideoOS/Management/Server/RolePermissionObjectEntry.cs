namespace VideoOS.Management.Server;

public class RolePermissionObjectEntry
{
	public string ObjectId { get; set; }

	public string ActionName { get; set; }

	public string RoleId { get; set; }

	public RolePermissionObjectEntry()
	{
	}

	public RolePermissionObjectEntry(string objectId, string actionName)
	{
		ObjectId = objectId;
		ActionName = actionName;
	}
}
