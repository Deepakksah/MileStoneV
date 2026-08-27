namespace VideoOS.Common.Controls.Reporting;

public class FrontPageSection
{
	public string Title { get; set; }

	public string Text { get; set; }

	public FrontPageSection()
	{
	}

	public FrontPageSection(string title, string text)
	{
		Title = title;
		Text = text;
	}
}
