namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class RecurringTimeModel
{
	public FrequencyModel Frequency { get; set; } = new FrequencyModel();

	public SubDayModel SubDay { get; set; } = new SubDayModel();
}
