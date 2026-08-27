namespace VideoOS.Management.Server;

public abstract class SearchFilter
{
	public string[] Users { get; set; }

	public string SearchString { get; set; }

	public bool SearchHeader { get; set; }

	public bool SearchDescription { get; set; }

	public bool SearchReference { get; set; }

	public bool SearchUser { get; set; }

	public string HeaderFilter { get; set; }

	public string DescriptionFilter { get; set; }
}
