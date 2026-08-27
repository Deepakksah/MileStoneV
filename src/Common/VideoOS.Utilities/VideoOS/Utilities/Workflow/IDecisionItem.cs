namespace VideoOS.Utilities.Workflow;

public interface IDecisionItem
{
	void CalculateNewValue();

	void SaveNewValue();

	bool IsDifferent();

	object GetNewestValue();
}
