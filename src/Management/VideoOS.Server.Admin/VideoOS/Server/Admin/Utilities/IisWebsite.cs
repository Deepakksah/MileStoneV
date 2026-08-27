namespace VideoOS.Server.Admin.Utilities;

public class IisWebsite
{
	public string Name { get; set; }

	public long Id { get; set; }

	public IisWebsiteStatus Status { get; set; }

	public IisWebsiteState State { get; set; }

	public IisWebsite(string name, long id, IisWebsiteState state)
	{
		Name = name;
		Id = id;
		Status = IisWebsiteStatus.Unknown;
		State = state;
	}
}
