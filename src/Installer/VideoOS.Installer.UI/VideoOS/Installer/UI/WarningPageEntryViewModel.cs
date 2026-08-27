using VideoOS.Installer.Interfaces.Content;

namespace VideoOS.Installer.UI;

public class WarningPageEntryViewModel
{
	public string Heading => Content.Heading;

	public ContentLinkText Details => Content.Details;

	public string Subheading => Content.Subheading;

	public ContentLinkText Subsection => Content.Subsection;

	public WarningPageContent Content { get; set; }

	public WarningPageEntryViewModel(WarningPageContent content)
	{
		Content = content;
	}
}
