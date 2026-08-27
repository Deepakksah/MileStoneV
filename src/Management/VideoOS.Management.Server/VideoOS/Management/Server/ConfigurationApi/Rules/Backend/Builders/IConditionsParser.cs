using System.Collections.Generic;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public interface IConditionsParser
{
	bool TryParseConditions(XmlNode conditionsNode, out List<ConditionModel> conditions);
}
