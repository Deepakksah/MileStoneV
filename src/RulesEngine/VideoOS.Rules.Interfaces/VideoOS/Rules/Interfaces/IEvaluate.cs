using System.Collections;

namespace VideoOS.Rules.Interfaces;

public interface IEvaluate
{
	void Evaluate(TriggerEvent triggerEvent, Stack resultStack);
}
