namespace VideoOS.Reporting.Web.Plugin.Live;

public class LiveResult : ILiveResult
{
	public Unit DisplayUnit { get; set; }

	public float DisplayValue { get; set; }

	public float ValueInPercent { get; set; }

	public bool Error { get; set; }
}
