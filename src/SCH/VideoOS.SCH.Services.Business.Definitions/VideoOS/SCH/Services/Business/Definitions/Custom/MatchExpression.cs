namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class MatchExpression
{
	public MessageData Data { get; set; }

	public string OperatorName { get; set; }

	public MatchExpression(MessageData data, string operatorName)
	{
		Data = data;
		OperatorName = operatorName;
	}
}
