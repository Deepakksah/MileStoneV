namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class CurrentConfigurationVersionsModel
{
	public long ConfigVersion { get; set; }

	public long SecurityVersion { get; set; }

	public long TokenVersion { get; set; }
}
