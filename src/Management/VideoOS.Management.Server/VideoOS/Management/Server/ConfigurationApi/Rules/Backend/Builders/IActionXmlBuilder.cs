using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public interface IActionXmlBuilder
{
	string GetXml(ActionModel action);
}
