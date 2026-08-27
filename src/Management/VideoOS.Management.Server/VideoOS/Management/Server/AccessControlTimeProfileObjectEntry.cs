namespace VideoOS.Management.Server;

public class AccessControlTimeProfileObjectEntry
{
	public string ObjectId { get; set; }

	public string ActionName { get; set; }

	public string TimeProfileId { get; set; }

	public bool IsDefaultTimeProfile { get; set; }

	public string RoleId { get; set; }
}
