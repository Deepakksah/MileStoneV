namespace VideoOS.Common.Integration;

public class Membership
{
	public string Container { get; private set; }

	public string MemberSid { get; private set; }

	public Membership(string container, string memberSid)
	{
		Container = container;
		MemberSid = memberSid;
	}
}
