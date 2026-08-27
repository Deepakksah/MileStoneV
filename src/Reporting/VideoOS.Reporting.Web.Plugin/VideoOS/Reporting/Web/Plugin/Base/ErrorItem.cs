namespace VideoOS.Reporting.Web.Plugin.Base;

public class ErrorItem
{
	public int ErrorNumber { get; set; }

	public string ErrorDescription { get; set; }

	public ErrorItem(int errorNumber, string errorDescription)
	{
		ErrorNumber = errorNumber;
		ErrorDescription = errorDescription;
	}
}
