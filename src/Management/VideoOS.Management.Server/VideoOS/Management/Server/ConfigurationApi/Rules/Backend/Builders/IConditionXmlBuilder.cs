using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public interface IConditionXmlBuilder
{
	string GetXml(IDictionary<ConditionType, ConditionModel> conditions);
}
