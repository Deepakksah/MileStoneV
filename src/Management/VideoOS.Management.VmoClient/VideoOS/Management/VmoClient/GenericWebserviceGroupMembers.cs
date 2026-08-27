namespace VideoOS.Management.VmoClient;

public class GenericWebserviceGroupMembers
{
	public string GroupId { get; private set; }

	public string MemberId { get; private set; }

	public GenericWebserviceGroupMembers(string groupId, string memberId)
	{
		GroupId = groupId;
		MemberId = memberId;
	}
}
