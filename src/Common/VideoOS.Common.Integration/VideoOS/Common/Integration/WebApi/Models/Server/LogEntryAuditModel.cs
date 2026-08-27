namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class LogEntryAuditModel : LogEntryModel
{
	public string AccessGrantedValue { get; set; }

	public string AuditTypeValue { get; set; }

	public string UserCredentials { get; set; }

	public string Location { get; set; }

	public string ResourceID { get; set; }

	public string ResourceTypeValue { get; set; }

	public string ResourceName { get; set; }

	public string ResourceLocation { get; set; }
}
