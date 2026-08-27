using System.Xml.XPath;

namespace VideoOS.Rules.Interfaces;

public interface IRuleCondition
{
	void Initialize(IXPathNavigable conditionsNode, IXPathNavigable ruleTypeNode, IRuleEngine ruleEngine);

	bool IsTrue(TriggerEvent triggerEvent);
}
