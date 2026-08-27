namespace VideoOS.Installer.Interfaces.Content;

public class WarningPageContent
{
	public string Heading { get; }

	public ContentLinkText Details { get; }

	public string Subheading { get; }

	public ContentLinkText Subsection { get; }

	public int Rank { get; }

	public WarningPageContent(string heading, ContentLinkText details, string subheading, ContentLinkText subsection, int rank)
	{
		Heading = heading;
		Details = details;
		Subheading = subheading;
		Subsection = subsection;
		Rank = rank;
	}
}
