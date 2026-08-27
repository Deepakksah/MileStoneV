namespace VideoOS.Reporting.Web.Plugin.Live;

public interface ILiveResult
{
	Unit DisplayUnit { get; }

	float DisplayValue { get; }

	float ValueInPercent { get; }

	bool Error { get; }
}
