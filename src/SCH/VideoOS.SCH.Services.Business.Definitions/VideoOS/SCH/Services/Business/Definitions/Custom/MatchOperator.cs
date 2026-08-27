namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class MatchOperator
{
	public string Name { get; set; }

	public string Description { get; set; }

	public MatchOperator(string name, string description)
	{
		Name = name;
		Description = description;
	}
}
