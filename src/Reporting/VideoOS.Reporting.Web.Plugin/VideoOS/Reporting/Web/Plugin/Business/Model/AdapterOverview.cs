namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class AdapterOverview
{
	public string AdapterName { get; set; }

	public AdapterCounter AdapterTotal { get; set; }

	public AdapterCounter AdapterReceived { get; set; }

	public AdapterCounter AdapterSent { get; set; }
}
